using System.Runtime.ExceptionServices;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Propagates the async-enumerable sequence that reacts first.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
	/// <param name="source">The first sequence to merge together</param>
	/// <param name="otherSources">The other sequences to merge together</param>
	/// <returns>An async-enumerable sequence that surfaces whichever sequence returned first.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="otherSources"/>, or any of
	/// the items in <paramref name="otherSources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The implementation of this method is deeply unfair with regards to the ordering of the input sequences. The
	/// sequences are initialized in the order in which they are received. This means that earlier sequences will have
	/// an opportunity to finish sooner, meaning that all other things being equal, the earlier a sequence is (where
	/// <paramref name="source"/> precedes any sequence in <paramref name="otherSources"/>), the more likely it will be
	/// chosen by this operator. Additionally, the first sequence to return the first element of the sequence
	/// synchronously will be chosen.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Amb<TSource>(
		this IAsyncEnumerable<TSource> source,
		params IAsyncEnumerable<TSource>[] otherSources)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(otherSources);

		foreach (var s in otherSources)
			Guard.IsNotNull(s, nameof(otherSources));

		return Amb(otherSources.Prepend(source));
	}

	/// <summary>
	/// Propagates the async-enumerable sequence that reacts first.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
	/// <param name="sources">The sequence of sequences to merge together</param>
	/// <returns>A sequence of every element from all source sequences, returned in an order based on how long it takes
	/// to iterate each element.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> or any of the items in <paramref
	/// name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The implementation of this method is deeply unfair with regards to the ordering of the <paramref
	/// name="sources"/>. The sequences in <paramref name="sources"/> are initialized in the order in which they are
	/// received. This means that earlier sequences will have an opportunity to finish sooner, meaning that all other
	/// things being equal, the earlier a sequence is in <paramref name="sources"/>, the more likely it will be chosen
	/// by this operator. Additionally, the first sequence to return the first element of the sequence synchronously
	/// will be chosen.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Amb<TSource>(
		this IEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		Guard.IsNotNull(sources);

		return Core(sources);

		static async IAsyncEnumerable<TSource> Core(
			IEnumerable<IAsyncEnumerable<TSource>> sources,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var cancellationSources = new List<CancellationTokenSource>();
			var enumerators = new List<IAsyncEnumerator<TSource>>();
			var tasks = new List<Task<bool>>();

			IAsyncEnumerator<TSource>? e = default;
			CancellationTokenSource? eCts = default;
			try
			{
				foreach (var s in sources)
				{
#pragma warning disable CA2000 // Dispose objects before losing scope
					// these will be disposed later
					var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
#pragma warning restore CA2000 // Dispose objects before losing scope

					var iter = s.GetAsyncEnumerator(cts.Token);

					var firstMove = iter.MoveNextAsync();
					if (firstMove.IsCompleted)
					{
						// if the sequence returned the first element synchronously, then it is obviously "first", so
						// choose it and forget the rest. we do not add this iter to the lists since those only track
						// the items that need to be canceled and disposed.
						e = iter;
						eCts = cts;

						// if the selected sequence is empty, then the amb sequence is empty as well.
						if (!firstMove.Result)
						{
							await iter.DisposeAsync().ConfigureAwait(false);
							cts.Dispose();
							yield break;
						}

						break;
					}

					// async; add it to the list
					cancellationSources.Add(cts);
					enumerators.Add(iter);
					tasks.Add(firstMove.AsTask());
				}

				if (e == null)
				{
					// who finishes first?
					var t = await Task.WhenAny(tasks).ConfigureAwait(false);
					var moveNext = await t.ConfigureAwait(false);

					// if the selected sequence is empty, then the amb sequence is empty as well.
					if (!moveNext)
						yield break;

					// since we built all three lists simultaneously, we can access the same index of each. 
					// we need the enumerator (to continue to enumerate it) and the cts (to dispose it at the end)
					var idx = tasks.IndexOf(t);
					e = enumerators[idx];
					eCts = cancellationSources[idx];

					// remove the selected item from the list of still-running iterators
					// however, if empty, then leave items in list so they can be disposed 
					cancellationSources.RemoveAt(idx);
					enumerators.RemoveAt(idx);
					tasks.RemoveAt(idx);
				}
			}
			finally
			{
				// give each still-running task a chance to bail early
				foreach (var cts in cancellationSources)
					cts.Cancel();

#pragma warning disable CA1031 // Do not catch general exception types
				ExceptionDispatchInfo? edi = null;
				try
				{
					_ = await Task.WhenAll(tasks).ConfigureAwait(false);
				}
				// because we canceled the cts, we might get OperationCanceledException; we don't actually care about
				// these because we're intentionally cancelling them.
				catch (Exception ex) when (
					ex is OperationCanceledException
					|| (ex is AggregateException ae && ae.InnerExceptions.All(e => e is OperationCanceledException)))
				{ }
				// if we're in the normal path, then e != null; in this case, we need to report any exceptions that we
				// encounter.
				catch (Exception ex) when (e != null)
				{
					edi = ExceptionDispatchInfo.Capture(ex);
				}
				// on the other hand, if e == null, then we silently ignore any exceptions, so that the original
				// exception can propagate normally. this matches the behavior of await Task.WhenAll which only throws
				// the first exception it encounters.
				catch { }

				foreach (var en in enumerators)
				{
					try
					{
						await en.DisposeAsync().ConfigureAwait(false);
					}
					// don't worry about any exceptions while disposing - theoretically these should be fast and
					// error-free, but just in case...
					catch { }
				}
#pragma warning restore CA1031 // Do not catch general exception types

				edi?.Throw();

				// properly dispose of the sources
				foreach (var cts in cancellationSources)
					cts.Dispose();
			}

			try
			{
				yield return e.Current;
				while (await e.MoveNextAsync().ConfigureAwait(false))
					yield return e.Current;
			}
			finally
			{
				await e.DisposeAsync().ConfigureAwait(false);
				eCts?.Dispose();
			}
		}
	}
}
