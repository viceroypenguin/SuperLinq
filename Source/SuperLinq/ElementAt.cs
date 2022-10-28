// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>Returns the element at a specified index in a sequence.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
	/// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence.</exception>
	/// <returns>The element at the specified position in the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>If the type of <paramref name="source" /> implements <see cref="IList{T}" />, that implementation is used to obtain the element at the specified index. Otherwise, this method obtains the specified element.</para>
	/// <para>This method throws an exception if <paramref name="index" /> is out of range. To instead return a default value when the specified index is out of range, use the <see cref="SuperEnumerable.ElementAtOrDefault" /> method.</para>
	/// </remarks>
#if !NET6_0_OR_GREATER
	public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, Index index)
#else
	[Obsolete("This method has been implemented by the framework.")]
	public static TSource ElementAt<TSource>(IEnumerable<TSource> source, Index index)
#endif
	{
		Guard.IsNotNull(source);

		if (!index.IsFromEnd)
		{
			return Enumerable.ElementAt(source, index.Value);
		}

		if (source is ICollection<TSource> c)
		{
			return Enumerable.ElementAt(source, c.Count - index.Value);
		}

		if (!TryGetElementFromEnd(source, index.Value, out var element))
		{
			ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
		}

		return element;
	}

	/// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
	/// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
	/// <returns><see langword="default" /> if <paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence; otherwise, the element at the specified position in the <paramref name="source" /> sequence.</returns>
	/// <remarks>
	/// <para>If the type of <paramref name="source" /> implements <see cref="IList{T}" />, that implementation is used to obtain the element at the specified index. Otherwise, this method obtains the specified element.</para>
	/// <para>The default value for reference and nullable types is <see langword="null" />.</para>
	/// </remarks>
#if !NET6_0_OR_GREATER
	public static TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, Index index)
#else
	[Obsolete("This method has been implemented by the framework.")]
	public static TSource? ElementAtOrDefault<TSource>(IEnumerable<TSource> source, Index index)
#endif
	{
		Guard.IsNotNull(source);

		if (!index.IsFromEnd)
		{
			return Enumerable.ElementAt(source, index.Value);
		}

		if (source.TryGetCollectionCount(out var count))
		{
			return Enumerable.ElementAt(source, count - index.Value);
		}

		TryGetElementFromEnd(source, index.Value, out var element);
		return element;
	}

	private static bool TryGetElementFromEnd<TSource>(IEnumerable<TSource> source, int indexFromEnd, [MaybeNullWhen(false)] out TSource element)
	{
		if (indexFromEnd > 0)
		{
			using var e = source.GetEnumerator();
			if (e.MoveNext())
			{
				Queue<TSource> queue = new();
				queue.Enqueue(e.Current);
				while (e.MoveNext())
				{
					if (queue.Count == indexFromEnd)
					{
						queue.Dequeue();
					}

					queue.Enqueue(e.Current);
				}

				if (queue.Count == indexFromEnd)
				{
					element = queue.Dequeue();
					return true;
				}
			}
		}

		element = default;
		return false;
	}
}
