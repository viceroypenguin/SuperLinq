namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence from an array of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The array that is returned as the alternate
	/// sequence if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, params T[] fallback)
	{
		return source.FallbackIfEmpty((IEnumerable<T>)fallback);
	}

	/// <summary>
	/// Returns the elements of a sequence, but if it is empty then
	/// returns an alternate sequence of values.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the sequences.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="fallback">The alternate sequence that is returned
	/// if <paramref name="source"/> is empty.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that containing fallback values
	/// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="fallback"/> is <see langword="null"/>.</exception>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> fallback)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(fallback);

		return source is ICollection<T> 
			? new FallbackIfEmptyCollectionIterator<T>(source, fallback) 
			: Core(source, fallback);

		static IEnumerable<T> Core(IEnumerable<T> source, IEnumerable<T> fallback)
		{
			using (var e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					do
					{
						yield return e.Current;
					} while (e.MoveNext());

					yield break;
				}
			}

			foreach (var item in fallback)
				yield return item;
		}
	}

	private sealed class FallbackIfEmptyCollectionIterator<T> : CollectionIterator<T>
	{
		private readonly IEnumerable<T> _source;
		private readonly IEnumerable<T> _fallback;

		public FallbackIfEmptyCollectionIterator(IEnumerable<T> source, IEnumerable<T> fallback)
		{
			_source = source;
			_fallback = fallback;
		}

		public override int Count =>
			_source.TryGetCollectionCount() is { } and 0
				? _fallback.Count()
				: _source.GetCollectionCount();

		protected override IEnumerable<T> GetEnumerable()
		{
			return _source.TryGetCollectionCount() is { } and 0
				? _fallback
				: _source;
		}
	}
}
