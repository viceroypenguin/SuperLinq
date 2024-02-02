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
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(count);

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

			ArgumentOutOfRangeException.ThrowIfNotEqual(c, count, $"{nameof(source)}.Count()");
		}
	}

	private sealed class AssertCountCollectionIterator<T>(
		IEnumerable<T> source,
		int count
	) : CollectionIterator<T>
	{
		public override int Count
		{
			get
			{
				ArgumentOutOfRangeException.ThrowIfNotEqual(source.GetCollectionCount(), count, "source.Count()");
				return count;
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			ArgumentOutOfRangeException.ThrowIfNotEqual(source.GetCollectionCount(), count, "source.Count()");

			foreach (var item in source)
				yield return item;
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			_ = source.CopyTo(array, arrayIndex);
		}
	}

	private sealed class AssertCountListIterator<T>(
		IList<T> source,
		int count
	) : ListIterator<T>
	{
		public override int Count
		{
			get
			{
				ArgumentOutOfRangeException.ThrowIfNotEqual(source.Count, count, "source.Count()");
				return count;
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			var cnt = (uint)Count;
			for (var i = 0; i < cnt; i++)
			{
				yield return source[i];
			}
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			source.CopyTo(array, arrayIndex);
		}

		protected override T ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return source[index];
		}
	}
}
