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
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);
		Guard.IsGreaterThanOrEqualTo(index, 0);

		return Core(first, second, index);

		static IEnumerable<T> Core(IEnumerable<T> first, IEnumerable<T> second, int index)
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

		return !index.IsFromEnd ? Insert(first, second, index.Value) :
			index.Value == 0 ? first.Concat(second) :
			Core(first, second, index.Value);

		static IEnumerable<T> Core(IEnumerable<T> first, IEnumerable<T> second, int index)
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
}
