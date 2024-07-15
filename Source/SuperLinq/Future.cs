// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NETCOREAPP

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

internal static class Future
{
	public static bool TryDequeue<T>(this Queue<T> queue, out T result)
	{
		if (queue.Count == 0)
		{
			result = default!;
			return false;
		}

		result = queue.Dequeue();
		return !EqualityComparer<T>.Default.Equals(result, default!);
	}

	public static bool TryGetValue<T>(this SortedSet<T> set, T equalValue, [MaybeNullWhen(false)] out T actualValue)
	{
		var index = set.FindIndex(x => set.Comparer.Compare(equalValue, x) == 0);
		if (index == -1)
		{
			actualValue = default;
			return false;
		}

		actualValue = set.ElementAt(index);
		return true;
	}

	public static HashSet<TSource> ToHashSet<TSource>(
		this IEnumerable<TSource> source,
		IEqualityComparer<TSource>? comparer)
	{
		ArgumentNullException.ThrowIfNull(source);
		return new HashSet<TSource>(source, comparer);
	}
}

#endif
