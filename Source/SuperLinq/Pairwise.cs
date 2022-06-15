namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence resulting from applying a function to each
	/// element in the source sequence and its
	/// predecessor, with the exception of the first element which is
	/// only returned as the predecessor of the second element.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">A transform function to apply to
	/// each pair of sequence.</param>
	/// <returns>
	/// Returns the resulting sequence.
	/// </returns>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var source = new[] { "a", "b", "c", "d" };
	/// var result = source.Pairwise((a, b) => a + b);
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// "ab", "bc" and "cd", in turn.
	/// </example>

	public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
	{
		source.ThrowIfNull();
		resultSelector.ThrowIfNull();

		return _(); IEnumerable<TResult> _()
		{
			using var e = source.GetEnumerator();

			if (!e.MoveNext())
				yield break;

			var previous = e.Current;
			while (e.MoveNext())
			{
				yield return resultSelector(previous, e.Current);
				previous = e.Current;
			}
		}
	}
}
