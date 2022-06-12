namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Prepends a single value to a sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The sequence to prepend to.</param>
	/// <param name="value">The value to prepend.</param>
	/// <returns>
	/// Returns a sequence where a value is prepended to it.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <code><![CDATA[
	/// int[] numbers = { 1, 2, 3 };
	/// var result = numbers.Prepend(0);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// 0, 1, 2 and 3, in turn.

	public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
	{
		if (source == null) throw new ArgumentNullException(nameof(source));
		return source is PendNode<TSource> node
			 ? node.Prepend(value)
			 : PendNode<TSource>.WithSource(source).Prepend(value);
	}
}
