namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Immediately executes the given action on each element in the source sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask ForEach<TSource>(
		this IAsyncEnumerable<TSource> source,
		Action<TSource> action,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(action);

		return Core(source, action, cancellationToken);

		static async ValueTask Core(
			IAsyncEnumerable<TSource> source,
			Action<TSource> action,
			CancellationToken cancellationToken)
		{
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				action(element);
		}
	}

	/// <summary>
	/// Immediately executes the given action on each element in the source sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask ForEach<TSource>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, ValueTask> action,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(action);

		return Core(source, action, cancellationToken);

		static async ValueTask Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, ValueTask> action,
			CancellationToken cancellationToken)
		{
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				await action(element).ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Immediately executes the given action on each element in the source sequence. Each element's index is used in
	/// the logic of the action.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element; the second parameter of the action represents the
	/// index of the source element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask ForEach<TSource>(
		this IAsyncEnumerable<TSource> source,
		Action<TSource, int> action,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(action);

		return Core(source, action, cancellationToken);

		static async ValueTask Core(
			IAsyncEnumerable<TSource> source,
			Action<TSource, int> action,
			CancellationToken cancellationToken)
		{
			var index = 0;
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				action(element, index++);
		}
	}

	/// <summary>
	/// Immediately executes the given action on each element in the source sequence. Each element's index is used in
	/// the logic of the action.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
	/// <param name="source">The sequence of elements</param>
	/// <param name="action">The action to execute on each element; the second parameter of the action represents the
	/// index of the source element.</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="action"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>
	public static ValueTask ForEach<TSource>(
		this IAsyncEnumerable<TSource> source,
		Func<TSource, int, ValueTask> action,
		CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(action);

		return Core(source, action, cancellationToken);

		static async ValueTask Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, int, ValueTask> action,
			CancellationToken cancellationToken)
		{
			var index = 0;
			await foreach (var element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				await action(element, index++).ConfigureAwait(false);
		}
	}
}
