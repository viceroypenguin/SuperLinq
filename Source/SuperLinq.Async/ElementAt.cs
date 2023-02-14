// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>Returns the element at a specified index in a sequence.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">An <see cref="IAsyncEnumerable{T}" /> to return an element from.</param>
	/// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence.</exception>
	/// <returns>The element at the specified position in the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>
	/// This method throws an exception if <paramref name="index" /> is out of range. 
	/// To instead return a default value when the specified index is out of range, 
	/// use the <see cref="ElementAtOrDefaultAsync{TSource}(IAsyncEnumerable{TSource}, Index, CancellationToken)" /> method.</para>
	/// </remarks>
	public static async ValueTask<TSource> ElementAtAsync<TSource>(this IAsyncEnumerable<TSource> source, Index index, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);

		if (!index.IsFromEnd)
		{
			return await AsyncEnumerable.ElementAtAsync(source, index.Value, cancellationToken).ConfigureAwait(false);
		}

		var (success, element) = await TryGetElementFromEnd(source, index.Value, cancellationToken).ConfigureAwait(false);
		if (!success)
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
		}

		return Debug.AssertNotNull(element);
	}

	/// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">An <see cref="IAsyncEnumerable{T}" /> to return an element from.</param>
	/// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <returns><see langword="default" /> if <paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence; otherwise, the element at the specified position in the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>The default value for reference and nullable types is <see langword="null" />.</para>
	/// </remarks>
	public static async ValueTask<TSource?> ElementAtOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Index index, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);

		if (!index.IsFromEnd)
		{
			return await AsyncEnumerable.ElementAtOrDefaultAsync(source, index.Value, cancellationToken).ConfigureAwait(false);
		}

		var (_, element) = await TryGetElementFromEnd(source, index.Value, cancellationToken).ConfigureAwait(false);
		return element;
	}

	private static async ValueTask<(bool success, TSource? element)> TryGetElementFromEnd<TSource>(IAsyncEnumerable<TSource> source, int indexFromEnd, CancellationToken cancellationToken)
	{
		if (indexFromEnd > 0)
		{
			await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);
			if (await e.MoveNextAsync())
			{
				Queue<TSource> queue = new();
				queue.Enqueue(e.Current);
				while (await e.MoveNextAsync())
				{
					if (queue.Count == indexFromEnd)
					{
						_ = queue.Dequeue();
					}

					queue.Enqueue(e.Current);
				}

				if (queue.Count == indexFromEnd)
				{
					return (true, queue.Dequeue());
				}
			}
		}

		return (false, default);
	}
}
