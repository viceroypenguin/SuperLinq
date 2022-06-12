using SuperLinq.Experimental;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Repeats the sequence the specified number of times.
	/// </summary>
	/// <typeparam name="T">Type of elements in sequence</typeparam>
	/// <param name="sequence">The sequence to repeat</param>
	/// <param name="count">Number of times to repeat the sequence</param>
	/// <returns>A sequence produced from the repetition of the original source sequence</returns>

	public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int count)
	{
		if (sequence == null) throw new ArgumentNullException(nameof(sequence));
		if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Repeat count must be greater than or equal to zero.");
		return RepeatImpl(sequence, count);
	}

	/// <summary>
	/// Repeats the sequence forever.
	/// </summary>
	/// <typeparam name="T">Type of elements in sequence</typeparam>
	/// <param name="sequence">The sequence to repeat</param>
	/// <returns>A sequence produced from the infinite repetition of the original source sequence</returns>

	public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence)
	{
		if (sequence == null) throw new ArgumentNullException(nameof(sequence));
		return RepeatImpl(sequence, null);
	}


	static IEnumerable<T> RepeatImpl<T>(IEnumerable<T> sequence, int? count)
	{
		var memo = sequence.Memoize();
		using (memo as IDisposable)
		{
			while (count == null || count-- > 0)
			{
				foreach (var item in memo)
					yield return item;
			}
		}
	}
}
