namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Asserts that a source sequence contains a given count of elements.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="count">Count to assert.</param>
	/// <returns>
	/// Returns the original sequence as long it is contains the number of elements specified by <paramref
	/// name="count"/>. Otherwise it throws <see cref="ArgumentException" />.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than <c>0</c>.</exception>
	/// <exception cref="ArgumentException"><paramref name="source"/> has a length different than <paramref
	/// name="count"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source, int count)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return Core(source, count);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, int count)
		{
			if (source.TryGetCollectionCount(out var c))
			{
				Guard.IsEqualTo(c, count, $"{nameof(source)}.Count()");

				foreach (var item in source)
					yield return item;
			}
			else
			{
				c = 0;
				foreach (var item in source)
				{
					if (++c > count)
						break;
					yield return item;
				}
				Guard.IsEqualTo(c, count, $"{nameof(source)}.Count()");
			}
		}
	}
}
