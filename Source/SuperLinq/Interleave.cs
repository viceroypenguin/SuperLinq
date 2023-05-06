using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
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
	public static IEnumerable<T> Interleave<T>(this IEnumerable<T> source, params IEnumerable<T>[] otherSources)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(otherSources);

		foreach (var s in otherSources)
			Guard.IsNotNull(s, nameof(otherSources));

		if (source is ICollection<T> && otherSources.All(s => s is ICollection<T>))
			return new InterleaveIterator<T>(otherSources.Prepend(source).Cast<ICollection<T>>());

		return Interleave(otherSources.Prepend(source));
	}

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
	/// The sequences are interleaved in the order that they appear in the <paramref name="sources"/>
	/// collection.
	/// </remarks>
	/// <typeparam name="T">The type of the elements of the source sequences</typeparam>
	/// <param name="sources">The sequences to interleave together</param>
	/// <returns>A sequence of interleaved elements from all of the source sequences</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IEnumerable<T> Interleave<T>(this IEnumerable<IEnumerable<T>> sources)
	{
		Guard.IsNotNull(sources);

		if (sources is IEnumerable<ICollection<T>> sourcesColl)
			return new InterleaveIterator<T>(sourcesColl.ToList());

		return InterleaveCore(sources);
	}

	private static IEnumerable<T> InterleaveCore<T>(IEnumerable<IEnumerable<T>> sources)
	{
		using var list = new EnumeratorList<T>(sources);

		while (list.Any())
		{
			for (var i = 0; list.MoveNext(i); i++)
			{
				yield return list.Current(i);
			}
		}
	}

	private sealed class InterleaveIterator<T> : IteratorCollection<T, T>
	{
		private readonly IEnumerable<ICollection<T>> _sources;

		public InterleaveIterator(IEnumerable<ICollection<T>> sources)
		{
			_sources = sources;
		}

		public override int Count => _sources.Sum(static s => s.Count);

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			InterleaveCore(_sources);
	}
}
