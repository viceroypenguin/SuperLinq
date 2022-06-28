namespace Test.Async;

internal static partial class TestExtensions
{
	public static IEnumerable<T> Seq<T>(params T[] items) => items;

	public static IAsyncEnumerable<T> AsyncSeq<T>(params T[] items) =>
		items.ToAsyncEnumerable();

	public static IAsyncEnumerable<int> AsyncSeqExceptionAt(int index) =>
		AsyncSuperEnumerable.From(
			Enumerable.Range(1, index).Select(i => Func(() => Task.FromResult(i)))
				.Append(() => Task.FromException<int>(new NotSupportedException()))
				.ToArray());

	internal static async Task AssertEmpty<T>(this IAsyncEnumerable<T> actual) =>
		Assert.Empty(await actual.ToListAsync());

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, actual);

	internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, actual);

	internal static async Task AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, IEnumerable<T> expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	internal static async Task AssertSequenceEqual<T>(this IAsyncEnumerable<T> actual, params T[] expected) =>
		Assert.Equal(expected, await actual.ToListAsync());

	public static WatchableEnumerator<T> AsWatchable<T>(this IAsyncEnumerator<T> source) => new(source);
}

sealed class WatchableEnumerator<T> : IAsyncEnumerator<T>
{
	readonly IAsyncEnumerator<T> _source;

	public event EventHandler? Disposed;
	public event EventHandler<bool>? MoveNextCalled;

	public WatchableEnumerator(IAsyncEnumerator<T> source) =>
		_source = source ?? throw new ArgumentNullException(nameof(source));

	public T Current => _source.Current;

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
