namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Takes items from the input sequence until the given predicate returns <see langword="true"/> when applied to
	///     the current source item; that item will be the last taken.
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
	///	    Items from the source sequence until the <paramref name="predicate"/> first returns <see langword="true"/>
	///     when applied to the item.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    TakeUntil differs from <see cref="Enumerable.TakeWhile{TSource}(IEnumerable{TSource}, Func{TSource,
	///     bool})"/> in two respects.
	/// </para>
	/// <para>
	///	    Firstly, the sense of the predicate is reversed: it is expected that the predicate will return <see
	///     langword="false"/> to start with, and then return <see langword="true"/> - for example, when trying to find
	///     a matching item in a sequence.
	/// </para>
	/// <para>
	///	    Secondly, TakeUntil returns the element which causes the predicate to return <see langword="true"/>.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	///	</para>
	/// </remarks>
	public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);

		return Core(source, predicate);

		static IEnumerable<TSource> Core(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (var item in source)
			{
				yield return item;
				if (predicate(item))
					yield break;
			}
		}
	}
}
