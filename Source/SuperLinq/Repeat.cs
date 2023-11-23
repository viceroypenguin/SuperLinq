namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Generates a sequence by repeating the given value infinitely.
	/// </summary>
	/// <typeparam name="TResult">
	///	    Result sequence element type.
	/// </typeparam>
	/// <param name="value">
	///	    Value to repeat in the resulting sequence.
	/// </param>
	/// <returns>
	///	    Sequence repeating the given value infinitely.
	/// </returns>
	public static IEnumerable<TResult> Repeat<TResult>(TResult value)
	{
		while (true)
			yield return value;
	}

	/// <summary>
	///	    Repeats and concatenates the source sequence infinitely.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <returns>
	///	    Sequence obtained by concatenating the source sequence to itself infinitely.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result. The <paramref name="source"/> sequence is
	///     cached as the returned <see cref="IEnumerable{T}"/> is enumerated. When <paramref name="source"/> has
	///     completed, the values from <paramref name="source"/> will be returned again indefinitely. 
	/// </para>
	/// <para>
	///	    The cache is maintained separately for each <see cref="IEnumerator{T}"/> generated from the returned <see
	///     cref="IEnumerable{T}"/>.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Core(source);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source)
		{
			using var buffer = source.Memoize();
			while (true)
			{
				foreach (var el in buffer)
					yield return el;
			}
		}
	}

	/// <summary>
	///	    Repeats and concatenates the source sequence the given number of times.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="count">
	///	    Number of times to repeat the source sequence.
	///	</param>
	/// <returns>
	///	    Sequence obtained by concatenating the source sequence to itself the specified number of times.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than or equal to <c>0</c>.
	///	</exception>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result. The <paramref name="source"/> sequence is
	///     cached as the returned <see cref="IEnumerable{T}"/> is enumerated. When <paramref name="source"/> has
	///     completed, the values from <paramref name="source"/> will be returned again <paramref name="count"/> times. 
	/// </para>
	/// <para>
	///	    The cache is maintained separately for each <see cref="IEnumerator{T}"/> generated from the returned <see
	///     cref="IEnumerable{T}"/>.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int count)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

		return Core(source, count);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, int count)
		{
			using var buffer = source.Memoize();
			while (count-- > 0)
			{
				foreach (var el in buffer)
					yield return el;
			}
		}
	}
}
