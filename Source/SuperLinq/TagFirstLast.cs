namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence of tuples containing the element and flags indicating whether the element is the first
	///     and/or last of the sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <returns>
	///	    Returns the resulting sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TSource item, bool isFirst, bool isLast)> TagFirstLast<TSource>(this IEnumerable<TSource> source)
	{
		return source.TagFirstLast(ValueTuple.Create);
	}

	/// <summary>
	///	    Returns a sequence resulting from applying a function to each element in the source sequence with additional
	///     parameters indicating whether the element is the first and/or last of the sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the element of the returned sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="resultSelector">
	///	    A function that determines how to project the each element along with its first or last tag.
	/// </param>
	/// <returns>
	///	    Returns the resulting sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="resultSelector"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> TagFirstLast<TSource, TResult>(
		this IEnumerable<TSource> source,
		Func<TSource, bool, bool, TResult> resultSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		if (source is IList<TSource> list)
			return new TagFirstLastIterator<TSource, TResult>(list, resultSelector);

		return Core(source, resultSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, Func<TSource, bool, bool, TResult> resultSelector)
		{
			using var iter = source.GetEnumerator();

			if (!iter.MoveNext())
				yield break;

			var cur = iter.Current;
			var first = true;

			while (iter.MoveNext())
			{
				yield return resultSelector(cur, first, false);
				cur = iter.Current;
				first = false;
			}

			yield return resultSelector(cur, first, true);
		}
	}

	private sealed class TagFirstLastIterator<TSource, TResult>(
		IList<TSource> source,
		Func<TSource, bool, bool, TResult> resultSelector
	) : ListIterator<TResult>
	{
		public override int Count => source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			if (source.Count <= 1)
			{
				if (source.Count == 1)
					yield return resultSelector(source[0], true, true);

				yield break;
			}

			yield return resultSelector(source[0], true, false);

			var cnt = (uint)source.Count - 1;

			for (var i = 1; i < cnt; i++)
				yield return resultSelector(source[i], false, false);

			yield return resultSelector(source[^1], false, true);
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return resultSelector(source[index], index == 0, index == source.Count - 1);
		}
	}
}
