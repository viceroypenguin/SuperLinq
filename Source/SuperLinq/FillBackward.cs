using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence with each null reference or value in the source replaced with the following non-null
	///     reference or value in that sequence.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <typeparam name="T">
	///	    Type of the elements in the source sequence.
	/// </typeparam>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> with null references or values replaced.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution semantics and streams its results. If references or values are <see
	///     langword="null" /> at the end of the sequence then they remain <see langword="null" />.
	/// </remarks>
	public static IEnumerable<T?> FillBackward<T>(this IEnumerable<T?> source)
	{
		return source.FillBackward(static e => e == null);
	}

	/// <summary>
	///	    Returns a sequence with each missing element in the source replaced with the following non-missing element
	///     in that sequence. An additional parameter specifies a function used to determine if an element is considered
	///     missing or not.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The function used to determine if an element in the sequence is considered missing.
	/// </param>
	/// <typeparam name="T">
	///	    Type of the elements in the source sequence.
	/// </typeparam>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="predicate"/> is <see langword="null" />.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution semantics and streams its results. If elements are missing at the end of
	///     the sequence then they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);

		return source is ICollection<T> coll
			? new FillBackwardCollection<T>(coll, predicate, fillSelector: default)
			: FillBackwardCore(source, predicate, fillSelector: default);
	}

	/// <summary>
	///	    Returns a sequence with each missing element in the source replaced with the following non-missing element
	///     in that sequence. Additional parameters specify two functions, one used to determine if an element is
	///     considered missing or not and another to provide the replacement for the missing element.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The function used to determine if an element in the sequence is considered missing.
	/// </param>
	/// <param name="fillSelector">
	///	    The function used to produce the element that will replace the missing one. Its first argument receives the
	///     current element considered missing while the second argument receives the next non-missing element.
	/// </param>
	/// <typeparam name="T">
	///	    Type of the elements in the source sequence.
	/// </typeparam>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="predicate"/>, or <paramref name="fillSelector"/> is <see
	///     langword="null" />.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution semantics and streams its results. If elements are missing at the end of
	///     the sequence then they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T> fillSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(fillSelector);

		return source is ICollection<T> coll
			? new FillBackwardCollection<T>(coll, predicate, fillSelector)
			: FillBackwardCore(source, predicate, fillSelector);
	}

	private static IEnumerable<T> FillBackwardCore<T>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T>? fillSelector)
	{
		List<T>? blanks = null;

		foreach (var item in source)
		{
			var isBlank = predicate(item);
			if (isBlank)
			{
				(blanks ??= []).Add(item);
			}
			else
			{
				if (blanks != null)
				{
					foreach (var blank in blanks)
					{
						yield return fillSelector != null
							? fillSelector(blank, item)
							: item;
					}

					blanks.Clear();
				}
				yield return item;
			}
		}

		if (blanks?.Count > 0)
		{
			foreach (var blank in blanks)
				yield return blank;
		}
	}

	private sealed class FillBackwardCollection<T>(
		ICollection<T> source,
		Func<T, bool> predicate,
		Func<T, T, T>? fillSelector
	) : CollectionIterator<T>
	{
		public override int Count => source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			FillBackwardCore(source, predicate, fillSelector);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(arrayIndex, Count);

			source.CopyTo(array, arrayIndex);

			var i = arrayIndex + source.Count - 1;
			for (; i >= arrayIndex && predicate(array[i]); i--)
				;

			if (i < arrayIndex)
				return;

			var last = array[i--];
			for (; i >= arrayIndex; i--)
			{
				if (predicate(array[i]))
				{
					array[i] = fillSelector != null
						? fillSelector(array[i], last)
						: last;
				}
				else
					last = array[i];
			}
		}
	}
}
