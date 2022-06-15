namespace Test;

public enum SourceKind
{
	Sequence,
	BreakingList,
	BreakingReadOnlyList,
	BreakingCollection,
	BreakingReadOnlyCollection
}

static partial class TestExtensions
{
	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, actual);

	internal static async ValueTask AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	internal static async ValueTask AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, await actual.ToListAsync());
}
