namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Selects elements by index from a sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="indices">The list of indices of elements in the <paramref name="source"/> sequence to select.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> whose elements are the result of selecting elements according to the <paramref name="indices"/> sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="indices"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">An index in <paramref name="indices"/> is out of range for the input sequence <paramref name="source"/>.</exception>
	public static IAsyncEnumerable<TSource> BindByIndex<TSource>(
		this IAsyncEnumerable<TSource> source,
		IAsyncEnumerable<int> indices
	)
	{
		return BindByIndex(
			source,
			indices,
			static (e, i) => e,
			static i => ThrowHelper.ThrowArgumentOutOfRangeException<TSource>(
				nameof(indices),
				"Index is greater than the length of the first sequence."
			)
		);
	}

	/// <summary>
	/// Selects elements by index from a sequence and transforms them using the provided functions.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="indices">The list of indices of elements in the <paramref name="source"/> sequence to select.</param>
	/// <param name="resultSelector">A transform function to apply to each source element; the second parameter of the function represents the index of the output sequence.</param>
	/// <param name="missingSelector">A transform function to apply to missing source elements; the parameter represents the index of the output sequence.</param>
	/// <returns>
	/// An <see cref="IAsyncEnumerable{T}"/> whose elements are the result of selecting elements according to the <paramref name="indices"/> sequence
	/// and invoking the transform function.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="indices"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="missingSelector"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TResult> BindByIndex<TSource, TResult>(
		this IAsyncEnumerable<TSource> source,
		IAsyncEnumerable<int> indices,
		Func<TSource, int, TResult> resultSelector,
		Func<int, TResult> missingSelector
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(indices);
		ArgumentNullException.ThrowIfNull(resultSelector);
		ArgumentNullException.ThrowIfNull(missingSelector);

		return Core(source, indices, resultSelector, missingSelector);

		static async IAsyncEnumerable<TResult> Core(
			IAsyncEnumerable<TSource> source,
			IAsyncEnumerable<int> indices,
			Func<TSource, int, TResult> resultSelector,
			Func<int, TResult> missingSelector,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			// keeps track of the order of indices to know what order items should be output in
			var lookup = await indices
				.Index()
				.ToDictionaryAsync(
					x =>
					{
						ArgumentOutOfRangeException.ThrowIfNegative(x.item, nameof(indices));
						return x.item;
					},
					x => x.index,
					cancellationToken
				)
				.ConfigureAwait(false);

			// keep track of items out of output order
			var lookback = new Dictionary<int, TSource>();

			// which input index are we on?
			var index = 0;
			// which output index are we on?
			var outputIndex = 0;

			// for each item in input
			await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				// does the current input index have an output?
				if (lookup.TryGetValue(index, out var oi))
				{
					// is the current item's output order the next one?
					if (oi == outputIndex)
					{
						// return the item and increment output order
						yield return resultSelector(item, outputIndex);
						outputIndex++;

						// while we're here, catch up on any lookbacks
						while (lookback.TryGetValue(outputIndex, out var e))
						{
							yield return resultSelector(e, outputIndex);
							_ = lookback.Remove(outputIndex);
							outputIndex++;
						}
					}
					// otherwise, store in lookback for later
					else
					{
						lookback[oi] = item;
					}
				}

				index++;
			}

			// catch up any remaining items
			while (outputIndex < lookup.Count)
			{
				// can we find the current output index in lookback?
				if (lookback.TryGetValue(outputIndex, out var e))
				{
					// return it
					yield return resultSelector(e, outputIndex);
					_ = lookback.Remove(outputIndex);
				}
				else
				{
					// otherwise, return a missing item
					yield return missingSelector(outputIndex);
				}

				outputIndex++;
			}
		}
	}
}
