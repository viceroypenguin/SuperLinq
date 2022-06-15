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
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
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
		first.ThrowIfNull();
		second.ThrowIfNull();
		if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

		return _(); IEnumerable<T> _()
		{
			var i = -1;

			using var iter = first.GetEnumerator();

			while (++i < index && iter.MoveNext())
				yield return iter.Current;

			if (i < index)
				throw new ArgumentOutOfRangeException(nameof(index), "Insertion index is greater than the length of the first sequence.");

			foreach (var item in second)
				yield return item;

			while (iter.MoveNext())
				yield return iter.Current;
		}
	}
}
