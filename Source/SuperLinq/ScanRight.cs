﻿using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Performs a right-associative scan (inclusive prefix) on a sequence of elements. This operator is the
	///     right-associative version of the <see cref="Scan{TSource}(IEnumerable{TSource}, Func{TSource, TSource,
	///     TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="func">
	///	    A right-associative accumulator function to be invoked on each element. Its first argument is the current
	///     value in the sequence; second argument is the previous accumulator value.
	/// </param>
	/// <returns>
	///	    The scanned sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> ScanRight<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(func);

		if (source is ICollection<TSource> coll)
			return new ScanRightIterator<TSource>(coll, func);

		return ScanRightCore(source, func);
	}

	private static IEnumerable<TSource> ScanRightCore<TSource>(IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
	{
		var list = source.ToList();

		if (list.Count == 0)
			yield break;

		var seed = list[^1];
		var stack = new Stack<TSource>(list.Count);
		stack.Push(seed);

		for (var i = list.Count - 2; i >= 0; i--)
		{
			seed = func(list[i], seed);
			stack.Push(seed);
		}

		foreach (var item in stack)
			yield return item;
	}

	private sealed class ScanRightIterator<T> : CollectionIterator<T>
	{
		private readonly ICollection<T> _source;
		private readonly Func<T, T, T> _func;

		public ScanRightIterator(ICollection<T> source, Func<T, T, T> func)
		{
			_source = source;
			_func = func;
		}

		public override int Count => _source.Count;

		[ExcludeFromCodeCoverage]
		protected override IEnumerable<T> GetEnumerable() =>
			ScanRightCore(_source, _func);

		public override void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			var (sList, b, cnt) = _source is IList<T> s
				? (s, 0, s.Count)
				: (array, arrayIndex, SuperEnumerable.CopyTo(_source, array, arrayIndex));

			var i = cnt - 1;
			var state = sList[b + i];
			array[arrayIndex + i] = state;

			for (i--; i >= 0; i--)
			{
				state = _func(sList[b + i], state);
				array[arrayIndex + i] = state;
			}
		}
	}

	/// <summary>
	///	    Performs a right-associative scan (inclusive prefix) on a sequence of elements. This operator is the
	///     right-associative version of the <see cref="Scan{TSource}(IEnumerable{TSource}, Func{TSource, TSource,
	///     TSource})"/> LINQ operator.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Type of elements in source sequence.
	/// </typeparam>
	/// <typeparam name="TAccumulate">
	///	    Type of the state accumulator.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="func">
	///	    A right-associative accumulator function to be invoked on each element. Its first argument is the current
	///     value in the sequence; second argument is the previous accumulator value.
	/// </param>
	/// <param name="seed">
	///	    The initial accumulator value.
	///	</param>
	/// <returns>
	///	    The scanned sequence.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="func"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method is implemented by using deferred execution. However, <paramref name="source"/> will be consumed
	///     in it's entirety immediately when first element of the returned sequence is consumed. 
	/// </para>
	/// </remarks>
	public static IEnumerable<TAccumulate> ScanRight<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(func);

		return Core(source, seed, func);

		static IEnumerable<TAccumulate> Core(IEnumerable<TSource> source, TAccumulate seed, Func<TSource, TAccumulate, TAccumulate> func)
		{
			var list = source.ToList();
			var stack = new Stack<TAccumulate>(list.Count + 1);
			stack.Push(seed);

			for (var i = list.Count - 1; i >= 0; i--)
			{
				seed = func(list[i], seed);
				stack.Push(seed);
			}

			foreach (var item in stack)
				yield return item;
		}
	}
}
