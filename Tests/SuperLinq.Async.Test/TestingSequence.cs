using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using static Test.Async.TestingSequence;

namespace Test.Async;

internal static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) =>
		new(elements.ToAsyncEnumerable(), Options.None, maxEnumerations: 1);

	internal static TestingSequence<T> OfWithFailure<T>(params T[] elements) =>
		new(elements.ToAsyncEnumerable().Concat(new AsyncBreakingSequence<T>()), Options.None, maxEnumerations: 1);

	internal static TestingSequence<T> Of<T>(Options options, params T[] elements) =>
		new(elements.ToAsyncEnumerable(), options, maxEnumerations: 1);

	internal static TestingSequence<T> AsTestingSequence<T>(
		this IEnumerable<T> source,
		Options options = Options.None,
		int maxEnumerations = 1) =>
			source is not null ? new TestingSequence<T>(source.ToAsyncEnumerable(), options, maxEnumerations)
			: throw new ArgumentNullException(nameof(source));

	internal static TestingSequence<T> AsTestingSequence<T>(
		this IAsyncEnumerable<T> source,
		Options options = Options.None,
		int maxEnumerations = 1) =>
			source is not null ? new TestingSequence<T>(source, options, maxEnumerations)
			: throw new ArgumentNullException(nameof(source));

	internal static void AssertTestingSequence([DoesNotReturnIf(false)] bool expectation, string message, [CallerArgumentExpression(nameof(expectation))] string? expr = "")
	{
		if (!expectation)
			throw new TestingSequenceException($"{message}\nExpected `{expr}` to be `true`.");
	}

	internal const string ExpectedDisposal = "Expected sequence to be disposed.";
	internal const string TooManyEnumerations = "Sequence should not be enumerated more than expected.";
	internal const string TooManyDisposals = "Sequence should not be disposed more than once per enumeration.";
	internal const string SimultaneousEnumerations = "Sequence should not have simultaneous enumeration.";
	internal const string MoveNextPostDisposal = "LINQ operators should not call MoveNext() on a disposed sequence.";
	internal const string MoveNextPostEnumeration = "LINQ operators should not continue iterating a sequence that has terminated.";
	internal const string CurrentPostDisposal = "LINQ operators should not attempt to get the Current value on a disposed sequence.";
	internal const string CurrentPostEnumeration = "LINQ operators should not attempt to get the Current value on a completed sequence.";

	[Flags]
	public enum Options
	{
		None,
		AllowRepeatedDisposals = 0x2,
		AllowRepeatedMoveNexts = 0x4,
	}
}

public sealed class TestingSequenceException(string message) : Exception(message);

/// <summary>
/// Sequence that asserts whether its iterator has been disposed
/// when it is disposed itself and also whether GetAsyncEnumerator() is
/// called exactly once or not.
/// </summary>
internal sealed class TestingSequence<T> : IAsyncEnumerable<T>, IAsyncDisposable, IDisposable
{
	private readonly IAsyncEnumerable<T> _sequence;
	private readonly Options _options;
	private readonly int _maxEnumerations;

	private int _disposedCount;
	private int _enumerationCount;

	internal TestingSequence(IAsyncEnumerable<T> sequence, Options options, int maxEnumerations)
	{
		_sequence = sequence;
		_maxEnumerations = maxEnumerations;
		_options = options;
	}

	public int MoveNextCallCount { get; private set; }
	public bool IsDisposed => _enumerationCount > 0 && _disposedCount == _enumerationCount;

	void IDisposable.Dispose()
	{
		if (_enumerationCount > 0)
			AssertTestingSequence(_disposedCount == _enumerationCount, ExpectedDisposal);
	}

