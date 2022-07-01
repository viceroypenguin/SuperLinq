namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence from an array of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The array that is returned as the alternate
	/// sequence if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is null.</exception>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, params T[] fallback)
	{
		return source.FallbackIfEmpty((IEnumerable<T>)fallback);
	}

	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The alternate sequence that is returned
	/// if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is null.</exception>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> fallback)
	{
		source.ThrowIfNull();
		fallback.ThrowIfNull();

		return _(source, fallback);

		static IEnumerable<T> _(IEnumerable<T> source, IEnumerable<T> fallback)
		{
			using (var e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					do { yield return e.Current; }
					while (e.MoveNext());
					yield break;
				}
			}

			foreach (var item in fallback)
				yield return item;
		}
	}
}
