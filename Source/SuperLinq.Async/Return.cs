namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a single-element sequence containing the item provided.
	/// </summary>
	/// <typeparam name="T">The type of the item.</typeparam>
	/// <param name="item">The item to return in a sequence.</param>
	/// <returns>A sequence containing only <paramref name="item"/>.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public static async IAsyncEnumerable<T> Return<T>(T item)
	{
		yield return item;
	}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