	ValueTask IAsyncDisposable.DisposeAsync()
	{
		if (_enumerationCount > 0)
			AssertTestingSequence(_disposedCount == _enumerationCount, ExpectedDisposal);

		return default;
	}

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
	{
		AssertTestingSequence(_enumerationCount == _disposedCount, SimultaneousEnumerations);
		AssertTestingSequence(_enumerationCount < _maxEnumerations, TooManyEnumerations);

		var enumerator = _sequence.GetAsyncEnumerator(cancellationToken).AsWatchable();
		_enumerationCount++;

		var disposed = false;
		enumerator.Disposed += delegate
		{
			if (!disposed)
			{
				_disposedCount++;
				disposed = true;
			}
			else if (!_options.HasFlag(Options.AllowRepeatedDisposals))
			{
				AssertTestingSequence(expectation: false, TooManyDisposals);
			}
		};

		var ended = false;
		enumerator.MoveNextCalled += (_, moved) =>
		{
			AssertTestingSequence(disposed is false, MoveNextPostDisposal);
			if (!_options.HasFlag(Options.AllowRepeatedMoveNexts))
				AssertTestingSequence(ended is false, MoveNextPostEnumeration);

			ended = !moved;
			MoveNextCallCount++;
		};

		enumerator.GetCurrentCalled += delegate
		{
			AssertTestingSequence(disposed is false, CurrentPostDisposal);
			AssertTestingSequence(ended is false, CurrentPostEnumeration);
		};

		return enumerator;
	}
}

public sealed class TestingSequenceTest
{
	[Fact]
	public async Task TestingSequencePublicPropertiesTest()
	{
		await using var sequence = Of(1, 2, 3, 4);
		Guard.IsFalse(sequence.IsDisposed);
		Guard.IsEqualTo(sequence.MoveNextCallCount, 0);

		var iter = sequence.GetAsyncEnumerator();
		Guard.IsFalse(sequence.IsDisposed);
		Guard.IsEqualTo(sequence.MoveNextCallCount, 0);

		for (var i = 1; i <= 4; i++)
		{
			_ = await iter.MoveNextAsync();
			Guard.IsFalse(sequence.IsDisposed);
			Guard.IsEqualTo(sequence.MoveNextCallCount, i);
		}

		await iter.DisposeAsync();
		Guard.IsTrue(sequence.IsDisposed);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateDisposal()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			var _ = enumerable.GetAsyncEnumerator();

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			await using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, ExpectedDisposal);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateNumberOfUsages()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using (enumerable.GetAsyncEnumerator())
				yield return 1;

			await using (enumerable.GetAsyncEnumerator())
				yield return 2;

			await using (enumerable.GetAsyncEnumerator())
				yield return 3;

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, TooManyEnumerations);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateDisposeOnDisposedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetAsyncEnumerator();
			await enumerator.DisposeAsync();
			await enumerator.DisposeAsync();

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, TooManyDisposals);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateMoveNextOnDisposedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetAsyncEnumerator();
			await enumerator.DisposeAsync();
			_ = await enumerator.MoveNextAsync();

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, MoveNextPostDisposal);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateMoveNextOnCompletedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enumerator = enumerable.GetAsyncEnumerator();
			while (await enumerator.MoveNextAsync())
				yield return enumerator.Current;

			_ = await enumerator.MoveNextAsync();

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, MoveNextPostEnumeration);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateCurrentOnDisposedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetAsyncEnumerator();
			await enumerator.DisposeAsync();
			yield return enumerator.Current;

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, CurrentPostDisposal);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateCurrentOnEndedSequence()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enumerator = enumerable.GetAsyncEnumerator();
			while (await enumerator.MoveNextAsync())
				yield return enumerator.Current;

			yield return enumerator.Current;

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, CurrentPostEnumeration);
	}

	[Fact]
	public async Task TestingSequenceShouldValidateSimultaneousEnumeration()
	{
		static async IAsyncEnumerable<int> InvalidUsage(IAsyncEnumerable<int> enumerable)
		{
			await using var enum1 = enumerable.GetAsyncEnumerator();
			await using var enum2 = enumerable.GetAsyncEnumerator();

			await Task.Yield();
			yield break;
		}

		static async Task Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
			await InvalidUsage(xs).Consume();
		}

		await AssertSequenceBehavior(Act, SimultaneousEnumerations);
	}

	private static async Task AssertSequenceBehavior(Func<Task> act, string message)
	{
		var ex = await Assert.ThrowsAsync<TestingSequenceException>(act);
		Assert.StartsWith(message, ex.Message, StringComparison.Ordinal);
	}
}
