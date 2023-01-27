namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Excludes a contiguous number of elements from a sequence starting at a given index.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the sequence</typeparam>
	/// <param name="sequence">The sequence to exclude elements from</param>
	/// <param name="startIndex">The zero-based index at which to begin excluding elements</param>
	/// <param name="count">The number of elements to exclude</param>
	/// <returns>A sequence that excludes the specified portion of elements</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="startIndex"/> or <paramref name="count"/> is less than <c>0</c>.
	/// </exception>
	public static IEnumerable<T> Exclude<T>(this IEnumerable<T> sequence, int startIndex, int count)
	{
		Guard.IsNotNull(sequence);
		Guard.IsGreaterThanOrEqualTo(startIndex, 0);
		Guard.IsGreaterThanOrEqualTo(count, 0);

		if (count == 0)
			return sequence;

		return _(sequence, startIndex, count);

		static IEnumerable<T> _(IEnumerable<T> sequence, int startIndex, int count)
		{
			var index = 0;
			var endIndex = startIndex + count;

			foreach (var item in sequence)
			{
				if (index < startIndex || index >= endIndex)
					yield return item;
				index++;
			}
		}
	}
}
