namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a sequence that concatenates both given sequences, regardless of whether an error occurs.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="first">First sequence.</param>
	/// <param name="second">Second sequence.</param>
	/// <returns>Sequence concatenating the elements of both sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is <see
	/// langword="null"/>.</exception>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		return OnErrorResumeNext(new[] { first, second, });
	}

	/// <summary>
	/// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
	/// sequences.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources)
	{
		Guard.IsNotNull(sources);

		return sources.AsEnumerable().OnErrorResumeNext();
	}

	/// <summary>
	/// Creates a sequence that concatenates the given sequences, regardless of whether an error occurs in any of the
	/// sequences.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="sources">Source sequences.</param>
	/// <returns>Sequence concatenating the elements of the given sequences, ignoring errors.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
	{
		Guard.IsNotNull(sources);

		return Core(sources);

		static IEnumerable<TSource> Core(IEnumerable<IEnumerable<TSource>> sources)
		{
			foreach (var source in sources)
			{
				Guard.IsNotNull(source);
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
