namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a sequence that concatenates both given sequences, regardless of whether an error occurs.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="first">
	///	    First sequence.
	/// </param>
	/// <param name="second">
	///	    Second sequence.
	/// </param>
	/// <returns>
	///	    Sequence concatenating the elements of both sequences, ignoring errors.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="first"/> is enumerated until either the sequence completes or an error occurs during
	///     enumeration. After either of these events, <paramref name="second"/> is then enumerated in the same way.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		return OnErrorResumeNext(new[] { first, second, });
	}

	/// <summary>
	///	    Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of
	///     the sequences.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="sources">
	///	    Source sequences.
	/// </param>
	/// <returns>
	///	    Sequence concatenating the elements of the given sequences, ignoring errors.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    Each sequence of <paramref name="sources"/> is enumerated until either the sequence completes or an error occurs during
	///     enumeration. The returned sequence completes when all sub-sequences have been enumerated in this manner.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return sources.AsEnumerable().OnErrorResumeNext();
	}

	/// <summary>
	///	    Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of
	///     the sequences.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="sources">
	///	    Source sequences.
	/// </param>
	/// <returns>
	///	    Sequence concatenating the elements of the given sequences, ignoring errors.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    Each sequence of <paramref name="sources"/> is enumerated until either the sequence completes or an error occurs during
	///     enumeration. The returned sequence completes when all sub-sequences have been enumerated in this manner.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
	{
		ArgumentNullException.ThrowIfNull(sources);

		return Core(sources);

		static IEnumerable<TSource> Core(IEnumerable<IEnumerable<TSource>> sources)
		{
			foreach (var source in sources)
			{
				ArgumentNullException.ThrowIfNull(source);
				using var e = source.GetEnumerator();

				while (true)
				{
#pragma warning disable CA1031 // Do not catch general exception types
					try
					{
						if (!e.MoveNext())
							break;
					}
					catch
					{
						break;
					}
#pragma warning restore CA1031 // Do not catch general exception types

					yield return e.Current;
				}
			}
		}
	}
}
