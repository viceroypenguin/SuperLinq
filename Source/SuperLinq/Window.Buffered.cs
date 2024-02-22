﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	///     projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="selector"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="selector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to evaluate a sliding window over.
	/// </param>
	/// <param name="size">
	///	    The size (number of elements) in each window.
	/// </param>
	/// <param name="selector">
	///	    A transform function to apply to each window.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each
	///     window of <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below <c>1</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the start of
	///     the sequence.
	/// </para>
	/// <para>
	///	    In this overload of <c>Window</c>, a single array of length <paramref name="size"/> is allocated as a
	///     buffer for all subsequences.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		int size,
		ReadOnlySpanFunc<TSource, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);

		return WindowImpl(source, new TSource[size], size, WindowType.Normal, selector);
	}

	/// <summary>
	///	    Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	///     projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="selector"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="selector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to evaluate a sliding window over.
	/// </param>
	/// <param name="array">
	///	    An array to use as a buffer for each subsequence.
	/// </param>
	/// <param name="selector">
	///	    A transform function to apply to each window.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each
	///     window of <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A window can contain fewer elements than <c><paramref name="array"/>.Length</c>, especially as it slides
	///     over the start of the sequence.
	/// </para>
	/// <para>
	///	    In this overload of <c>Window</c>, <paramref name="array"/> is used as a common buffer for all
	///     subsequences.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		ReadOnlySpanFunc<TSource, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);

		return WindowImpl(source, array, array.Length, WindowType.Normal, selector);
	}

	/// <summary>
	///	    Processes a sequence into a series of subsequences representing a windowed subset of the original, and then
	///     projecting them into a new form.
	/// </summary>
	/// <typeparam name="TSource">
	///	    The type of the elements of <paramref name="selector"/>.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    The type of the value return by <paramref name="selector"/>.
	/// </typeparam>
	/// <param name="source">
	///	    The sequence to evaluate a sliding window over.
	/// </param>
	/// <param name="array">
	///	    An array to use as a buffer for each subsequence.
	/// </param>
	/// <param name="size">
	///	    The size (number of elements) in each window.
	/// </param>
	/// <param name="selector">
	///	    A transform function to apply to each window.
	/// </param>
	/// <returns>
	///	    An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each
	///     window of <paramref name="source"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null" />.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	///	    <paramref name="size"/> is below <c>1</c> or above <c><paramref name="array"/>.Length</c>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    A window can contain fewer elements than <paramref name="size"/>, especially as it slides over the start of
	///     the sequence.
	/// </para>
	/// <para>
	///	    In this overload of <c>Window</c>, <paramref name="array"/> is used as a common buffer for all
	///     subsequences.<br/> This overload is provided to ease usage of common buffers, such as those rented from <see
	///     cref="System.Buffers.ArrayPool{T}"/>, which may return an array larger than requested.
	/// </para>
	/// <para>
	///	    This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Window<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		int size,
		ReadOnlySpanFunc<TSource, TResult> selector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(array);
		ArgumentNullException.ThrowIfNull(selector);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(size, array.Length);

		return WindowImpl(source, array, size, WindowType.Normal, selector);
	}
}
