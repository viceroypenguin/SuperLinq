namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Determines whether or not the number of elements in the sequence is greater than or equal to the given
	///     integer.
	/// </summary>
	/// <typeparam name="T">
	///	    Element type of sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence
	/// </param>
	/// <param name="count">
	///     The minimum number of items a sequence must have for this function to return <see langword="true"/>.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if the number of elements in the sequence is greater than or equal to the given
	///     integer, <see langword="false"/> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool AtLeast<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count, count, int.MaxValue);
	}

	/// <summary>
	///	    Determines whether or not the number of elements in the sequence is lesser than or equal to the given
	///     integer.
	/// </summary>
	/// <typeparam name="T">
	///	    Element type of sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence
	/// </param>
	/// <param name="count">
	///	    The maximum number of items a sequence must have for this function to return <see langword="true"/>.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if the number of elements in the sequence is lesser than or equal to the given
	///     integer, <see langword="false"/> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool AtMost<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count + 1, 0, count);
	}

	/// <summary>
	///	    Determines whether or not the number of elements in the sequence is equals to the given integer.
	/// </summary>
	/// <typeparam name="T">
	///	    Element type of sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence
	/// </param>
	/// <param name="count">
	///	    The exactly number of items a sequence must have for this function to return <see langword="true"/>.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if the number of elements in the sequence is equals to the given integer, <see
	///     langword="false"/> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is negative.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool Exactly<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count + 1, count, count);
	}

	/// <summary>
	///	    Determines whether or not the number of elements in the sequence is between an inclusive range of minimum
	///     and maximum integers.
	/// </summary>
	/// <typeparam name="T">
	///	    Element type of sequence
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence
	/// </param>
	/// <param name="min">
	///	    The minimum number of items a sequence must have for this function to return <see langword="true"/>.
	/// </param>
	/// <param name="max">
	///	    The maximum number of items a sequence must have for this function to return <see langword="true"/>.
	/// </param>
	/// <returns>
	///	    <see langword="true"/> if the number of elements in the sequence is between (inclusive) the min and max
	///     given integers, <see langword="false"/> otherwise.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="min"/> is negative, or <paramref name="max"/> is less than <paramref name="min"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static bool CountBetween<T>(this IEnumerable<T> source, int min, int max)
	{
		Guard.IsGreaterThanOrEqualTo(min, 0);
		Guard.IsGreaterThanOrEqualTo(max, min);

		return QuantityIterator(source, max + 1, min, max);
	}

	private static bool QuantityIterator<T>(IEnumerable<T> source, int limit, int min, int max)
	{
		Guard.IsNotNull(source);

		var count = source.TryGetCollectionCount() ?? source.CountUpTo(limit);

		return count >= min && count <= max;
	}

	/// <summary>
	///	    Compares two sequences and returns an integer that indicates whether the first sequence has fewer, the same
	///     or more elements than the second sequence.
	/// </summary>
	/// <typeparam name="TFirst">
	///	    Element type of the first sequence.
	/// </typeparam>
	/// <typeparam name="TSecond">
	///	    Element type of the second sequence.
	/// </typeparam>
	/// <param name="first">
	///	    The first sequence.
	/// </param>
	/// <param name="second">
	///	    The second sequence.
	/// </param>
	/// <returns>
	///	    <c>-1</c> if the first sequence has the fewest elements, <c>0</c> if the two sequences have the same number
	///     of elements or <c>1</c> if the first sequence has the most elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static int CompareCount<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		if (first.TryGetCollectionCount() is int firstCount)
		{
			return firstCount.CompareTo(second.TryGetCollectionCount() ?? second.CountUpTo(firstCount + 1));
		}
		else if (second.TryGetCollectionCount() is int secondCount)
		{
			return first.CountUpTo(secondCount + 1).CompareTo(secondCount);
		}
		else
		{
			bool firstHasNext;
			bool secondHasNext;

			using var e1 = first.GetEnumerator();
			using (var e2 = second.GetEnumerator())
			{
				do
				{
					firstHasNext = e1.MoveNext();
					secondHasNext = e2.MoveNext();
				}
				while (firstHasNext && secondHasNext);
			}

			return firstHasNext.CompareTo(secondHasNext);
		}
	}

	private static int CountUpTo<T>(this IEnumerable<T> source, int max)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(max, 0);

		var count = 0;

		using var e = source.GetEnumerator();
		while (count < max && e.MoveNext())
			count++;

		return count;
	}
}
