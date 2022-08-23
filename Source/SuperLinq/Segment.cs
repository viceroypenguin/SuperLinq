namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence to segment</param>
	/// <param name="newSegmentPredicate">A function, which returns <see langword="true"/> if the given element begins a new segment, and <see langword="false"/> otherwise</param>
	/// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
	/// </exception>

	public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, bool> newSegmentPredicate)
	{
		Guard.IsNotNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => newSegmentPredicate(curr));
	}

	/// <summary>
	/// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence to segment</param>
	/// <param name="newSegmentPredicate">A function, which returns <see langword="true"/> if the given element or index indicate a new segment, and <see langword="false"/> otherwise</param>
	/// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
	/// </exception>

	public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, int, bool> newSegmentPredicate)
	{
		Guard.IsNotNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => newSegmentPredicate(curr, index));
	}

	/// <summary>
	/// Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence to segment</param>
	/// <param name="newSegmentPredicate">A function, which returns <see langword="true"/> if the given current element, previous element or index indicate a new segment, and <see langword="false"/> otherwise</param>
	/// <returns>A sequence of segment, each of which is a portion of the original sequence</returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="newSegmentPredicate"/> are <see langword="null"/>.
	/// </exception>

	public static IEnumerable<IEnumerable<T>> Segment<T>(this IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(newSegmentPredicate);

		return _(source, newSegmentPredicate);

		static IEnumerable<IEnumerable<T>> _(IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
		{
			using var e = source.GetEnumerator();

			if (!e.MoveNext()) // break early (it's empty)
				yield break;

			// Ensure that the first item is always part of the first
			// segment. This is an intentional behavior. Segmentation always
			// begins with the second element in the sequence.

			var previous = e.Current;
			var segment = new List<T> { previous };

			for (var index = 1; e.MoveNext(); index++)
			{
				var current = e.Current;

				if (newSegmentPredicate(current, previous, index))
				{
					yield return segment;
					segment = new List<T>();
				}

				segment.Add(current);

				previous = current;
			}

			yield return segment;
		}
	}
}
