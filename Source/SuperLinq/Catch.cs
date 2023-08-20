namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a sequence that corresponds to the source sequence, concatenating it with the sequence resulting
	///     from calling an exception handler function in case of an error.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <typeparam name="TException">
	///	    Exception type to catch.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="handler">
	///	    Handler to invoke when an exception of the specified type occurs.
	/// </param>
	/// <returns>
	///	    Source sequence, concatenated with an exception handler result sequence in case of an error.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="handler"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) The sequence <c>errSource</c> returned by <paramref name="handler"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Catch<TSource, TException>(
		this IEnumerable<TSource> source,
		Func<TException, IEnumerable<TSource>> handler)
		where TException : Exception
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(handler);

		return Core(source, handler);

		static IEnumerable<TSource> Core(
			IEnumerable<TSource> source,
			Func<TException, IEnumerable<TSource>> handler)
		{
			IEnumerable<TSource>? errSource;
			using var e = source.GetEnumerator();
			while (true)
			{
				try
				{
					if (!e.MoveNext())
						yield break;
				}
				catch (TException ex)
				{
					errSource = handler(ex);
					break;
				}

				yield return e.Current;
			}

			Assert.NotNull(errSource);

			foreach (var item in errSource)
				yield return item;
		}
	}

	/// <summary>
	///	    Creates a sequence that returns the elements of the first sequence, switching to the second in case of an
	///     error.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="first">
	///	    First sequence.
	/// </param>
	/// <param name="second">
	///	    Second sequence, concatenated to the result in case the first sequence completes exceptionally.
	/// </param>
	/// <returns>
	///	    The first sequence, followed by the second sequence in case an error is produced.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="first"/> or <paramref name="second"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///		This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Catch<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		return Catch(new[] { first, second, });
	}

	/// <summary>
	///	    Creates a sequence by concatenating source sequences until a source sequence completes successfully.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="sources">
	///	    Source sequences.
	/// </param>
	/// <returns>
	///	    Sequence that continues to concatenate source sequences while errors occur.
	///	</returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) Any sequence <c>source</c> returned by <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Catch<TSource>(params IEnumerable<TSource>[] sources)
	{
		Guard.IsNotNull(sources);

		return sources.AsEnumerable().Catch();
	}

	/// <summary>
	///	    Creates a sequence by concatenating source sequences until a source sequence completes successfully.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="sources">
	///	    Source sequences.
	/// </param>
	/// <returns>
	///	    Sequence that continues to concatenate source sequences while errors occur.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) Any sequence <c>source</c> returned by <paramref name="sources"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Catch<TSource>(this IEnumerable<IEnumerable<TSource>> sources)
	{
		Guard.IsNotNull(sources);

		return Core(sources);

		static IEnumerable<TSource> Core(IEnumerable<IEnumerable<TSource>> sources)
		{
			using var sourceIter = sources.GetEnumerator();

			if (!sourceIter.MoveNext())
				yield break;

			var source = sourceIter.Current;
			var hasNext = sourceIter.MoveNext();

			// outer loop is not infinite.
			// on last loop (`hasNext == false`), then either
			// `source` will iterate successfully (yield break)
			// or it will fail (throw). either way, it will not
			// make it outside of the inner `while (true)`
			while (true)
			{
				Guard.IsNotNull(source);
				using var e = source.GetEnumerator();

				while (true)
				{
					try
					{
						if (!e.MoveNext())
							yield break;
					}
					catch
					{
						if (!hasNext)
							throw;

						break;
					}

					yield return e.Current;
				}

				source = sourceIter.Current;
				hasNext = sourceIter.MoveNext();
			}
		}
	}
}
