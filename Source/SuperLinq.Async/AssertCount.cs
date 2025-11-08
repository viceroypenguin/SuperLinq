namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	///	    Asserts that a source sequence contains a given count of elements.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in <paramref name="source"/> sequence.
	///	</typeparam>
	/// <param name="source">
	///	    Source sequence.
	///	</param>
	/// <param name="count">
	///	    Count to assert.
	///	</param>
	/// <returns>
	///	    Returns the original sequence as long it is contains the number of elements specified by <paramref
	///     name="count"/>. Otherwise it throws <see cref="InvalidOperationException" />.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	///	</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="count"/> is less than <c>0</c>.
	///	</exception>
	/// <exception cref="InvalidOperationException">
	///	    Thrown lazily <paramref name="source"/> has a length different than <paramref name="count"/>.
	///	</exception>
	/// <remarks>
	/// <para>
	///		This operator uses deferred execution and streams its results.
	/// </para>
	/// <para>
	///	    The sequence length is evaluated lazily during the enumeration of the sequence.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> AssertCount<TSource>(this IAsyncEnumerable<TSource> source, int count)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(count);

		return Core(source, count);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			int count,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var c = 0;
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (++c > count)
					break;

				yield return item;
			}

			AssertSequenceCount(count, c);
		}
	}

	private static void AssertSequenceCount(int expected, int actual)
	{
		if (expected != actual)
			ThrowHelper.ThrowInvalidOperationException($"Sequence contains too {(actual < expected ? "few" : "many")} elements when exactly '{expected:N0}' {(expected == 1 ? "was" : "were")} expected.");
	}
}
