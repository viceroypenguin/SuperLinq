using System.Collections;
using CommunityToolkit.Diagnostics;
using Xunit.Sdk;

namespace Test;

internal static class TestingSequence
{
	internal static TestingSequence<T> Of<T>(params T[] elements) => new(elements, 1);

	internal static TestingSequence<T> AsTestingSequence<T>(this IEnumerable<T> source, int numEnumerations = 1) =>
		source != null
		? new TestingSequence<T>(source, numEnumerations)
		: ThrowHelper.ThrowArgumentNullException<TestingSequence<T>>(nameof(source));

	internal const string ExpectedDisposal = "Expected sequence to be disposed.";
	internal const string TooManyEnumerations = "Sequence should not be enumerated more than expected.";
	internal const string SimultaneousEnumerations = "Sequence should not have simultaneous enumeration.";
	internal const string MoveNextDisposed = "LINQ operators should not call MoveNext() on a disposed sequence.";
	internal const string CurrentDisposed = "LINQ operators should not attempt to get the Current value on a disposed sequence.";
	internal const string CurrentCompleted = "LINQ operators should not attempt to get the Current value on a completed sequence.";
}

internal sealed class TestingSequence<T> : IDisposableEnumerable<T>
{
	private IEnumerable<T>? _sequence;
	private readonly int _numEnumerations;

	private bool _hasEnumerated;
	private bool _currentlyEnumerating;
	private int _disposedCount;
	private int _enumerationCount;

	internal TestingSequence(IEnumerable<T> sequence, int numEnumerations)
	{
		_sequence = sequence;
		_numEnumerations = numEnumerations;
	}

	public int MoveNextCallCount { get; private set; }

	void IDisposable.Dispose()
	{
		if (_hasEnumerated)
			Assert.True(_disposedCount == _enumerationCount, TestingSequence.ExpectedDisposal);
	}

	public IEnumerator<T> GetEnumerator()
	{
		Assert.False(_sequence is null, TestingSequence.TooManyEnumerations);
		Assert.False(_currentlyEnumerating, TestingSequence.SimultaneousEnumerations);

		_hasEnumerated = true;

		var enumerator = _sequence.GetEnumerator().AsWatchable();
		_currentlyEnumerating = true;

		var disposed = false;
		enumerator.Disposed += delegate
		{
			if (!disposed)
			{
				_disposedCount++;
				_currentlyEnumerating = false;
				disposed = true;
			}
		};

		var ended = false;
		enumerator.MoveNextCalled += (_, moved) =>
		{
			Assert.True(_currentlyEnumerating, TestingSequence.MoveNextDisposed);
			ended = !moved;
			MoveNextCallCount++;
		};
		enumerator.GetCurrentCalled += delegate
		{
			Assert.True(_currentlyEnumerating, TestingSequence.CurrentDisposed);
			Assert.False(ended, TestingSequence.CurrentCompleted);
		};

		if (++_enumerationCount == _numEnumerations)
			_sequence = null;
		return enumerator;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TestingSequenceTest
{
	[Fact]
	public void TestingSequenceShouldValidateDisposal()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();

			yield break;
		}

		var ex = Assert.Throws<TrueException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.ExpectedDisposal, ex.Message);
	}

	[Fact]
	public void TestingSequenceShouldValidateNumberOfUsages()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using (var enumerator = enumerable.GetEnumerator())
				yield return 1;
			using (var enumerator = enumerable.GetEnumerator())
				yield return 2;
			using (var enumerator = enumerable.GetEnumerator())
				yield return 3;

			yield break;
		}

		var ex = Assert.Throws<FalseException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(2);
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.TooManyEnumerations, ex.Message);
	}

	[Fact]
	public void TestingSequenceShouldValidateMoveNextOnDisposedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using var enumerator = enumerable.GetEnumerator();
			enumerator.Dispose();
			enumerator.MoveNext();

			yield break;
		}

		var ex = Assert.Throws<TrueException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.MoveNextDisposed, ex.Message);
	}

	[Fact]
	public void TestingSequenceShouldValidateCurrentOnDisposedSequence()
	{
		static IEnumerable<int> InvalidUsage(IEnumerable<int> enumerable)
		{
			using var enumerator = enumerable.GetEnumerator();
			enumerator.Dispose();
			yield return enumerator.Current;

			yield break;
		}

		var ex = Assert.Throws<TrueException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.CurrentDisposed, ex.Message);
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

			yield break;
		}

		var ex = Assert.Throws<FalseException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence();
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.CurrentCompleted, ex.Message);
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

		var ex = Assert.Throws<FalseException>(() =>
		{
			using var xs = Enumerable.Range(1, 10).AsTestingSequence(2);
			InvalidUsage(xs).Consume();
		});
		Assert.StartsWith(TestingSequence.SimultaneousEnumerations, ex.Message);
	}
}
