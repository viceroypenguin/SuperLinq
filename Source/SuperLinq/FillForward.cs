using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Returns a sequence with each null reference or value in the source replaced with the previous non-null
	///     reference or value seen in that sequence.
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
	///     langword="null" /> at the start of the sequence then they remain <see langword="null" />.
	/// </remarks>
	public static IEnumerable<T?> FillForward<T>(this IEnumerable<T?> source)
	{
		return source.FillForward(static e => e == null);
	}

	/// <summary>
	///	    Returns a sequence with each missing element in the source replaced with the previous non-missing element
	///     seen in that sequence. An additional parameter specifies a function used to determine if an element is
	///     considered missing or not.
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
	///	    This method uses deferred execution semantics and streams its results. If elements are missing at the start
	///     of the sequence then they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillForward<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);

		return source is ICollection<T> coll
			? new FillForwardCollection<T>(coll, predicate, fillSelector: default)
			: FillForwardCore(source, predicate, fillSelector: default);
	}

	/// <summary>
	///	    Returns a sequence with each missing element in the source replaced with one based on the previous
	///     non-missing element seen in that sequence. Additional parameters specify two functions, one used to
	///     determine if an element is considered missing or not and another to provide the replacement for the missing
	///     element.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The function used to determine if an element in the sequence is considered missing.
	/// </param>
	/// <param name="fillSelector">
	///	    The function used to produce the element that will replace the missing one. Its first argument receives the
	///     current element considered missing while the second argument receives the previous non-missing element.
	/// </param>
	/// <typeparam name="T">
	///	    Type of the elements in the source sequence.
	/// </typeparam>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="predicate"/>, or <paramref name="fillSelector"/> is <see
	///     langword="null" />.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution semantics and streams its results. If elements are missing at the start
	///     of the sequence then they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillForward<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T> fillSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);
		Guard.IsNotNull(fillSelector);

		return source is ICollection<T> coll
			? new FillForwardCollection<T>(coll, predicate, fillSelector)
			: FillForwardCore(source, predicate, fillSelector);
	}

	private static IEnumerable<T> FillForwardCore<T>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T>? fillSelector)
	{
		(bool, T?) seed = default;

		foreach (var item in source)
		{
			if (predicate(item))
			{
				yield return (seed, fillSelector) switch
				{
					((true, var s), { } f) => f(item, s),
					((true, var s), _) => s,
					_ => item,
				};
			}
			else
			{
				seed = (true, item);
				yield return item;
			}
		}
	}

	private sealed class FillForwardCollection<T> : CollectionIterator<T>
	{
		private readonly ICollection<T> _source;
		private readonly Func<T, bool> _predicate;
		private readonly Func<T, T, T>? _fillSelector;

		public FillForwardCollection(ICollection<T> source, Func<T, bool> predicate, Func<T, T, T>? fillSelector)
		{
			_source = source;
			_predicate = predicate;
			_fillSelector = fillSelector;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			FillForwardCore(_source, _predicate, _fillSelector);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			Guard.IsNotNull(array);
			Guard.IsBetweenOrEqualTo(arrayIndex, 0, array.Length - Count);

			_source.CopyTo(array, arrayIndex);

			var i = arrayIndex;
			var max = arrayIndex + _source.Count;
			for (; i < max && _predicate(array[i]); i++)
				;

			if (i >= max)
				return;

			var last = array[i++];
			for (; i < max; i++)
			{
				if (_predicate(array[i]))
				{
					array[i] = _fillSelector != null
						? _fillSelector(array[i], last)
						: last;
				}
				else
					last = array[i];
			}
		}
	}
}
