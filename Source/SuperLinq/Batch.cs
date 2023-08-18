namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="source"/>.
	///	</typeparam>
	/// <param name="source">
	///	    An <see cref="IEnumerable{T}"/> whose elements to chunk.
	///	</param>
	/// <param name="size">
	///	    The maximum size of each chunk.
	///	</param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size
	///     size.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A chunk can contain fewer elements than <paramref name="size"/>, specifically the final buffer of <paramref
	///     name="source"/>.
	/// </para>
	/// <para>
	///	    A separate array is allocated for each returned chunk. Other overloads of <c>Batch</c> are available which
	///	    do not require additional array allocations for each chunk.
	///	</para>
	/// <para>
	///	    Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
	{
		// yes this operator duplicates on net6+; but no name overlap, so leave alone
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		if (source is IList<TSource> list)
			return new BatchIterator<TSource>(list, size);

		return Core(source, size);

		static IEnumerable<IList<TSource>> Core(IEnumerable<TSource> source, int size)
		{
			TSource[]? array = null;

			if (source is ICollection<TSource> coll)
			{
				if (coll.Count == 0)
					yield break;

				if (coll.Count <= size)
				{
					array = new TSource[coll.Count];
					coll.CopyTo(array, 0);
					yield return array;
					yield break;
				}
			}
			else if (source.TryGetCollectionCount() == 0)
			{
				yield break;
			}

			var n = 0;
			foreach (var item in source)
			{
				(array ??= new TSource[size])[n++] = item;
				if (n != size)
					continue;

				yield return array;
				array = null;
				n = 0;
			}

			if (n != 0)
			{
				Array.Resize(ref array, n);
				yield return array;
			}
		}
	}

	private sealed class BatchIterator<T> : ListIterator<IList<T>>
	{
		private readonly IList<T> _source;
		private readonly int _size;

		public BatchIterator(IList<T> source, int size)
		{
			_source = source;
			_size = size;
		}

		public override int Count => ((_source.Count - 1) / _size) + 1;

		protected override IEnumerable<IList<T>> GetEnumerable()
		{
			var sourceIndex = 0;
			var count = (uint)_source.Count;
			while (sourceIndex + _size - 1 < count)
			{
				var window = new T[_size];
				for (var i = 0; i < _size && sourceIndex < count; sourceIndex++, i++)
					window[i] = _source[sourceIndex];

				yield return window;
			}

			if (sourceIndex < count)
			{
				var window = new T[count - sourceIndex];
				for (var j = 0; sourceIndex < count; sourceIndex++, j++)
					window[j] = _source[sourceIndex];

				yield return window;
			}
		}

		protected override IList<T> ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);

			var start = index * _size;
			var max = (uint)Math.Min(_source.Count, start + _size);
			var arr = new T[Math.Min(_size, max - start)];
			for (int i = 0, j = start; i < _size && j < max; i++, j++)
				arr[i] = _source[j];

			return arr;
		}
	}
}
