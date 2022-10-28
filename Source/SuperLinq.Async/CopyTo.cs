namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Copies the contents of a sequence into a provided list.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="list">The list that is the destination of the elements copied from <paramref
	/// name="source"/>.</param>
	/// <param name="cancellationToken">The <see cref="CancellationToken"/> in use.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position 0.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static ValueTask CopyTo<TSource>(this IAsyncEnumerable<TSource> source, IList<TSource> list, CancellationToken cancellationToken = default)
	{
		return source.CopyTo(list, 0, cancellationToken);
	}

	/// <summary>
	/// Copies the contents of a sequence into a provided list.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of elements of <paramref name="source"/></typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="list">The list that is the destination of the elements copied from <paramref
	/// name="source"/>.</param>
	/// <param name="index">The position in <paramref name="list"/> at which to start copying data</param>
	/// <param name="cancellationToken">The <see cref="CancellationToken"/> in use.</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position
	/// <paramref name="index"/>.
	/// </para>
	/// <para>
	/// This operator executes immediately.
	/// </para>
	/// </remarks>
	public static async ValueTask CopyTo<TSource>(this IAsyncEnumerable<TSource> source, IList<TSource> list, int index, CancellationToken cancellationToken = default)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(list);
		Guard.IsGreaterThanOrEqualTo(index, 0);

		if (list is List<TSource> l)
		{
			var i = index;
			await foreach (var el in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (i < l.Count)
					l[i] = el;
				else
					l.Add(el);
				i++;
			}
		}
		else if (list is TSource[] array)
		{
			var i = index;
			await foreach (var el in source.WithCancellation(cancellationToken).ConfigureAwait(false))
				array[i++] = el;
		}
		else
		{
			var i = index;
			await foreach (var el in source.WithCancellation(cancellationToken).ConfigureAwait(false))
			{
				if (i < list.Count)
					list[i] = el;
				else
					list.Add(el);
				i++;
			}
		}
	}
}
