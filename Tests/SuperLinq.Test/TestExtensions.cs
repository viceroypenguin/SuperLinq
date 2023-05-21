﻿using CommunityToolkit.Diagnostics;

namespace Test;

public enum SourceKind
{
	Sequence,
	BreakingCollection,
}

internal static partial class TestExtensions
{
	internal static IEnumerable<T> Seq<T>(params T[] values) => values;

	public static IEnumerable<int> SeqExceptionAt(int index) =>
		SuperEnumerable.From(
			Enumerable.Range(1, index - 1)
				.Select(i => Func(() => i))
				.Append(BreakingFunc.Of<int>())
				.ToArray());

	/// <summary>
	/// Just to make our testing easier so we can chain the assertion call.
	/// </summary>
	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	/// <summary>
	/// Make testing even easier - a params array makes for readable tests :)
	/// The sequence should be evaluated exactly once.
	/// </summary>
	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected)
	{
		if (actual is ICollection<T>)
		{
			var arr = new T[expected.Length];
			var cnt = SuperEnumerable.CopyTo(actual, arr);
			Assert.Equal(expected.Length, cnt);
			Assert.Equal(expected, arr);
		}
		else
		{
			Assert.Equal(expected, actual);
		}
	}

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, Func<T, T, bool> comparer, params T[] expected) =>
		Assert.Equal(expected, actual, EqualityComparer.Create(comparer));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.True(actual.CollectionEqual(expected, comparer: default));

	internal static void AssertCollectionEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer) =>
		Assert.True(actual.CollectionEqual(expected, comparer));

	internal static IEnumerable<IDisposableEnumerable<T>> GetCollectionSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence(maxEnumerations: 2);
		yield return new BreakingCollection<T>(input);
	}

	internal static IEnumerable<IDisposableEnumerable<T>> GetListSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence(maxEnumerations: 2);
		yield return new BreakingList<T>(input);
	}

	internal static IEnumerable<IDisposableEnumerable<T>> GetAllSequences<T>(this IEnumerable<T> input)
	{
		// UI will consume one enumeration
		yield return input.AsTestingSequence(maxEnumerations: 2);
		yield return new BreakingCollection<T>(input);
		yield return new BreakingList<T>(input);
	}

	internal static IDisposableEnumerable<T> ToSourceKind<T>(this IList<T> input, SourceKind sourceKind) =>
		sourceKind switch
		{
			SourceKind.Sequence => input.AsTestingSequence(),
			SourceKind.BreakingCollection => new BreakingCollection<T>(input),
			_ => ThrowHelper.ThrowArgumentException<IDisposableEnumerable<T>>(nameof(sourceKind)),
		};
}
