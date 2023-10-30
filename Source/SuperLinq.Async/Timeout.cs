namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Applies a timeout policy for each element in the async-enumerable sequence. If the next element isn't received
	/// within the specified timeout duration, a <see cref="TimeoutException"/> is propagated to the consumer.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
	/// <param name="source">Source sequence to perform a timeout for.</param>
	/// <param name="timeout">Maximum duration between values before a timeout occurs.</param>
	/// <returns>The source sequence with a <see cref="TimeoutException"/> in case of a timeout.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is less than <see
	/// cref="TimeSpan.Zero"/>.</exception>
	/// <exception cref="TimeoutException">If no element is produced within <paramref name="timeout"/> from the previous
	/// element.</exception>
	/// <remarks>
	/// <para>
	/// Specifying a <see cref="TimeSpan.Zero"/> value for <paramref name="timeout"/> is not recommended but supported,
	/// causing timeout timers to be scheduled that are due immediately. However, this doesn't guarantee a timeout will
	/// occur. If the iteration is synchronous, then the timeout will not be evaluated at all. Additionally, even a <see
	/// cref="TimeSpan.Zero"/> timeout has a minimum time to be handled, and the action to propagate a timeout may not
	/// execute immediately. In such cases, the next element may arrive before the scheduler gets a chance to run the
	/// timeout action.
	/// </para>
	/// <para>
	/// <b>Note</b>: If <paramref name="source"/> is completely synchronous, then the <paramref name="timeout"/> will
	/// not be evaluated at all. If the iteration is synchronous and takes longer than <paramref name="timeout"/>, no
	/// exception will be thrown.
	/// </para>
	/// <para>
	/// This operator does not throw immediately on the expiration of <paramref name="timeout"/>. It is a violation of
	/// spec to attempt to dispose or otherwise interact with an <see cref="IAsyncEnumerator{T}"/> while the <see
	/// cref="IAsyncEnumerator{T}.MoveNextAsync"/> task is not completed. The <see cref="CancellationToken"/> provided
	/// to the inner enumerable will be canceled when the <paramref name="timeout"/> is expired, but this operator will
	/// continue to wait until the task is complete or canceled before throwing a <see cref="TimeoutException"/>.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Timeout<TSource>(this IAsyncEnumerable<TSource> source, TimeSpan timeout)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentOutOfRangeException.ThrowIfNegative(timeout.Milliseconds);

		return Core(source, timeout);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source, TimeSpan timeout,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			var e = source.GetAsyncEnumerator(cts.Token);
			await using (e.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				while (true)
				{
					var moveNextVt = e.MoveNextAsync();

					if (!moveNextVt.IsCompleted)
					{
						try
						{
							var moveNextTask = moveNextVt.AsTask();
							var delayTask = Task.Delay(timeout, cancellationToken);
							var successTask = await Task.WhenAny(moveNextTask, delayTask).ConfigureAwait(false);

							if (successTask == delayTask)
							{
#if NET8_0_OR_GREATER
								await cts.CancelAsync();
#else
								cts.Cancel();
#endif

								_ = await moveNextTask.ConfigureAwait(false);
								throw new TimeoutException("The operation has timed out.");
							}
						}
						catch (OperationCanceledException ex) when (cts.IsCancellationRequested)
						{
							throw new TimeoutException("The operation has timed out.", ex);
						}
					}

					var moveNext = moveNextVt.Result;
					if (!moveNext)
						yield break;

					yield return e.Current;
				}
			}
		}
	}
}
