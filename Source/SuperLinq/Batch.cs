namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size
	/// size.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <paramref name="size"/>, specifically the final buffer of <paramref
	/// name="source"/>.
	/// </para>
	/// <para>
	/// Returned subsequences are buffered, but the overall operation is streamed.<br/>
	/// </para>
	/// </remarks>
	public static IEnumerable<IList<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
	{
		// yes this operator duplicates on net6+; but no name overlap, so leave alone
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return Core(source, size);

		static IEnumerable<IList<TSource>> Core(IEnumerable<TSource> source, int size)
		{
			TSource[]? array = null;

			if (source is ICollection<TSource> coll)
			{
				if (coll.Count == 0)
					yield break;

				if (coll.Count <= size)
				{
					array = new TSource[coll.Count];
					coll.CopyTo(array, 0);
					yield return array;
					yield break;
				}
			}
			else if (source.TryGetCollectionCount() == 0)
			{
				yield break;
			}

			var n = 0;
			foreach (var item in source)
			{
				(array ??= new TSource[size])[n++] = item;
				if (n == size)
				{
					yield return array;
					n = 0;
				}
			}

			if (n != 0)
			{
				Array.Resize(ref array, n);
				yield return array;
			}
		}
	}
}
