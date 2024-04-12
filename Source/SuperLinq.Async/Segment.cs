namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(this IAsyncEnumerable<T> source, Func<T, bool> newSegmentPredicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => new ValueTask<bool>(newSegmentPredicate(curr)));
	}

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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> newSegmentPredicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(
		this IAsyncEnumerable<T> source,
		Func<T, int, bool> newSegmentPredicate
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => new ValueTask<bool>(newSegmentPredicate(curr, index)));
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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(
		this IAsyncEnumerable<T> source,
		Func<T, int, ValueTask<bool>> newSegmentPredicate
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(
		this IAsyncEnumerable<T> source,
		Func<T, T, int, bool> newSegmentPredicate
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Segment(source, (curr, prev, index) => new ValueTask<bool>(newSegmentPredicate(curr, prev, index)));
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

	public static IAsyncEnumerable<IReadOnlyList<T>> Segment<T>(
		this IAsyncEnumerable<T> source,
		Func<T, T, int, ValueTask<bool>> newSegmentPredicate
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(newSegmentPredicate);

		return Core(source, newSegmentPredicate);

		static async IAsyncEnumerable<IReadOnlyList<T>> Core(
			IAsyncEnumerable<T> source,
			Func<T, T, int, ValueTask<bool>> newSegmentPredicate,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

			if (!await e.MoveNextAsync()) // break early (it's empty)
				yield break;

			// Ensure that the first item is always part of the first
			// segment. This is an intentional behavior. Segmentation always
			// begins with the second element in the sequence.

			var previous = e.Current;
			var segment = new List<T> { previous };

			for (var index = 1; await e.MoveNextAsync(); index++)
			{
				var current = e.Current;

				if (await newSegmentPredicate(current, previous, index).ConfigureAwait(false))
				{
					yield return segment; // yield the completed segment
					segment = [current]; // start a new segment
				}
				else // not a new segment, append and continue
				{
					segment.Add(current);
				}

				previous = current;
			}

			yield return segment;
		}
	}
}
