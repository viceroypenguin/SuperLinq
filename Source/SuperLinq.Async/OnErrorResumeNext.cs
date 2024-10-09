namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Creates a sequence that concatenates both given sequences, regardless of whether an error occurs.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="first">First sequence.</param>
	/// <param name="second">Second sequence.</param>
	/// <returns>Sequence concatenating the elements of both sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see
	/// langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		return OnErrorResumeNext([first, second]);
	}

	/// <summary>
	/// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
	/// sequences.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(params IAsyncEnumerable<TSource>[] sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return sources.ToAsyncEnumerable().OnErrorResumeNext();
	}

	/// <summary>
	/// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
	/// sequences.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return sources.ToAsyncEnumerable().OnErrorResumeNext();
	}

	/// <summary>
	/// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
	/// sequences.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<TSource> OnErrorResumeNext<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return Core(sources);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<IAsyncEnumerable<TSource>> sources,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await foreach (var source in sources.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				ArgumentNullException.ThrowIfNull(source);
				await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken);

				while (true)
				{
#pragma warning disable CA1031 // Do not catch general exception types
					try
					{
						if (!await e.MoveNextAsync())
							break;
					}
					catch
					{
						break;
					}
#pragma warning restore CA1031 // Do not catch general exception types

					yield return e.Current;
				}
			}
		}
	}
}
