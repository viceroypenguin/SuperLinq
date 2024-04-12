namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Skips items from the input sequence until the given predicate returns true
	/// when applied to the current source item; that item will be the last skipped.
	/// </summary>
	/// <remarks>
	/// <para>
	/// SkipUntil differs from Enumerable.SkipWhile in two respects. Firstly, the sense
	/// of the predicate is reversed: it is expected that the predicate will return false
	/// to start with, and then return true - for example, when trying to find a matching
	/// item in a sequence.
	/// </para>
	/// <para>
	/// Secondly, SkipUntil skips the element which causes the predicate to return true. For
	/// example, in a sequence <code><![CDATA[{ 1, 2, 3, 4, 5 }]]></code> and with a predicate of
	/// <code><![CDATA[x => x == 3]]></code>, the result would be <code><![CDATA[{ 4, 5 }]]></code>.
	/// </para>
	/// <para>
	/// SkipUntil is as lazy as possible: it will not iterate over the source sequence
	/// until it has to, it won't iterate further than it has to, and it won't evaluate
	/// the predicate until it has to. (This means that an item may be returned which would
	/// actually cause the predicate to throw an exception if it were evaluated, so long as
	/// it comes after the first item causing the predicate to return true.)
	/// </para>
	/// </remarks>
	/// <typeparam name="TSource">Type of the source sequence</typeparam>
	/// <param name="source">Source sequence</param>
	/// <param name="predicate">Predicate used to determine when to stop yielding results from the source.</param>
	/// <returns>Items from the source sequence after the predicate first returns true when applied to the item.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null</exception>

	public static IAsyncEnumerable<TSource> SkipUntil<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);

		return Core(source, predicate);

		static async IAsyncEnumerable<TSource> Core(
			IAsyncEnumerable<TSource> source,
			Func<TSource, bool> predicate,
			[EnumeratorCancellation] CancellationToken cancellationToken = default
		)
		{
			await using var enumerator = source.GetConfiguredAsyncEnumerator(cancellationToken);

			do
			{
				if (!await enumerator.MoveNextAsync())
					yield break;
			}
			while (!predicate(enumerator.Current));

			while (await enumerator.MoveNextAsync())
				yield return enumerator.Current;
		}
	}
}
