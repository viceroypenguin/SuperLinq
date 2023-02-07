namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Completely consumes the given sequence. This method uses immediate execution,
	/// and doesn't store any data during execution.
	/// </summary>
	/// <typeparam name="T">Element type of the sequence</typeparam>
	/// <param name="source">Source to consume</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	public static ValueTask Consume<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		return Core(source, cancellationToken);

		static async ValueTask Core(IAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			await foreach (var _ in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}
