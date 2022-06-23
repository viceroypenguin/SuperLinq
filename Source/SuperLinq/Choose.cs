namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Applies a function to each element of the source sequence and
	/// returns a new sequence of result elements for source elements
	/// where the function returns a couple (2-tuple) having a <c>true</c>
	/// as its first element and result as the second.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the elements in <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">
	/// The type of the elements in the returned sequence.</typeparam>
	/// <param name="source"> The source sequence.</param>
	/// <param name="chooser">The function that is applied to each source
	/// element.</param>
	/// <returns>A sequence <typeparamref name="TResult"/> elements.</returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var str = "O,l,2,3,4,S,6,7,B,9";
	/// var xs = str.Split(',').Choose(s => (int.TryParse(s, out var n), n));
	/// ]]></code>
	/// The <c>xs</c> variable will be a sequence of the integers 2, 3, 4,
	/// 6, 7 and 9.
	/// </example>

	public static IEnumerable<TResult> Choose<T, TResult>(
		this IEnumerable<T> source,
		Func<T, (bool, TResult)> chooser)
	{
		source.ThrowIfNull();
		chooser.ThrowIfNull();

		return _(source, chooser);

		static IEnumerable<TResult> _(IEnumerable<T> source, Func<T, (bool, TResult)> chooser)
		{
			foreach (var item in source)
			{
				var (some, value) = chooser(item);
				if (some)
					yield return value;
			}
		}
	}
}
