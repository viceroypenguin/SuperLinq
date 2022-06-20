namespace Test.Async;

public enum SourceKind
{
	Sequence,
	BreakingList,
	BreakingReadOnlyList,
	BreakingCollection,
	BreakingReadOnlyCollection,
}

internal static partial class TestExtensions
{
	public static IAsyncEnumerable<T> AsyncSeq<T>(params T[] items) =>
		items.ToAsyncEnumerable();

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, actual);

	internal static async ValueTask AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	internal static async ValueTask AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, await actual.ToListAsync());
}
