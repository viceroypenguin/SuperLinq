namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a sequence that corresponds to the source sequence, concatenating it with the sequence resulting from
	/// calling an exception handler function in case of an error.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TException">Exception type to catch.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="handler">Handler to invoke when an exception of the specified type occurs.</param>
	/// <returns>Source sequence, concatenated with an exception handler result sequence in case of an error.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="handler"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Catch<TSource, TException>(
		this IAsyncEnumerable<TSource> source,
		Func<TException, IAsyncEnumerable<TSource>> handler)
		where TException : Exception
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(handler);

		return Core(source, handler);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TException, IAsyncEnumerable<TSource>> handler,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			IAsyncEnumerable<TSource>? errSource;
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
			while (true)
			{
				try
				{
					if (!await e.MoveNextAsync())
						yield break;
				}
				catch (TException ex)
				{
					errSource = handler(ex);
					break;
				}

				yield return e.Current;
			}

			Assert.NotNull(errSource);

			await foreach (var item in errSource.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return item;
		}
	}

	/// <summary>
	/// Creates a sequence that returns the elements of the first sequence, switching to the second in case of an error.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="first">First sequence.</param>
	/// <param name="second">Second sequence, concatenated to the result in case the first sequence completes
	/// exceptionally.</param>
	/// <returns>The first sequence, followed by the second sequence in case an error is produced.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		return Catch(new[] { first, second, });
	}

	/// <summary>
	/// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Catch<TSource>(params IAsyncEnumerable<TSource>[] sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return sources.ToAsyncEnumerable().Catch();
	}

	/// <summary>
	/// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Catch<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return sources.ToAsyncEnumerable().Catch();
	}

	/// <summary>
	/// Creates a sequence by concatenating source sequences until a source sequence completes successfully.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence that continues to concatenate source sequences while errors occur.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This method uses deferred execution and streams its results.
	/// </remarks>
	public static IAsyncEnumerable<TSource> Catch<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return Core(sources);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<IAsyncEnumerable<TSource>> sources,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var sourceIter = sources.GetConfiguredAsyncEnumerator(cancellationToken);

			if (!await sourceIter.MoveNextAsync())
				yield break;

			var source = sourceIter.Current;
			var hasNext = await sourceIter.MoveNextAsync();

			// outer loop is not infinite.
			// on last loop (`hasNext == false`), then either
			// `source` will iterate successfully (yield break)
			// or it will fail (throw). either way, it will not
			// make it outside of the inner `while (true)`
			while (true)
			{
				ArgumentNullException.ThrowIfNull(source);
				await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

				while (true)
				{
					try
					{
						if (!await e.MoveNextAsync())
							yield break;
					}
					catch
					{
						if (!hasNext)
							throw;

						break;
					}

					yield return e.Current;
				}

				source = sourceIter.Current;
				hasNext = await sourceIter.MoveNextAsync();
			}
		}
	}
}
