namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates an enumerable sequence based on an enumerable factory function.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="enumerableFactory">Enumerable factory function.</param>
	/// <returns>Sequence that will invoke the enumerable factory upon iteration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="enumerableFactory"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TResult> Defer<TResult>(Func<IAsyncEnumerable<TResult>> enumerableFactory)
	{
		ArgumentNullException.ThrowIfNull(enumerableFactory);

		return Core(enumerableFactory);

		static async IAsyncEnumerable<TResult> Core(
			Func<IAsyncEnumerable<TResult>> enumerableFactory,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var el in enumerableFactory().WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return el;
		}
	}
}
