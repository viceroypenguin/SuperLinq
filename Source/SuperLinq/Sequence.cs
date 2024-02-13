namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Generates a sequence of integral numbers within a specified range.
	/// </summary>
	/// <param name="start">
	///	    The value of the first integer in the sequence.
	/// </param>
	/// <param name="count">
	///	    The number of sequential integers to generate.
	/// </param>
	/// <param name="step">
	///	    The step increment for each returned value.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{Int32}"/>that contains a range of sequential integral numbers.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than 0. -or- <paramref name="start"/> + (<paramref name="count"/> -1) *
	///     <paramref name="step"/> cannot be contained by an <see cref="int"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///		This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Range(int start, int count, int step)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(count);

		var max = start + ((count - 1) * (long)step);
		ArgumentOutOfRangeException.ThrowIfLessThan(max, int.MinValue, nameof(count));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(max, int.MaxValue, nameof(count));

		return new SequenceIterator(start, step, count);
	}

	/// <summary>
	///	    Generates a sequence of integral numbers within the (inclusive) specified range. If sequence is ascending
	///     the step is +1, otherwise -1.
	/// </summary>
	/// <param name="start">
	///	    The value of the first integer in the sequence.
	/// </param>
	/// <param name="stop">
	///	    The value of the last integer in the sequence.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{Int32}"/> that contains a range of sequential integral numbers.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Sequence(int start, int stop)
	{
		return new SequenceIterator(start, start <= stop ? 1 : -1, Math.Abs((long)stop - start) + 1);
	}

	/// <summary>
	///	    Generates a sequence of integral numbers within the (inclusive) specified range. An additional parameter
	///     specifies the steps in which the integers of the sequence increase or decrease.
	/// </summary>
	/// <param name="start">
	///	    The value of the first integer in the sequence.
	/// </param>
	/// <param name="stop">
	///	    The value of the last integer in the sequence.
	/// </param>
	/// <param name="step">
	///	    The step to define the next number.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{Int32}"/> that contains a range of sequential integral numbers.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    When <paramref name="step"/> is equal to zero, this operator returns an infinite sequence where all elements
	///     are equals to <paramref name="start"/>. This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<int> Sequence(int start, int stop, int step)
	{
		if (step == 0)
			return Repeat(start);

		if (stop == start)
			return Return(start);

		if (Math.Sign((long)stop - start) != Math.Sign(step))
			return [];

		return new SequenceIterator(start, step, (((long)stop - start) / step) + 1);
	}

	private sealed class SequenceIterator(
		int start,
		int step,
		long count
	) : ListIterator<int>
	{
		public override int Count => count <= int.MaxValue ? (int)count : int.MaxValue;

		protected override IEnumerable<int> GetEnumerable()
		{
			var value = start;
			for (var i = 0; i < Count; i++)
			{
				yield return value;
				value += step;
			}
		}

		protected override int ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return start + (step * index);
		}
	}
}
