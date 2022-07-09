namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Interleaves the elements of two or more sequences into a single sequence, skipping sequences as they are consumed
	/// </summary>
	/// <remarks>
	/// Interleave combines sequences by visiting each in turn, and returning the first element of each, followed
	/// by the second, then the third, and so on. So, for example:<br/>
	/// <code><![CDATA[
	/// {1,1,1}.Interleave( {2,2,2}, {3,3,3} ) => { 1,2,3,1,2,3,1,2,3 }
	/// ]]></code>
	/// This operator behaves in a deferred and streaming manner.<br/>
	/// When sequences are of unequal length, this method will skip those sequences that have been fully consumed
	/// and continue interleaving the remaining sequences.<br/>
	/// The sequences are interleaved in the order that they appear in the <paramref name="otherSources"/>
	/// collection, with <paramref name="source"/> as the first sequence.
	/// </remarks>
	/// <typeparam name="T">The type of the elements of the source sequences</typeparam>
	/// <param name="source">The first sequence in the interleave group</param>
	/// <param name="otherSources">The other sequences in the interleave group</param>
	/// <returns>A sequence of interleaved elements from all of the source sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="otherSources"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException">Any of the items in <paramref name="otherSources"/> is <see langword="null"/>.</exception>
	public static IAsyncEnumerable<T> Interleave<T>(this IAsyncEnumerable<T> source, params IAsyncEnumerable<T>[] otherSources)
	{
		source.ThrowIfNull();
		otherSources.ThrowIfNull();
		if (otherSources.Any(s => s == null))
			throw new ArgumentNullException(nameof(otherSources), "One or more sequences passed to Interleave was null.");

		return _(otherSources.Prepend(source));

		static async IAsyncEnumerable<T> _(IEnumerable<IAsyncEnumerable<T>> sources, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var list = await EnumeratorList<T>.Create(sources, cancellationToken).ConfigureAwait(false);
			await using var ignored_ = list.ConfigureAwait(false);

			while (list.Any())
			{
				for (var i = 0; await list.MoveNext(i).ConfigureAwait(false); i++)
				{
					yield return list.Current(i);
				}
			}
		}
	}
}
