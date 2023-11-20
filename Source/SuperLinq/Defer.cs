// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates an enumerable sequence based on an enumerable factory function.
	/// </summary>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="enumerableFactory">Enumerable factory function.</param>
	/// <returns>Sequence that will invoke the enumerable factory upon iteration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="enumerableFactory"/> is <see
	/// langword="null"/>.</exception>
	public static IEnumerable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
	{
		ArgumentNullException.ThrowIfNull(enumerableFactory);

		return Core(enumerableFactory);

		static IEnumerable<TResult> Core(Func<IEnumerable<TResult>> enumerableFactory)
		{
			foreach (var el in enumerableFactory())
				yield return el;
		}
	}
}
