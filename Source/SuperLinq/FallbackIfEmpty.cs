﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns the elements of a sequence, but if it is empty then returns an alternate sequence from an array of
	///     values.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequences.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="fallback">
	///	    The array that is returned as the alternate sequence if <paramref name="source"/> is empty.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> that containing fallback values if <paramref name="source"/> is empty;
	///     otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="fallback"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The length of <paramref name="source"/> is not evaluated until enumeration. <paramref name="source"/> is
	///	    enumerated; if there is at least one item, the elements of <paramref name="source"/> will be streamed in a
	///	    deferred manner. If there are no items in <paramref name="source"/>, the items in <paramref name="fallback"/>
	///	    will be streamed.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, params T[] fallback)
	{
		return source.FallbackIfEmpty((IEnumerable<T>)fallback);
	}

	/// <summary>
	///	    Returns the elements of a sequence, but if it is empty then returns an alternate sequence of values.
	/// </summary>
	/// <typeparam name="T">
	///	    The type of the elements in the sequences.
	/// </typeparam>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="fallback">
	///	    The alternate sequence that is returned if <paramref name="source"/> is empty.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> that containing fallback values if <paramref name="source"/> is empty;
	///     otherwise, <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="fallback"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The length of <paramref name="source"/> is not evaluated until enumeration. <paramref name="source"/> is
	///	    enumerated; if there is at least one item, the elements of <paramref name="source"/> will be streamed using
	///	    deferred execution. If there are no items in <paramref name="source"/>, then <paramref name="fallback"/>
	///	    will be streamed using deferred execution.
	/// </para>
	/// </remarks>
	public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> fallback)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(fallback);

		return source.TryGetCollectionCount() is not null
			   && fallback.TryGetCollectionCount() is not null
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
			_source.GetCollectionCount() == 0
				? _fallback.Count()
				: _source.GetCollectionCount();

		protected override IEnumerable<T> GetEnumerable()
		{
			return _source.GetCollectionCount() == 0
				? _fallback
				: _source;
		}
	}
}
