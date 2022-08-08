using System.Runtime.CompilerServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Filters a sequence of values based on an enumeration of boolean values
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
	/// <param name="filter">An <see cref="IEnumerable{T}"/> of boolean values identifying which elements of <paramref name="source"/> to keep.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> that contains elements from the input sequence 
	/// where the matching value in <paramref name="filter"/> is <see langword="true"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="filter"/> is <see langword="null"/>.</exception>
	public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, IEnumerable<bool> filter)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(filter);

		return _(source, filter);

		static IEnumerable<TSource> _(IEnumerable<TSource> source, IEnumerable<bool> filter)
		{
			using var sIter = source.GetEnumerator();
			using var fIter = filter.GetEnumerator();

			while (true)
			{
				var sMoved = sIter.MoveNext();
				var fMoved = fIter.MoveNext();
				if (sMoved != fMoved)
					ThrowHelper.ThrowArgumentException(nameof(filter), "'source' and 'filter' did not have equal lengths.");

				if (!sMoved)
					yield break;

				if (fIter.Current)
					yield return sIter.Current;
			}
		}
	}
}
