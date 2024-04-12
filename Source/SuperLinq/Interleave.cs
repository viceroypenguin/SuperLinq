using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Interleaves the elements of two or more sequences into a single sequence in a round-robin fashion until all
	///     sequences are consumed.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of the source sequences
	/// </typeparam>
	/// <param name="source">
	///	    The first sequence in the interleave group
	/// </param>
	/// <param name="otherSources">
	///	    The other sequences in the interleave group
	/// </param>
	/// <returns>
	///	    A sequence of interleaved elements from all of the source sequences
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="otherSources"/>, or any of the sequences in <paramref
	///     name="otherSources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> Interleave<T>(this IEnumerable<T> source, params IEnumerable<T>[] otherSources)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(otherSources);

		foreach (var s in otherSources)
			ArgumentNullException.ThrowIfNull(s, nameof(otherSources));

		if (source is ICollection<T> && otherSources.All(s => s is ICollection<T>))
			return new InterleaveIterator<T>(otherSources.Prepend(source).Cast<ICollection<T>>());

		return Interleave(otherSources.Prepend(source));
	}

	/// <summary>
	///	    Interleaves the elements of two or more sequences into a single sequence in a round-robin fashion until all
	///     sequences are consumed.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements of the source sequences
	/// </typeparam>
	/// <param name="sources">
	///	    The sequences to interleave together
	/// </param>
	/// <returns>
	///	    A sequence of interleaved elements from all of the source sequences
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> Interleave<T>(this IEnumerable<IEnumerable<T>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

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
				yield return list.Current(i);
		}
	}

	private sealed class InterleaveIterator<T>(
		IEnumerable<ICollection<T>> sources
	) : CollectionIterator<T>
	{
		public override int Count => sources.Sum(static s => s.Count);

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			InterleaveCore(sources);
	}
}
