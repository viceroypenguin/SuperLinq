namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence with each null reference or value in the source
	/// replaced with the previous non-null reference or value seen in
	/// that sequence.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with null references or values
	/// replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If references or values are null at the start of the
	/// sequence then they remain null.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source)
	{
		return source.FillForward(e => new ValueTask<bool>(e == null));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the previous non-missing element seen in that sequence. An
	/// additional parameter specifies a function used to determine if an
	/// element is considered missing or not.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();

		return FillForwardImpl(source, i => new ValueTask<bool>(predicate(i)), fillSelector: null);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the previous non-missing element seen in that sequence. An
	/// additional parameter specifies a function used to determine if an
	/// element is considered missing or not.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();

		return FillForwardImpl(source, predicate, fillSelector: null);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with one based on the previous non-missing element seen in that
	/// sequence. Additional parameters specifiy two functions, one used to
	/// determine if an element is considered missing or not and another
	/// to provide the replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the previous non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T> fillSelector)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();
		fillSelector.ThrowIfNull();

		return FillForwardImpl(source, i => new ValueTask<bool>(predicate(i)), (a, b) => new ValueTask<T>(fillSelector(a, b)));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with one based on the previous non-missing element seen in that
	/// sequence. Additional parameters specifiy two functions, one used to
	/// determine if an element is considered missing or not and another
	/// to provide the replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the previous non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, T> fillSelector)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();
		fillSelector.ThrowIfNull();

		return FillForwardImpl(source, predicate, (a, b) => new ValueTask<T>(fillSelector(a, b)));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with one based on the previous non-missing element seen in that
	/// sequence. Additional parameters specifiy two functions, one used to
	/// determine if an element is considered missing or not and another
	/// to provide the replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the previous non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, Func<T, T, ValueTask<T>> fillSelector)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();
		fillSelector.ThrowIfNull();

		return FillForwardImpl(source, i => new ValueTask<bool>(predicate(i)), fillSelector);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with one based on the previous non-missing element seen in that
	/// sequence. Additional parameters specifiy two functions, one used to
	/// determine if an element is considered missing or not and another
	/// to provide the replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the previous non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the start of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillForward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, ValueTask<T>> fillSelector)
	{
		source.ThrowIfNull();
		predicate.ThrowIfNull();
		fillSelector.ThrowIfNull();

		return FillForwardImpl(source, predicate, fillSelector);
	}

	private static async IAsyncEnumerable<T> FillForwardImpl<T>(IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, ValueTask<T>>? fillSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		(bool, T) seed = default;

		await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			if (await predicate(item).ConfigureAwait(false))
			{
				yield return seed is (true, { } someSeed)
						   ? fillSelector != null
							 ? await fillSelector(item, someSeed).ConfigureAwait(false)
							 : someSeed
						   : item;
			}
			else
			{
				seed = (true, item);
				yield return item;
			}
		}
	}
}
