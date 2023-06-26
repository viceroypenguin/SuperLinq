namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence of tuples where the `key` is 
	/// the zero-based index of the `value` in the source
	/// sequence.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <returns>A sequence of tuples.</returns>
	/// <remarks>This operator uses deferred execution and streams its
	/// results.</remarks>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(int index, TSource item)> Index<TSource>(this IEnumerable<TSource> source)
	{
		return source.Index(0);
	}

	/// <summary>
	/// Returns a sequence of tuples where the `key` is 
	/// the zero-based index of the `value` in the source
	/// sequence. An additional parameter specifies the 
	/// starting index.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="startIndex"></param>
	/// <returns>A sequence of tuples.</returns>
	/// <remarks>This operator uses deferred execution and streams its
	/// results.</remarks>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	public static IEnumerable<(int index, TSource item)> Index<TSource>(this IEnumerable<TSource> source, int startIndex)
	{
		Guard.IsNotNull(source);

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

	private sealed class IndexCollectionIterator<T> : CollectionIterator<(int index, T item)>
	{
		private readonly IEnumerable<T> _source;
		private readonly int _startIndex;

		public IndexCollectionIterator(IEnumerable<T> source, int startIndex)
		{
			_source = source;
			_startIndex = startIndex;
		}

		public override int Count => _source.GetCollectionCount();

		protected override IEnumerable<(int index, T item)> GetEnumerable()
		{
			var index = _startIndex;
			foreach (var item in _source)
				yield return (index++, item);
		}
	}

	private sealed class IndexListIterator<T> : ListIterator<(int index, T item)>
	{
		private readonly IList<T> _source;
		private readonly int _startIndex;

		public IndexListIterator(IList<T> source, int startIndex)
		{
			_source = source;
			_startIndex = startIndex;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<(int index, T item)> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return (_startIndex + i, _source[i]);
			}
		}

		protected override (int index, T item) ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);
			return (_startIndex + index, _source[index]);
		}
	}
}
