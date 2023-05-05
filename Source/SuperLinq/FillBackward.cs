using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence with each null reference or value in the source
	/// replaced with the following non-null reference or value in
	/// that sequence.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> with null references or values
	/// replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If references or values are null at the end of the
	/// sequence then they remain null.
	/// </remarks>
	public static IEnumerable<T?> FillBackward<T>(this IEnumerable<T?> source)
	{
		return source.FillBackward(static e => e == null);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. An
	/// additional parameter specifies a function used to determine if an
	/// element is considered missing or not.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> with missing values replaced.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);

		return source is ICollection<T> coll
			? new FillBackwardCollection<T>(coll, predicate, fillSelector: default)
			: FillBackwardCore(source, predicate, fillSelector: default);
	}

	/// <summary>
	/// Returns a sequence with each missing element in the source replaced
	/// with the following non-missing element in that sequence. Additional
	/// parameters specify two functions, one used to determine if an
	/// element is considered missing or not and another to provide the
	/// replacement for the missing element.
	/// </summary>
	/// <param name="source">The source sequence.</param>
	/// <param name="predicate">The function used to determine if
	/// an element in the sequence is considered missing.</param>
	/// <param name="fillSelector">The function used to produce the element
	/// that will replace the missing one. Its first argument receives the
	/// current element considered missing while the second argument
	/// receives the next non-missing element.</param>
	/// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> with missing elements filled.
	/// </returns>
	/// <remarks>
	/// This method uses deferred execution semantics and streams its
	/// results. If elements are missing at the end of the sequence then
	/// they remain missing.
	/// </remarks>
	public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source, Func<T, bool> predicate, Func<T, T, T> fillSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(predicate);
		Guard.IsNotNull(fillSelector);

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
				(blanks ??= new List<T>()).Add(item);
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

	private sealed class FillBackwardCollection<T> : IteratorCollection<T, T>
	{
		private readonly ICollection<T> _source;
		private readonly Func<T, bool> _predicate;
		private readonly Func<T, T, T>? _fillSelector;

		public FillBackwardCollection(ICollection<T> source, Func<T, bool> predicate, Func<T, T, T>? fillSelector)
		{
			_source = source;
			_predicate = predicate;
			_fillSelector = fillSelector;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		public override IEnumerator<T> GetEnumerator() =>
			FillBackwardCore(_source, _predicate, _fillSelector)
				.GetEnumerator();

		public override void CopyTo(T[] array, int arrayIndex)
		{
			_source.CopyTo(array, arrayIndex);

			var i = arrayIndex + _source.Count - 1;
			for (; _predicate(array[i]); i--)
			{
				if (i < arrayIndex)
					return;
			}

			var last = array[i--];
			for (; i >= arrayIndex; i--)
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

		[ExcludeFromCodeCoverage]
		public override bool Contains(T item) =>
			FillBackwardCore(_source, _predicate, _fillSelector)
				.Contains(item);
	}
}
