namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence with each null reference or value in the source
	/// replaced with the following non-null reference or value in
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
	/// results. If references or values are null at the end of the
	/// sequence then they remain null.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source)
	{
		return source.FillBackward(e => new ValueTask<bool>(e is null));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. An
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
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);

		return FillBackwardImpl(source, i => new ValueTask<bool>(predicate(i)), fillSelector: null);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. An
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
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);

		return FillBackwardImpl(source, predicate, fillSelector: null);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. Additional
	/// parameters specify two functions, one used to determine if an
	/// element is considered missing or not and another to provide the
	/// replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the next non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T> fillSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(fillSelector);

		return FillBackwardImpl(source, i => new ValueTask<bool>(predicate(i)), (a, b) => new ValueTask<T>(fillSelector(a, b)));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. Additional
	/// parameters specify two functions, one used to determine if an
	/// element is considered missing or not and another to provide the
	/// replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the next non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, T> fillSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(fillSelector);

		return FillBackwardImpl(source, predicate, (a, b) => new ValueTask<T>(fillSelector(a, b)));
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. Additional
	/// parameters specify two functions, one used to determine if an
	/// element is considered missing or not and another to provide the
	/// replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the next non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, Func<T, T, ValueTask<T>> fillSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(fillSelector);

		return FillBackwardImpl(source, i => new ValueTask<bool>(predicate(i)), fillSelector);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. Additional
	/// parameters specify two functions, one used to determine if an
	/// element is considered missing or not and another to provide the
	/// replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the next non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>

	public static IAsyncEnumerable<T> FillBackward<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, ValueTask<T>> fillSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(fillSelector);

		return FillBackwardImpl(source, predicate, fillSelector);
	}

	private static async IAsyncEnumerable<T> FillBackwardImpl<T>(IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate, Func<T, T, ValueTask<T>>? fillSelector, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		List<T>? blanks = null;

		await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			var isBlank = await predicate(item).ConfigureAwait(false);
			if (isBlank)
			{
				(blanks ??= []).Add(item);
			}
			else
			{
				if (blanks is not null)
				{
					foreach (var blank in blanks)
					{
						yield return fillSelector is not null
							? await fillSelector(blank, item).ConfigureAwait(false)
							: item;
					}

					blanks.Clear();
				}

				yield return item;
			}
		}

		if (blanks?.Count > 0)
		{
			foreach (var blank in blanks)
				yield return blank;
		}
	}
}
