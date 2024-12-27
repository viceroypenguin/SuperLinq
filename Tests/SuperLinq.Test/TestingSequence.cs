using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static Test.TestingSequence;

namespace Test;

internal static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) =>
		new(elements, Options.None, maxEnumerations: 1);

	internal static TestingSequence<T> AsTestingSequence<T>(
		this IEnumerable<T> source,
		Options options = Options.None,
		int maxEnumerations = 1
	) => source is not null ? new TestingSequence<T>(source, options, maxEnumerations)
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
/// when it is disposed itself and also whether GetEnumerator() is
/// called exactly once or not.
/// </summary>
internal class TestingSequence<T> : IDisposableEnumerable<T>
{
	private readonly IEnumerable<T> _sequence;
	private readonly Options _options;
	private readonly int _maxEnumerations;

	private int _disposedCount;
	private int _enumerationCount;

	internal TestingSequence(IEnumerable<T> sequence, Options options, int maxEnumerations)
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

	[SuppressMessage(
		"Style",
		"IDE0100",
		Justification = "Expanded code is used for better error reporting on test failures"
	)]
	public IEnumerator<T> GetEnumerator()
	{
		AssertTestingSequence(_enumerationCount == _disposedCount, SimultaneousEnumerations);
		AssertTestingSequence(_enumerationCount < _maxEnumerations, TooManyEnumerations);

		var enumerator = _sequence.GetEnumerator().AsWatchable();
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

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class TestingSequenceTest
{
	[Fact]
	public void TestingSequencePublicPropertiesTest()
	{
		using var sequence = Of(1, 2, 3, 4);
		Assert.False(sequence.IsDisposed);
		Assert.Equal(0, sequence.MoveNextCallCount);

		var iter = sequence.GetEnumerator();
		Assert.False(sequence.IsDisposed);
		Assert.Equal(0, sequence.MoveNextCallCount);

		for (var i = 1; i <= 4; i++)
		{
			_ = iter.MoveNext();
			Assert.False(sequence.IsDisposed);
			Assert.Equal(i, sequence.MoveNextCallCount);
		}

		iter.Dispose();
		Assert.True(sequence.IsDisposed);
	}

	[Fact]
	public void TestingSequenceShouldValidateDisposal()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			var _ = enumerable.GetEnumerator();

			yield break;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, ExpectedDisposal);
	}

	[Fact]
	public void TestingSequenceShouldValidateNumberOfUsages()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using (enumerable.GetEnumerator())
				yield return 1;

			using (enumerable.GetEnumerator())
				yield return 2;

			using (enumerable.GetEnumerator())
				yield return 3;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, TooManyEnumerations);
	}

	[Fact]
	public void TestingSequenceShouldValidateDisposeOnDisposedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			enumerator.Dispose();
			enumerator.Dispose();

			yield break;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, TooManyDisposals);
	}

	[Fact]
	public void TestingSequenceShouldValidateMoveNextOnDisposedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			enumerator.Dispose();
			_ = enumerator.MoveNext();

			yield break;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, MoveNextPostDisposal);
	}

	[Fact]
	public void TestingSequenceShouldValidateMoveNextOnCompletedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
				yield return enumerator.Current;

			_ = enumerator.MoveNext();
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, MoveNextPostEnumeration);
	}

	[Fact]
	public void TestingSequenceShouldValidateCurrentOnDisposedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			enumerator.Dispose();
			yield return enumerator.Current;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, CurrentPostDisposal);
	}

	[Fact]
	public void TestingSequenceShouldValidateCurrentOnEndedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
				yield return enumerator.Current;

			yield return enumerator.Current;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, CurrentPostEnumeration);
	}

	[Fact]
	public void TestingSequenceShouldValidateSimultaneousEnumeration()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using var enum1 = enumerable.GetEnumerator();
			using var enum2 = enumerable.GetEnumerator();

			yield break;
		}

		static void Act()
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(maxEnumerations: 2);
			InvalidUsage(xs).Consume();
		}

		AssertSequenceBehavior(Act, SimultaneousEnumerations);
	}

	private static void AssertSequenceBehavior(Action act, string message)
	{
		var ex = Assert.Throws<TestingSequenceException>(act);
		Assert.StartsWith(message, ex.Message, StringComparison.Ordinal);
	}
}
