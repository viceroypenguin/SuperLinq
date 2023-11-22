namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Transposes a sequence of rows into a sequence of columns.
	/// </summary>
	/// <typeparam name="T">
	///	    Type of source sequence elements.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence to transpose.
	/// </param>
	/// <returns>
	///	    Returns a sequence of columns in the source swapped into rows.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    If a rows is shorter than a follow it then the shorter row's elements are skipped in the corresponding
	///     column sequences. This operator uses deferred execution and streams its results. Source sequence is consumed
	///     greedily when an iteration begins. The inner sequences representing rows are consumed lazily and resulting
	///     sequences of columns are streamed.
	/// </remarks>
	public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return Core(source);

		static IEnumerable<IEnumerable<T>> Core(IEnumerable<IEnumerable<T>> source)
		{
			using var list = new EnumeratorList<T>(source);

			while (true)
			{
				var column = new T[list.Count];
				var count = 0;
				for (; list.MoveNext(count); count++)
				{
					column[count] = list.Current(count);
				}

				if (count == 0)
					yield break;

				yield return new ArraySegment<T>(column, 0, count);
			}
		}
	}
}
