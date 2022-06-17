using System.Collections;

namespace Test;

public class CountDownTest
{
	[Fact]
	public void IsLazy()
	{
		new BreakingSequence<object>()
			.CountDown(42, BreakingFunc.Of<object, int?, object>());
	}

	[Fact]
	public void WithNegativeCount()
	{
		const int count = 10;
		Enumerable.Range(1, count)
				  .CountDown(-1000, (_, cd) => cd)
				  .AssertSequenceEqual(Enumerable.Repeat((int?)null, count));
	}

	static IEnumerable<T> GetData<T>(Func<int[], int, int?[], T> selector)
	{
		var xs = Enumerable.Range(0, 5).ToArray();
		yield return selector(xs, -1, new int?[] { null, null, null, null, null });
		yield return selector(xs, 0, new int?[] { null, null, null, null, null });
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

	public static IEnumerable<object[]> ListData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		from kind in new[] { SourceKind.BreakingList, SourceKind.BreakingReadOnlyList }
		select new object[] { e.Source.ToSourceKind(kind), e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create), };

	[Theory, MemberData(nameof(ListData))]
	public void WithList(IEnumerable<int> xs, int count, IEnumerable<(int, int?)> expected)
	{
		Assert.Equal(expected, xs.CountDown(count, ValueTuple.Create));
	}

	public static IEnumerable<object[]> CollectionData { get; } =
		from e in GetData((xs, count, countdown) => new
		{
			Source = xs,
			Count = count,
			Countdown = countdown,
		})
		from isReadOnly in new[] { true, false }
		select new object[] { e.Source, isReadOnly, e.Count, e.Source.Zip(e.Countdown, ValueTuple.Create), };

	[Theory, MemberData(nameof(CollectionData))]
	public void WithCollection(int[] xs, bool isReadOnly, int count, IEnumerable<(int, int?)> expected)
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

		var ts = isReadOnly
			   ? TestCollection.CreateReadOnly(xs, Watch)
			   : TestCollection.Create(xs, Watch).AsEnumerable();

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

	static class TestCollection
	{
		public static ICollection<T>
			Create<T>(ICollection<T> collection,
						 Func<IEnumerator<T>, IEnumerator<T>> em = null)
		{
			return new Collection<T>(collection, em);
		}

		public static IReadOnlyCollection<T>
			CreateReadOnly<T>(ICollection<T> collection,
						Func<IEnumerator<T>, IEnumerator<T>> em = null)
		{
			return new ReadOnlyCollection<T>(collection, em);
		}

		/// <summary>
		/// A sequence that permits its enumerator to be substituted
		/// for another.
		/// </summary>

		abstract class Sequence<T> : IEnumerable<T>
		{
			readonly Func<IEnumerator<T>, IEnumerator<T>> _em;

			protected Sequence(Func<IEnumerator<T>, IEnumerator<T>> em) =>
				_em = em ?? (e => e);

			public IEnumerator<T> GetEnumerator() =>
				_em(Items.GetEnumerator());

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			protected abstract IEnumerable<T> Items { get; }
		}

		/// <summary>
		/// A collection that wraps another but which also permits its
		/// enumerator to be substituted for another.
		/// </summary>

		sealed class Collection<T> : Sequence<T>, ICollection<T>
		{
			readonly ICollection<T> _collection;

			public Collection(ICollection<T> collection,
							  Func<IEnumerator<T>, IEnumerator<T>> em = null) :
				base(em) =>
				_collection = collection ?? throw new ArgumentNullException(nameof(collection));

			public int Count => _collection.Count;
			public bool IsReadOnly => _collection.IsReadOnly;

			protected override IEnumerable<T> Items => _collection;

			public bool Contains(T item) => _collection.Contains(item);
			public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

			public void Add(T item) => throw new NotImplementedException();
			public void Clear() => throw new NotImplementedException();
			public bool Remove(T item) => throw new NotImplementedException();
		}

		/// <summary>
		/// A read-only collection that wraps another collection but which
		/// also permits its enumerator to be substituted for another.
		/// </summary>

		sealed class ReadOnlyCollection<T> : Sequence<T>, IReadOnlyCollection<T>
		{
			readonly ICollection<T> _collection;

			public ReadOnlyCollection(ICollection<T> collection,
									  Func<IEnumerator<T>, IEnumerator<T>> em = null) :
				base(em) =>
				_collection = collection ?? throw new ArgumentNullException(nameof(collection));

			public int Count => _collection.Count;

			protected override IEnumerable<T> Items => _collection;
		}
	}
}
