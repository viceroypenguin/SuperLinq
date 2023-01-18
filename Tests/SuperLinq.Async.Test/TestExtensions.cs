using CommunityToolkit.Diagnostics;
using SuperLinq;

namespace Test.Async;

internal static partial class TestExtensions
{
	public static IEnumerable<T> Seq<T>(params T[] items) => items;

	public static IAsyncEnumerable<T> AsyncSeq<T>(params T[] items) =>
		items.ToAsyncEnumerable();

	public static IAsyncEnumerable<int> AsyncSeqExceptionAt(int index) =>
		AsyncSuperEnumerable.From(
			Enumerable.Range(1, index)
				.Select(i => Func(() => Task.FromResult(i)))
				.Append(AsyncBreakingFunc.Of<int>())
				.ToArray());

	internal static async Task AssertEmpty<T>(this IAsyncEnumerable<T> actual) =>
		Assert.Empty(await actual.ToListAsync());

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, actual);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, Func<T, T, bool> comparer, params T[] expected) =>
		Assert.Equal(expected, actual, EqualityComparer.Create(comparer));

	internal static async Task AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, IAsyncEnumerable<T> expected) =>
		Assert.Equal(await expected.ToListAsync(), await actual.ToListAsync());

	internal static async Task AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	internal static async Task AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	internal static async Task AssertCollectionEqual<T>(this IAsyncEnumerable<T> actual, params T[] expected) =>
		Assert.True(await actual.CollectionEqual(expected.ToAsyncEnumerable(), comparer: default));

	internal static async Task AssertCollectionEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.True(await actual.CollectionEqual(expected.ToAsyncEnumerable(), comparer: default));

	internal static async Task AssertCollectionEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T>? comparer) =>
		Assert.True(await actual.CollectionEqual(expected.ToAsyncEnumerable(), comparer));

	public static WatchableEnumerator<T> AsWatchable<T>(this IAsyncEnumerator<T> source) => new(source);
}

internal sealed class WatchableEnumerator<T> : IAsyncEnumerator<T>
{
	private readonly IAsyncEnumerator<T> _source;

	public event EventHandler? Disposed;
	public event EventHandler? GetCurrentCalled;
	public event EventHandler<bool>? MoveNextCalled;

	public WatchableEnumerator(IAsyncEnumerator<T> source)
	{
		Guard.IsNotNull(source);
		_source = source;
	}

	public T Current
	{
		get
		{
			GetCurrentCalled?.Invoke(this, EventArgs.Empty);
			return _source.Current;
		}
	}

	public async ValueTask<bool> MoveNextAsync()
	{
		var moved = await _source.MoveNextAsync();
		MoveNextCalled?.Invoke(this, moved);
		return moved;
	}

	public async ValueTask DisposeAsync()
	{
		await _source.DisposeAsync();
		Disposed?.Invoke(this, EventArgs.Empty);
	}
}
