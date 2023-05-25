namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Inserts the elements of a sequence into another sequence at a
	/// specified index.
	/// </summary>
	/// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
	/// <param name="first">The source sequence.</param>
	/// <param name="second">The sequence that will be inserted.</param>
	/// <param name="index">
	/// The zero-based index at which to insert elements from
	/// <paramref name="second"/>.</param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="first"/>
	/// plus the elements of <paramref name="second"/> inserted at
	/// the given index.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="index"/> is negative.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown lazily if <paramref name="index"/> is greater than the
	/// length of <paramref name="first"/>. The validation occurs when
	/// yielding the next element after having iterated
	/// <paramref name="first"/> entirely.
	/// </exception>
	public static IEnumerable<T> Insert<T>(this IEnumerable<T> first, IEnumerable<T> second, int index)
	{
		Guard.IsGreaterThanOrEqualTo(index, 0);

		return Insert(first, second, (Index)index);
	}

	/// <summary>
	/// Inserts the elements of a sequence into another sequence at a
	/// specified index.
	/// </summary>
	/// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
	/// <param name="first">The source sequence.</param>
	/// <param name="second">The sequence that will be inserted.</param>
	/// <param name="index">
	/// The zero-based index at which to insert elements from
	/// <paramref name="second"/>.</param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="first"/>
	/// plus the elements of <paramref name="second"/> inserted at
	/// the given index.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="index"/> is negative.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown lazily if <paramref name="index"/> is greater than the
	/// length of <paramref name="first"/>. The validation occurs when
	/// yielding the next element after having iterated
	/// <paramref name="first"/> entirely.
	/// </exception>
	public static IEnumerable<T> Insert<T>(this IEnumerable<T> first, IEnumerable<T> second, Index index)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		if (first is IList<T> fList && second is IList<T> sList)
			return new InsertListIterator<T>(fList, sList, index);

		if (first.TryGetCollectionCount() is int && second.TryGetCollectionCount() is int)
			return new InsertCollectionIterator<T>(first, second, index);

		return !index.IsFromEnd ? InsertCore(first, second, index.Value) :
			index.Value == 0 ? first.Concat(second) :
			FromEndCore(first, second, index.Value);

		static IEnumerable<T> FromEndCore(IEnumerable<T> first, IEnumerable<T> second, int index)
		{
			using var e = first.CountDown(index, ValueTuple.Create).GetEnumerator();

			if (e.MoveNext())
			{
				var (_, countdown) = e.Current;
				if (countdown is { } n && n != index - 1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(
						nameof(index),
						"Insertion index is greater than the length of the first sequence.");
				}

				do
				{
					T a;
					(a, countdown) = e.Current;
					if (countdown == index - 1)
					{
						foreach (var b in second)
							yield return b;
					}

					yield return a;
				}
				while (e.MoveNext());
			}
		}
	}

	private static IEnumerable<T> InsertCore<T>(IEnumerable<T> first, IEnumerable<T> second, int index)
	{
		var i = -1;

		using var iter = first.GetEnumerator();

		while (++i < index && iter.MoveNext())
			yield return iter.Current;

		if (i < index)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(
				nameof(index),
				"Insertion index is greater than the length of the first sequence.");
		}

		foreach (var item in second)
			yield return item;

		while (iter.MoveNext())
			yield return iter.Current;
	}

	private class InsertCollectionIterator<T> : CollectionIterator<T>
	{
		private readonly IEnumerable<T> _first;
		private readonly IEnumerable<T> _second;
		private readonly Index _index;

		public InsertCollectionIterator(IEnumerable<T> first, IEnumerable<T> second, Index index)
		{
			_first = first;
			_second = second;
			_index = index;
		}

		public override int Count
		{
			get
			{
				var fCount = _first.GetCollectionCount();
				var idx = _index.GetOffset(fCount);
				Guard.IsBetweenOrEqualTo(idx, 0, fCount);
				return fCount + _second.GetCollectionCount();
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			var fCount = _first.GetCollectionCount();
			var idx = _index.GetOffset(fCount);
			Guard.IsBetweenOrEqualTo(idx, 0, fCount);

			return InsertCore(_first, _second, idx);
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_ = _first.CopyTo(array, arrayIndex);

			var span = array.AsSpan()[arrayIndex..];
			var cnt = _first.GetCollectionCount();
			var idx = _index.GetOffset(cnt);
			span[idx..cnt].CopyTo(span[(idx + _second.GetCollectionCount())..]);

			_ = _second.CopyTo(array, arrayIndex + idx);
		}
	}

	private class InsertListIterator<T> : ListIterator<T>
	{
		private readonly IList<T> _first;
		private readonly IList<T> _second;
		private readonly Index _index;

		public InsertListIterator(IList<T> first, IList<T> second, Index index)
		{
			_first = first;
			_second = second;
			_index = index;
		}

		public override int Count
		{
			get
			{
				var idx = _index.GetOffset(_first.Count);
				Guard.IsBetweenOrEqualTo(idx, 0, _first.Count);
				return _first.Count + _second.Count;
			}
		}

		protected override IEnumerable<T> GetEnumerable()
		{
			var idx = _index.GetOffset(_first.Count);
			Guard.IsBetweenOrEqualTo(idx, 0, _first.Count);

			for (var i = 0; i < (uint)idx; i++)
				yield return _first[i];

			var cnt = (uint)_second.Count;
			for (var j = 0; j < cnt; j++)
				yield return _second[j];

			cnt = (uint)_first.Count;
			for (var i = idx; i < cnt; i++)
				yield return _first[i];
		}

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_first.CopyTo(array, arrayIndex);

			var span = array.AsSpan()[arrayIndex..];
			var cnt = _first.Count;
			var idx = _index.GetOffset(cnt);
			span[idx..cnt].CopyTo(span[(idx + _second.Count)..]);

			_second.CopyTo(array, arrayIndex + idx);
		}

		protected override T ElementAt(int index)
		{
			var idx = _index.GetOffset(_first.Count);
			Guard.IsBetweenOrEqualTo(idx, 0, _first.Count);
			Guard.IsBetweenOrEqualTo(index, 0, _first.Count + _second.Count - 1);

			return index < idx ? _first[index] :
				index < idx + _second.Count ? _second[index - idx] :
				_first[index - _second.Count];
		}
	}
}
