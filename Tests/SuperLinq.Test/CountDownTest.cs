using System.Collections;
using CommunityToolkit.Diagnostics;

namespace Test;

public class CountDownTest
{
	[Fact]
	public void IsLazy()
	{
		new BreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Theory]
	[InlineData(0), InlineData(-1)]
	public void ExceptionOnNegativeCount(int param)
	{
		Assert.Throws<ArgumentOutOfRangeException>("count", () =>
			Seq(1).CountDown(param));
	}

	public static IEnumerable<T> GetData<T>(Func<int[], int, int?[], T> selector)
	{
		var xs = Enumerable.Range(0, 5).ToArray();
		yield return selector(xs, 1, new int?[] { null, null, null, null, 0 });
		yield return selector(xs, 2, new int?[] { null, null, null, 1, 0 });
		yield return selector(xs, 3, new int?[] { null, null, 2, 1, 0 });
		yield return selector(xs, 4, new int?[] { null, 3, 2, 1, 0 });
		yield return selector(xs, 5, new int?[] { 4, 3, 2, 1, 0 });
		yield return selector(xs, 6, new int?[] { 4, 3, 2, 1, 0 });
		yield return selector(xs, 7, new int?[] { 4, 3, 2, 1, 0 });
	}

	public static IEnumerable<object[]> SequenceData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		select new object[] { e.Source, e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create), };

	[Theory, MemberData(nameof(SequenceData))]
	public void WithSequence(int[] xs, int count, IEnumerable<(int, int?)> expected)
	{
		using var ts = xs.Select(x => x).AsTestingSequence();
		Assert.Equal(expected, ts.CountDown(count, ValueTuple.Create));
	}

	public static IEnumerable<object[]> CollectionData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		select new object[] { e.Source, e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create), };

	[Theory, MemberData(nameof(CollectionData))]
	public void WithCollection(int[] xs, int count, IEnumerable<(int, int?)> expected)
	{
		var moves = 0;
		var disposed = false;

		IEnumerator<T> Watch<T>(IEnumerator<T> e)
		{
			moves = 0;
			disposed = false;
			var te = e.AsWatchable();
			te.Disposed += delegate { disposed = true; };
			te.MoveNextCalled += delegate { moves++; };
			return te;
		}

		var ts = TestCollection.Create(xs, Watch).AsEnumerable();

		var result =
			ts.CountDown(count, ValueTuple.Create).Index(1)
				.Do(e =>
				{
					// For a collection, CountDown doesn't do any buffering
					// so check that as each result becomes available, the
					// source hasn't been "pulled" on more.
					Assert.Equal(e.index, moves);
				})
				.Select(x => x.item);

		Assert.Equal(expected, result);
		Assert.True(disposed);
	}

	private static class TestCollection
	{
		public static ICollection<T> Create<T>(
			ICollection<T> collection,
			Func<IEnumerator<T>, IEnumerator<T>>? em = null)
		{
			return new Collection<T>(collection, em);
		}

		/// <summary>
		/// A collection that wraps another but which also permits its
		/// enumerator to be substituted for another.
		/// </summary>
		private sealed class Collection<T> : ICollection<T>
		{
			private readonly Func<IEnumerator<T>, IEnumerator<T>> _em;
			private readonly ICollection<T> _collection;

			public Collection(
				ICollection<T> collection,
				Func<IEnumerator<T>, IEnumerator<T>>? em = null)
			{
				_collection = collection ?? ThrowHelper.ThrowArgumentNullException<ICollection<T>>(nameof(collection));
				_em = em ?? SuperEnumerable.Identity;
			}

			public int Count => _collection.Count;
			public bool IsReadOnly => _collection.IsReadOnly;

			public IEnumerator<T> GetEnumerator() =>
				_em(_collection.GetEnumerator());

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			public bool Contains(T item) => _collection.Contains(item);
			public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

			public void Add(T item) => throw new NotImplementedException();
			public void Clear() => throw new NotImplementedException();
			public bool Remove(T item) => throw new NotImplementedException();
		}
	}
}
