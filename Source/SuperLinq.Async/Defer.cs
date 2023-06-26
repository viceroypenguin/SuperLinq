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
		Guard.IsNotNull(enumerableFactory);

		return new DeferEnumerable<TResult>(enumerableFactory);
	}

	private sealed class DeferEnumerable<T> : IAsyncEnumerable<T>
	{
		private readonly Func<IAsyncEnumerable<T>> _factory;

		public DeferEnumerable(Func<IAsyncEnumerable<T>> factory)
		{
			_factory = factory;
		}

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken) =>
			_factory().GetAsyncEnumerator(cancellationToken);
	}
}
