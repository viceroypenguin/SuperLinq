// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NETCOREAPP

using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

[ExcludeFromCodeCoverage]
internal static class Future
{
	public static bool TryGetValue<T>(this SortedSet<T> set, T equalValue, [MaybeNullWhen(false)] out T actualValue)
	{
		foreach (var x in set)
		{
			if (set.Comparer.Compare(x, equalValue) == 0)
			{
				actualValue = x;
				return true;
			}
		}

		actualValue = default;
		return false;
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
