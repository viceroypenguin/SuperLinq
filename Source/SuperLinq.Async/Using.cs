namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Generates a sequence that's dependent on a resource object whose lifetime is determined by the sequence usage
	/// duration.
	/// </summary>
	/// <typeparam name="TSource">Source element type.</typeparam>
	/// <typeparam name="TResource">Resource type.</typeparam>
	/// <param name="resourceFactory">Resource factory function.</param>
	/// <param name="enumerableFactory">Enumerable factory function, having access to the obtained resource.</param>
	/// <returns>Sequence whose use controls the lifetime of the associated obtained resource.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="resourceFactory"/> or <paramref
	/// name="enumerableFactory"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="resourceFactory"/> and <paramref name="enumerableFactory"/> are evaluated lazily, once
	/// enumeration has begun. The value returned by <paramref name="resourceFactory"/> will be disposed after the
	/// enumeration has completed. 
	/// </para>
	/// <para>
	/// The values returned by <paramref name="enumerableFactory"/> and <paramref name="enumerableFactory"/> are not
	/// cached; multiple iterations of the <see cref="IAsyncEnumerable{T}"/> returned by this method will call these methods
	/// separately for each iteration.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Using<TSource, TResource>(
		Func<TResource> resourceFactory,
		Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory)
		where TResource : IAsyncDisposable
	{
		Guard.IsNotNull(resourceFactory);
		Guard.IsNotNull(enumerableFactory);

		return Core(resourceFactory, enumerableFactory);

		static async IAsyncEnumerable<TSource> Core(
			Func<TResource> resourceFactory,
			Func<TResource, IAsyncEnumerable<TSource>> enumerableFactory,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var resource = resourceFactory();
			await using (resource.ConfigureAwait(false))
			{
				await foreach (var item in enumerableFactory(resource).WithCancellation(cancellationToken).ConfigureAwait(false))
					yield return item;
			}
		}
	}
}
