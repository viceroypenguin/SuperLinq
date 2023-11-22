namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Generates a sequence that's dependent on a resource object whose lifetime is determined by the sequence
	///     usage duration.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source element type.
	/// </typeparam>
	/// <typeparam name="TResource">
	///	    Resource type.
	/// </typeparam>
	/// <param name="resourceFactory">
	///	    Resource factory function.
	/// </param>
	/// <param name="enumerableFactory">
	///	    Enumerable factory function, having access to the obtained resource.
	/// </param>
	/// <returns>
	///	    Sequence whose use controls the lifetime of the associated obtained resource.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="resourceFactory"/> or <paramref name="enumerableFactory"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="resourceFactory"/> and <paramref name="enumerableFactory"/> are evaluated lazily, once
	///     enumeration has begun. The value returned by <paramref name="resourceFactory"/> will be disposed after the
	///     enumeration has completed. 
	/// </para>
	/// <para>
	///	    The values returned by <paramref name="enumerableFactory"/> and <paramref name="enumerableFactory"/> are not
	///     cached; multiple iterations of the <see cref="IEnumerable{T}"/> returned by this method will call these
	///     methods separately for each iteration.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Using<TSource, TResource>(
		Func<TResource> resourceFactory,
		Func<TResource, IEnumerable<TSource>> enumerableFactory)
		where TResource : IDisposable
	{
		Guard.IsNotNull(resourceFactory);
		Guard.IsNotNull(enumerableFactory);

		return Core(resourceFactory, enumerableFactory);

		static IEnumerable<TSource> Core(
			Func<TResource> resourceFactory,
			Func<TResource, IEnumerable<TSource>> enumerableFactory)
		{
			using var resource = resourceFactory();
			foreach (var item in enumerableFactory(resource))
				yield return item;
		}
	}
}
