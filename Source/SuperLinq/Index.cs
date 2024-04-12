namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence of tuples where the `index` is the zero-based index of the `item` in the <paramref
	///     name="source"/> sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in <paramref name="source"/> sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <returns>
	///	    A sequence of tuples.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
#if NET9_0_OR_GREATER
	[Obsolete("This method has been implemented by the framework.")]
	public static IEnumerable<(int Index, TSource Item)> Index<TSource>(IEnumerable<TSource> source)
#else
	public static IEnumerable<(int Index, TSource Item)> Index<TSource>(this IEnumerable<TSource> source)
#endif
	{
		return source.Index(0);
	}

	/// <summary>
	///	    Returns a sequence of tuples where the `index` is the <paramref name="startIndex"/>-based index of the
	///     `item` in the <paramref name="source"/> sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in <paramref name="source"/> sequence.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="startIndex">
	///	    The index of the first value returned.
	/// </param>
	/// <returns>
	///	    A sequence of tuples.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<(int Index, TSource Item)> Index<TSource>(this IEnumerable<TSource> source, int startIndex)
	{
		ArgumentNullException.ThrowIfNull(source);

		if (source is IList<TSource> list)
			return new IndexListIterator<TSource>(list, startIndex);

		if (source.TryGetCollectionCount() is int)
			return new IndexCollectionIterator<TSource>(source, startIndex);

		return Core(source, startIndex);

		static IEnumerable<(int index, TSource item)> Core(IEnumerable<TSource> source, int startIndex)
		{
			foreach (var item in source)
				yield return (startIndex++, item);
		}
	}

	private sealed class IndexCollectionIterator<T>(
		IEnumerable<T> source,
		int startIndex
	) : CollectionIterator<(int index, T item)>
	{
		public override int Count => source.GetCollectionCount();

		protected override IEnumerable<(int index, T item)> GetEnumerable()
		{
			var index = startIndex;
			foreach (var item in source)
				yield return (index++, item);
		}
	}

	private sealed class IndexListIterator<T>(
		IList<T> source,
		int startIndex
	) : ListIterator<(int index, T item)>
	{
		public override int Count => source.Count;

		protected override IEnumerable<(int index, T item)> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
				yield return (startIndex + i, source[i]);
		}

		protected override (int index, T item) ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return (startIndex + index, source[index]);
		}
	}
}
