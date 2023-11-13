namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Skips items from the input sequence until the given predicate returns true when applied to the current
	///     source item; that item will be the last skipped.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of the source sequence
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence
	/// </param>
	/// <param name="predicate">
	///	    Predicate used to determine when to stop yielding results from the source.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>
	/// </exception>
	/// <returns>
	///	    Items from the source sequence after the predicate first returns true when applied to the item.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    SkipUntil differs from <see cref="Enumerable.SkipWhile{TSource}(IEnumerable{TSource}, Func{TSource,
	///     bool})"/> in two respects.
	/// </para>
	/// <para>
	///	    Firstly, the sense of the predicate is reversed: it is expected that the predicate will return <see
	///     langword="false"/> to start with, and then return <see langword="true"/> - for example, when trying to find
	///     a matching item in a sequence.
	/// </para>
	/// <para>
	///	    Secondly, SkipUntil skips the element which causes the predicate to return <see langword="true"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	///	</para>
	/// </remarks>
	public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);

		return Core(source, predicate);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			using var enumerator = source.GetEnumerator();

			do
			{
				if (!enumerator.MoveNext())
					yield break;
			}
			while (!predicate(enumerator.Current));

			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
	}
}
