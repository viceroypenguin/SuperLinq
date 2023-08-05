namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Asserts that a source sequence contains a given count of elements.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in <paramref name="source"/> sequence.
	///	</typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="count">
	///	    Count to assert.
	///	</param>
	/// <returns>
	///	    Returns the original sequence as long it is contains the number of elements specified by <paramref
	///     name="count"/>. Otherwise it throws <see cref="ArgumentException" />.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>0</c>.
	///	</exception>
	/// <exception cref="ArgumentException">
	///	    Thrown lazily <paramref name="source"/> has a length different than <paramref name="count"/>.
	///	</exception>
	/// <remarks>
	///	    The sequence length is evaluated lazily during the enumeration of the sequence.
	/// </remarks>
	public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source, int count)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		if (source is IList<TSource> list)
			return new AssertCountListIterator<TSource>(list, count);

		if (source.TryGetCollectionCount() is int)
			return new AssertCountCollectionIterator<TSource>(source, count);

		return Core(source, count);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, int count)
		{
			var c = 0;
			foreach (var item in source)
			{
				if (++c > count)
					break;
				yield return item;
			}
			Guard.IsEqualTo(c, count, $"{nameof(source)}.Count()");
		}
	}

	private sealed class AssertCountCollectionIterator<T> : CollectionIterator<T>
	{
		private readonly IEnumerable<T> _source;
		private readonly int _count;

		public AssertCountCollectionIterator(IEnumerable<T> source, int count)
		{
			_source = source;
			_count = count;
		}

		public override int Count
		{
			get
			{
				Guard.IsEqualTo(_source.GetCollectionCount(), _count, "source.Count()");
				return _count;
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			Guard.IsEqualTo(_source.GetCollectionCount(), _count, "source.Count()");

			foreach (var item in _source)
				yield return item;
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_ = _source.CopyTo(array, arrayIndex);
		}
	}

	private sealed class AssertCountListIterator<T> : ListIterator<T>
	{
		private readonly IList<T> _source;
		private readonly int _count;

		public AssertCountListIterator(IList<T> source, int count)
		{
			_source = source;
			_count = count;
		}

		public override int Count
		{
			get
			{
				Guard.IsEqualTo(_source.Count, _count, "source.Count()");
				return _count;
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return _source[i];
			}
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_source.CopyTo(array, arrayIndex);
		}

		protected override T ElementAt(int index)
		{
			Guard.IsBetweenOrEqualTo(index, 0, Count - 1);
			return _source[index];
		}
	}
}
