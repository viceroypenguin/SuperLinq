namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);

		return Do(source, onNext.ToAsync(), onCompleted: () => default);
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> onNext)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);

		return Do(source, onNext, onCompleted: () => default);
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/>, or <paramref
	/// name="onCompleted"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return Do(source, onNext.ToAsync(), onCompleted.ToAsync());
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/>, or <paramref
	/// name="onCompleted"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> onNext, Func<ValueTask> onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return Core(source, onNext, onCompleted);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, ValueTask> onNext,
			Func<ValueTask> onCompleted,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var el in source
				.WithCancellation(cancellationToken)
				.ConfigureAwait(false))
			{
				await onNext(el).ConfigureAwait(false);
				yield return el;
			}

			await onCompleted().ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/>, or <paramref
	/// name="onError"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onError);

		return Do(source, onNext.ToAsync(), onError.ToAsync(), onCompleted: () => default);
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/>, or <paramref
	/// name="onError"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> onNext, Func<Exception, ValueTask> onError)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onError);

		return Do(source, onNext, onError, onCompleted: () => default);
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional
	/// termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/> <paramref
	/// name="onError"/>, or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onError);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return Do(source, onNext.ToAsync(), onError.ToAsync(), onCompleted.ToAsync());
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional
	/// termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="onNext"/> <paramref
	/// name="onError"/>, or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask> onNext, Func<Exception, ValueTask> onError, Func<ValueTask> onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onError);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return Core(source, onNext, onError, onCompleted);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, ValueTask> onNext,
			Func<Exception, ValueTask> onError,
			Func<ValueTask> onCompleted,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var iter = source.GetConfiguredAsyncEnumerator(cancellationToken);
			while (true)
			{
				try
				{
					if (!await iter.MoveNextAsync())
						break;
				}
				catch (Exception ex)
				{
					await onError(ex).ConfigureAwait(false);
					throw;
				}

				var current = iter.Current;
				await onNext(current).ConfigureAwait(false);
				yield return current;
			}

			await onCompleted().ConfigureAwait(false);
		}
	}
}
