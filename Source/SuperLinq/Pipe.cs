namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="action">Action to invoke for each element.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	public static IEnumerable<TSource> Pipe<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
	{
		source.ThrowIfNull();
		action.ThrowIfNull();

		return source.Do(action);
	}
}
