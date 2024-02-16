namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to segment
	/// </param>
	/// <param name="newSegmentPredicate">
	///	    A function, which returns <see langword="true"/> if the given element begins a new segment, and <see
	///     langword="false"/> otherwise
	/// </param>
	/// <returns>
	///	    A sequence of segment, each of which is a portion of the original sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="newSegmentPredicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </para>
	/// </remarks>
	public static IEnumerable<IReadOnlyList<T>> Segment<T>(this IEnumerable<T> source, Func<T, bool> newSegmentPredicate)
	{
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => newSegmentPredicate(curr));
	}

	/// <summary>
	///	    Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to segment
	/// </param>
	/// <param name="newSegmentPredicate">
	///	    A function, which returns <see langword="true"/> if the given element and index begins a new segment, and
	///     <see langword="false"/> otherwise
	/// </param>
	/// <returns>
	///	    A sequence of segment, each of which is a portion of the original sequence
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="newSegmentPredicate"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </para>
	/// </remarks>
	public static IEnumerable<IReadOnlyList<T>> Segment<T>(this IEnumerable<T> source, Func<T, int, bool> newSegmentPredicate)
	{
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => newSegmentPredicate(curr, index));
	}

	/// <summary>
	///	    Divides a sequence into multiple sequences by using a segment detector based on the original sequence
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequence
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to segment
	/// </param>
	/// <param name="newSegmentPredicate">
	///	    A function, which returns <see langword="true"/> if the given current element, previous element, and index
	///     begins a new segment, and <see langword="false"/> otherwise
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="newSegmentPredicate"/> is <see langword="null"/>.
	/// </exception>
	/// <returns>
	///	    A sequence of segment, each of which is a portion of the original sequence
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution and streams the groupings. The grouping elements,
	///     however, are buffered. Each grouping is therefore yielded as soon as it is complete and before the next
	///     grouping occurs.
	/// </para>
	/// </remarks>
	public static IEnumerable<IReadOnlyList<T>> Segment<T>(this IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Core(source, newSegmentPredicate);

		static IEnumerable<IReadOnlyList<T>> Core(IEnumerable<T> source, Func<T, T, int, bool> newSegmentPredicate)
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
					segment = [];
				}

				segment.Add(current);

				previous = current;
			}

			yield return segment;
		}
	}
}
