namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Concurrently iterates multiple <see cref="IAsyncEnumerable{T}"/>
	/// and returns elements from each stream in the order in which the
	/// iteration completes (which may or may not be in the same order
	/// as the sources are provided).
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
	/// <param name="source">The first sequence to merge together</param>
	/// <param name="otherSources">The other sequences to merge together</param>
	/// <returns>A sequence of every element from all source sequences, returned in an order based on how long it takes to iterate each element.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSources"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">Any of the items in <paramref name="otherSources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method is very similar to <see cref="Interleave{T}(IAsyncEnumerable{T}, IAsyncEnumerable{T}[])"/>;
	/// however, that method executes each iterator sequentially returning each item in a fixed order, while
	/// this method executes the iterators in parallel, returning each item as they are received.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ConcurrentMerge<TSource>(
		this IAsyncEnumerable<TSource> source,
		params IAsyncEnumerable<TSource>[] otherSources)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(otherSources);

		return ConcurrentMerge(otherSources.Prepend(source), int.MaxValue);
	}

	/// <summary>
	/// Concurrently iterates multiple <see cref="IAsyncEnumerable{T}"/>
	/// and returns elements from each stream in the order in which the
	/// iteration completes (which may or may not be in the same order
	/// as the sources are provided).
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
	/// <param name="sources">The sequence of sequences to merge together</param>
	/// <returns>A sequence of every element from all source sequences, returned in an order based on how long it takes to iterate each element.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">Any of the items in <paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method is very similar to <see cref="Interleave{T}(IAsyncEnumerable{T}, IAsyncEnumerable{T}[])"/>;
	/// however, that method executes each iterator sequentially returning each item in a fixed order, while
	/// this method executes the iterators in parallel, returning each item as they are received.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ConcurrentMerge<TSource>(
		this IEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		return ConcurrentMerge(sources, int.MaxValue);
	}

	/// <summary>
	/// Concurrently iterates multiple <see cref="IAsyncEnumerable{T}"/>
	/// and returns elements from each stream in the order in which the
	/// iteration completes (which may or may not be in the same order
	/// as the sources are provided).
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of the source sequences</typeparam>
	/// <param name="sources">The sequence of sequences to merge together</param>
	/// <param name="maxConcurrency">The maximum number of outstanding iteration operations allowed at any given time.</param>
	/// <returns>A sequence of every element from all source sequences, returned in an order based on how long it takes to iterate each element.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">Any of the items in <paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method is very similar to <see cref="Interleave{T}(IAsyncEnumerable{T}, IAsyncEnumerable{T}[])"/>;
	/// however, that method executes each iterator sequentially returning each item in a fixed order, while
	/// this method executes the iterators in parallel, returning each item as they are received.
	/// </remarks>
	public static IAsyncEnumerable<TSource> ConcurrentMerge<TSource>(
		this IEnumerable<IAsyncEnumerable<TSource>> sources,
		int maxConcurrency)
	{
		Guard.IsNotNull(sources);
		Guard.IsGreaterThanOrEqualTo(maxConcurrency, 1);

		foreach (var s in sources)
			Guard.IsNotNull(s, nameof(sources));

		return Core(sources, maxConcurrency);

		static async IAsyncEnumerable<TSource> Core(
			IEnumerable<IAsyncEnumerable<TSource>> sources,
			int maxConcurrency,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			cancellationToken = cts.Token;

			var list = new List<IAsyncEnumerator<TSource>>();
			var active = new List<bool>();
			var pendingTaskList = new List<Task>();
			var i = 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void DisposeAsync(IAsyncEnumerator<TSource> it)
			{
				// need to remove both iterator and active status for it
				var idx = list.IndexOf(it);
				list.RemoveAt(idx);
				active.RemoveAt(idx);

				// since we're trying to round-robin, if current index
				// is after the iterator, we need to backtrack to account
				// for missing element in list
				if (i > idx) i--;
				else if (i >= list.Count) i = 0;

				// try to dispose
				var disposalTask = it.DisposeAsync();
				// if it's not immediate, then pending task for sure
				if (!disposalTask.IsCompleted)
					pendingTaskList.Add(disposalTask.AsTask());
			}

			static async ValueTask<(T1, T2)> And<T1, T2>(ValueTask<T1> t1, T2 t2) =>
				(await t1.ConfigureAwait(false), t2);

			try
			{
				foreach (var source in sources)
				{
					list.Add(source.GetAsyncEnumerator(cancellationToken));
					active.Add(item: false);
				}

				// while we still have iterators to process
				while (true)
				{
					// try to iterate sequences; return values if they iterate immediately
					// otherwise, queue up one task
					while (list.Count != 0
						&& pendingTaskList.Count < list.Count)
					{
						// look for an inactive task
						while (active[i])
							i = i + 1 >= list.Count ? 0 : i + 1;

						// found one, iterate it
						var e = list[i];
						var task = e.MoveNextAsync(cancellationToken);

						// returned immediately?
						if (task.IsCompleted)
						{
							// yes, so either return it or dispose it
							if (await task.ConfigureAwait(false))
							{
								yield return e.Current;
								// and go to the next iterator
								i = i + 1 >= list.Count ? 0 : i + 1;
							}
							else
							{
								DisposeAsync(e);
							}
						}
						else
						{
							// no, so mark it active and queue it up
							active[i] = true;
							pendingTaskList.Add(And(task, e).AsTask());
							// and go to the next iterator
							i = i + 1 >= list.Count ? 0 : i + 1;
							// only queue one at a time, so we can dequeue
							// as quickly as possible
							break;
						}
					}

					// only way to escape the while above without a pending task
					// is if the list is empty; if list is empty *and* no pending
					// tasks, then iteration is complete; get out.
					if (pendingTaskList.Count == 0)
						yield break;

					// deal with any pending items that exist
					// in some cases, we need to wait for someone to finish
					if (
						// if we've reached max concurrency
						pendingTaskList.Count >= maxConcurrency
						// if we have a task for every remaining list item
						// also covers if we don't have any iterators left (we need to flush the pending tasks)
						|| pendingTaskList.Count >= list.Count)
					{
						// go to sleep until we hear from someone
						// (don't care who woke us up, we'll cover that with the loop below)
						await Task.WhenAny(pendingTaskList).ConfigureAwait(false);
					}

					// clear out all completed tasks
					for (var j = 0; j < pendingTaskList.Count; j++)
					{
						// only deal with completed ones
						if (!pendingTaskList[j].IsCompleted)
							continue;

						// if this is an iterator task
						if (pendingTaskList[j] is Task<ValueTuple<bool, IAsyncEnumerator<TSource>>> t)
						{
							// did we move or not?
							var (moved, it) = await t.ConfigureAwait(false);
							if (moved)
							{
								// yes, so task is no longer active
								active[list.IndexOf(it)] = false;
								// and return it
								yield return it.Current;
							}
							else
							{
								// no, so dispose it
								DisposeAsync(it);
							}
						}

						// iterator or dispose task, we need to get rid of it
						pendingTaskList.RemoveAt(j);

						// because we're removing an item, we need to go back so the increment puts us on the same element
						j--;
					}
				}
			}
			finally
			{
				// since we can't `catch`, we'll deal with exceptions in `finally`
				// list.Count will be non-zero iff an exception is thrown
				if (list.Count != 0)
				{
					cts.Cancel();

#pragma warning disable CA1031 // Do not catch general exception types
					try
					{
						// clear out remaining pending tasks
						await Task.WhenAll(pendingTaskList).ConfigureAwait(false);
					}
					// we're already in the throw path of an exception, we don't need
					// to throw any other exceptions; this matches behavior of await Task.WhenAll
					// which only throws the first exception it encounters
					catch { }

					pendingTaskList.Clear();

					try
					{
						// don't foregt to dispose the remaining iterators too
						foreach (var e in list)
						{
							var t = e.DisposeAsync();
							if (!t.IsCompleted)
								pendingTaskList.Add(t.AsTask());
						}

						await Task.WhenAll(pendingTaskList).ConfigureAwait(false);
					}
					catch { }
#pragma warning restore CA1031 // Do not catch general exception types
				}
			}
		}
	}
}
