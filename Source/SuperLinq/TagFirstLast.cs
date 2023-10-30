namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence resulting from applying a function to each element in the source sequence with additional
	/// parameters indicating whether the element is the first and/or last of the sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <returns>
	/// Returns the resulting sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.TagFirstLast();
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// <c>(item: 123, isFirst: True, isLast: False)</c>,
	/// <c>(item: 456, isFirst: False, isLast: False)</c> and
	/// <c>(item: 789, isFirst: False, isLast: True)</c> in turn.
	/// </example>
	public static IEnumerable<(TSource item, bool isFirst, bool isLast)> TagFirstLast<TSource>(this IEnumerable<TSource> source)
	{
		return source.TagFirstLast(ValueTuple.Create);
	}

	/// <summary>
	/// Returns a sequence resulting from applying a function to each element in the source sequence with additional
	/// parameters indicating whether the element is the first and/or last of the sequence.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
	/// <param name="source">The source sequence.</param>
	/// <param name="resultSelector">A function that determines how to project the each element along with its first or
	/// last tag.</param>
	/// <returns>
	/// Returns the resulting sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// This operator uses deferred execution and streams its results.
	/// </remarks>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.TagFirstLast((num, fst, lst) => new
	///              {
	///                  Number = num,
	///                  IsFirst = fst, IsLast = lst
	///              });
	/// ]]></code>
	/// The <c>result</c> variable, when iterated over, will yield
	/// <c>{ Number = 123, IsFirst = True, IsLast = False }</c>,
	/// <c>{ Number = 456, IsFirst = False, IsLast = False }</c> and
	/// <c>{ Number = 789, IsFirst = False, IsLast = True }</c> in turn.
	/// </example>
	public static IEnumerable<TResult> TagFirstLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool, bool, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(resultSelector);

		if (source is IList<TSource> list)
			return new TagFirstLastIterator<TSource, TResult>(list, resultSelector);

		return Core(source, resultSelector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, Func<TSource, bool, bool, TResult> resultSelector)
		{
			using var iter = source.GetEnumerator();

			if (!iter.MoveNext())
				yield break;

			var cur = iter.Current;
			var first = true;

			while (iter.MoveNext())
			{
				yield return resultSelector(cur, first, false);
				cur = iter.Current;
				first = false;
			}

			yield return resultSelector(cur, first, true);
		}
	}

	private class TagFirstLastIterator<TSource, TResult> : ListIterator<TResult>
	{
		private readonly IList<TSource> _source;
		private readonly Func<TSource, bool, bool, TResult> _resultSelector;

		public TagFirstLastIterator(IList<TSource> source, Func<TSource, bool, bool, TResult> resultSelector)
		{
			_source = source;
			_resultSelector = resultSelector;
		}

		public override int Count => _source.Count;

		protected override IEnumerable<TResult> GetEnumerable()
		{
			if (_source.Count <= 1)
			{
				if (_source.Count == 1)
					yield return _resultSelector(_source[0], true, true);
				yield break;
			}

			yield return _resultSelector(_source[0], true, false);

			var cnt = (uint)_source.Count - 1;

			for (var i = 1; i < cnt; i++)
				yield return _resultSelector(_source[i], false, false);

			yield return _resultSelector(_source[^1], false, true);
		}

		protected override TResult ElementAt(int index)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(index);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Count);

			return _resultSelector(_source[index], index == 0, index == _source.Count - 1);
		}
	}
}
