// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates an enumerable sequence based on an enumerable factory function.
	/// </summary>
	/// <typeparam name="TResult">
	///	    Result sequence element type.
	/// </typeparam>
	/// <param name="enumerableFactory">
	///	    Enumerable factory function.
	/// </param>
	/// <returns>
	///	    Sequence that will invoke the enumerable factory upon iteration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="enumerableFactory"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="ArgumentNullException">
	///	    (Thrown lazily) The sequence <c>source</c> returned by <paramref name="enumerableFactory"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    <paramref name="enumerableFactory"/> is not run until the sequence returned by <see
	///	    cref="Defer{TResult}(Func{IEnumerable{TResult}})"/> is enumerated. At enumeration, <paramref
	///	    name="enumerableFactory"/> is executed and the sequence returned is enumerated in a streaming manner and
	///	    values are returned similarly.
	/// </para>
	/// <para>
	///	    <paramref name="enumerableFactory"/> is executed each time the sequence returned by <see
	///	    cref="Defer{TResult}(Func{IEnumerable{TResult}})"/> is enumerated.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
	{
		ArgumentNullException.ThrowIfNull(enumerableFactory);

		return Core(enumerableFactory);

		static IEnumerable<TResult> Core(Func<IEnumerable<TResult>> enumerableFactory)
		{
			var source = enumerableFactory();
			Guard.IsNotNull(source);

			foreach (var el in source)
				yield return el;
		}
	}
}
