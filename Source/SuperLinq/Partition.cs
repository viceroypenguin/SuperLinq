﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Partitions or splits a sequence in two using a predicate.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate function.
	/// </param>
	/// <typeparam name="T">
	///	    Type of source elements.
	/// </typeparam>
	/// <returns>
	///	    A tuple of elements satisfying the predicate and those that do not, respectively.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static (IEnumerable<T> True, IEnumerable<T> False)
		Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		return source.Partition(predicate, ValueTuple.Create);
	}

	/// <summary>
	///	    Partitions or splits a sequence in two using a predicate and then projects a result from the two.
	/// </summary>
	/// <param name="source">
	///	    The source sequence.
	/// </param>
	/// <param name="predicate">
	///	    The predicate function.
	/// </param>
	/// <param name="resultSelector">
	///	    Function that projects the result from sequences of elements that satisfy the predicate and those that do
	///     not, respectively, passed as arguments.
	/// </param>
	/// <typeparam name="T">
	///	    Type of source elements.
	/// </typeparam>
	/// <typeparam name="TResult">
	///	    Type of the result.
	/// </typeparam>
	/// <returns>
	///	    The return value from <paramref name="resultSelector"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="predicate"/>, or <paramref name="resultSelector"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method executes immediately.
	/// </para>
	/// </remarks>
	public static TResult Partition<T, TResult>(
		this IEnumerable<T> source,
		Func<T, bool> predicate,
		Func<IEnumerable<T>, IEnumerable<T>, TResult> resultSelector)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(resultSelector);

		var lookup = source.ToLookup(predicate);
		return resultSelector(lookup[true], lookup[false]);
	}
}
