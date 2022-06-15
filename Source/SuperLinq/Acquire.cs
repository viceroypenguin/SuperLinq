namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Ensures that a source sequence of <see cref="IDisposable"/>
	/// objects are all acquired successfully. If the acquisition of any
	/// one <see cref="IDisposable"/> fails then those successfully
	/// acquired till that point are disposed.
	/// </summary>
	/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
	/// <param name="source">Source sequence of <see cref="IDisposable"/> objects.</param>
	/// <returns>
	/// Returns an array of all the acquired <see cref="IDisposable"/>
	/// objects in source order.
	/// </returns>
	/// <remarks>
	/// This operator executes immediately.
	/// </remarks>

	public static TSource[] Acquire<TSource>(this IEnumerable<TSource> source)
		where TSource : IDisposable
	{
		source.ThrowIfNull();

		var disposables = new List<TSource>();
		try
		{
			disposables.AddRange(source);
			return disposables.ToArray();
		}
		catch
		{
			foreach (var disposable in disposables)
				disposable.Dispose();
			throw;
		}
	}
}
