using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource}"/> except that <see cref="Scan{TSource}"/> returns the sequence of
	/// intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	public static IEnumerable<TSource> Scan<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		if (source is ICollection<TSource> coll)
			return new ScanIterator<TSource>(coll, transformation);

		return ScanCore(source, transformation);
	}

	private static IEnumerable<TSource> ScanCore<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> transformation)
	{
		using var e = source.GetEnumerator();

		if (!e.MoveNext())
			yield break;

		var state = e.Current;
		yield return state;

		while (e.MoveNext())
		{
			state = transformation(state, e.Current);
			yield return state;
		}
	}

	private sealed class ScanIterator<T> : CollectionIterator<T>
	{
		private readonly ICollection<T> _source;
		private readonly Func<T, T, T> _transformation;

		public ScanIterator(ICollection<T> source, Func<T, T, T> transformation)
		{
			_source = source;
			_transformation = transformation;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			ScanCore(_source, _transformation);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			var (sList, b, cnt) = _source is IList<T> s
				? (s, 0, s.Count)
				: (array, arrayIndex, SuperEnumerable.CopyTo(_source, array, arrayIndex));

			var i = 0;
			var state = sList[b + i];
			array[arrayIndex + i] = state;

			for (i++; i < cnt; i++)
			{
				state = _transformation(state, sList[b + i]);
				array[arrayIndex + i] = state;
			}
		}
	}

	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource}"/> except that <see cref="Scan{TSource}"/> returns the sequence of
	/// intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	[Obsolete("Method renamed back to `Scan`.")]
	public static IEnumerable<TSource> ScanEx<TSource>(
		this IEnumerable<TSource> source,
		Func<TSource, TSource, TSource> transformation)
	{
		return Scan(source, transformation);
	}

	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource, TState}"/> except that <see cref="Scan{TSource,TState}"/> returns the
	/// sequence of intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <typeparam name="TState">Type of state</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="seed">Initial state to seed</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	public static IEnumerable<TState> Scan<TSource, TState>(
		this IEnumerable<TSource> source,
		TState seed,
		Func<TState, TSource, TState> transformation)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(transformation);

		if (source is ICollection<TSource> coll)
			return new ScanStateIterator<TSource, TState>(coll, seed, transformation);

		return ScanCore(source, seed, transformation);
	}

	private static IEnumerable<TState> ScanCore<TSource, TState>(
		IEnumerable<TSource> source,
		TState state,
		Func<TState, TSource, TState> transformation)
	{
		yield return state;

		foreach (var e in source)
		{
			state = transformation(state, e);
			yield return state;
		}
	}

	private sealed class ScanStateIterator<TSource, TState> : CollectionIterator<TState>
	{
		private readonly ICollection<TSource> _source;
		private readonly TState _state;
		private readonly Func<TState, TSource, TState> _transformation;

		public ScanStateIterator(
			ICollection<TSource> source,
			TState state,
			Func<TState, TSource, TState> transformation)
		{
			_source = source;
			_state = state;
			_transformation = transformation;
		}

		public override int Count => _source.Count + 1;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<TState> GetEnumerable() =>
			ScanCore(_source, _state, _transformation);

		public override void CopyTo(TState[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			var list = _source is IList<TSource> l ? l : _source.ToList();

			var state = _state;
			array[arrayIndex] = state;

			for (var i = 0; i < list.Count; i++)
			{
				state = _transformation(state, list[i]);
				array[arrayIndex + i + 1] = state;
			}
		}
	}

	/// <summary>
	/// Performs a scan (inclusive prefix sum) on a sequence of elements. This operator is similar to <see
	/// cref="Enumerable.Aggregate{TSource, TState}"/> except that <see cref="Scan{TSource,TState}"/> returns the
	/// sequence of intermediate results as well as the final one.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in source sequence</typeparam>
	/// <typeparam name="TState">Type of state</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="seed">Initial state to seed</param>
	/// <param name="transformation">Transformation operation</param>
	/// <returns>The scanned sequence</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="transformation"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its result.
	/// </remarks>
	[Obsolete("Method renamed back to `Scan`.")]
	public static IEnumerable<TState> ScanEx<TSource, TState>(
	this IEnumerable<TSource> source,
	TState seed,
	Func<TState, TSource, TState> transformation)
	{
		return Scan(source, seed, transformation);
	}
}
