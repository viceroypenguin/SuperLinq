namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Inserts the elements of a sequence into another sequence at a
	/// specified index.
	/// </summary>
	/// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
	/// <param name="first">The source sequence.</param>
	/// <param name="second">The sequence that will be inserted.</param>
	/// <param name="index">
	/// The zero-based index at which to insert elements from
	/// <paramref name="second"/>.</param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="first"/>
	/// plus the elements of <paramref name="second"/> inserted at
	/// the given index.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="index"/> is negative.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown lazily if <paramref name="index"/> is greater than the
	/// length of <paramref name="first"/>. The validation occurs when
	/// yielding the next element after having iterated
	/// <paramref name="first"/> entirely.
	/// </exception>
	public static IAsyncEnumerable<T> Insert<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, int index)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();
		index.ThrowIfLessThan(0);

		return _(first, second, index);

		static async IAsyncEnumerable<T> _(IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, int index, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var i = -1;

			await using var iter = first.GetConfiguredAsyncEnumerator(cancellationToken);

			while (++i < index && await iter.MoveNextAsync())
				yield return iter.Current;

			if (i < index)
				index.ThrowOutOfRange("Insertion index is greater than the length of the first sequence.");

			await foreach (var item in second.WithCancellation(cancellationToken).ConfigureAwait(false))
				yield return item;

			while (await iter.MoveNextAsync())
				yield return iter.Current;
		}
	}

	/// <summary>
	/// Inserts the elements of a sequence into another sequence at a
	/// specified index.
	/// </summary>
	/// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
	/// <param name="first">The source sequence.</param>
	/// <param name="second">The sequence that will be inserted.</param>
	/// <param name="index">
	/// The zero-based index at which to insert elements from
	/// <paramref name="second"/>.</param>
	/// <returns>
	/// A sequence that contains the elements of <paramref name="first"/>
	/// plus the elements of <paramref name="second"/> inserted at
	/// the given index.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="index"/> is negative.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown lazily if <paramref name="index"/> is greater than the
	/// length of <paramref name="first"/>. The validation occurs when
	/// yielding the next element after having iterated
	/// <paramref name="first"/> entirely.
	/// </exception>
	public static IAsyncEnumerable<T> Insert<T>(this IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, Index index)
	{
		first.ThrowIfNull();
		second.ThrowIfNull();

		return !index.IsFromEnd ? Insert(first, second, index) :
			index.Value == 0 ? first.Concat(second) :
			_(first, second, index.Value);

		static async IAsyncEnumerable<T> _(IAsyncEnumerable<T> first, IAsyncEnumerable<T> second, int index, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var e = first.CountDown(index, ValueTuple.Create).GetConfiguredAsyncEnumerator(cancellationToken);

			if (await e.MoveNextAsync())
			{
				var (_, countdown) = e.Current;
				if (countdown is { } n && n != index - 1)
					index.ThrowOutOfRange("Insertion index is greater than the length of the first sequence.");

				do
				{
					T a;
					(a, countdown) = e.Current;
					if (countdown == index - 1)
					{
						await foreach (var b in second.WithCancellation(cancellationToken).ConfigureAwait(false))
							yield return b;
					}

					yield return a;
				}
				while (await e.MoveNextAsync());
			}
		}
	}
}
