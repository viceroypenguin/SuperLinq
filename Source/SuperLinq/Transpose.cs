namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Transposes a sequence of rows into a sequence of columns.
	/// </summary>
	/// <typeparam name="T">Type of source sequence elements.</typeparam>
	/// <param name="source">Source sequence to transpose.</param>
	/// <returns>
	/// Returns a sequence of columns in the source swapped into rows.
	/// </returns>
	/// <remarks>
	/// If a rows is shorter than a follow it then the shorter row's
	/// elements are skipped in the corresponding column sequences.
	/// This operator uses deferred execution and streams its results.
	/// Source sequence is consumed greedily when an iteration begins.
	/// The inner sequences representing rows are consumed lazily and
	/// resulting sequences of columns are streamed.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var matrix = new[]
	/// {
	///     new[] { 10, 11 },
	///     new[] { 20 },
	///     new[] { 30, 31, 32 }
	/// };
	/// var result = matrix.Transpose();
	/// ]]></code>
	/// The <c>result</c> variable will contain [[10, 20, 30], [11, 31], [32]].
	/// </example>

	public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
	{
		source.ThrowIfNull();

		return _(); IEnumerable<IEnumerable<T>> _()
		{
			IEnumerator<T>?[] enumerators = source.Select(e => e.GetEnumerator()).Acquire();

			try
			{
				while (true)
				{
					var column = new T[enumerators.Length];
					var count = 0;
					for (var i = 0; i < enumerators.Length; i++)
					{
						var enumerator = enumerators[i];
						if (enumerator == null)
							continue;

						if (enumerator.MoveNext())
						{
							column[count++] = enumerator.Current;
						}
						else
						{
							enumerator.Dispose();
							enumerators[i] = null;
						}
					}

					if (count == 0)
						yield break;

					Array.Resize(ref column, count);
					yield return column;
				}
			}
			finally
			{
				foreach (var e in enumerators)
					e?.Dispose();
			}
		}
	}
}
