#region License and Terms
// SuperLinq - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size size.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	public static IEnumerable<IList<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="resultSelector"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <param name="resultSelector">A transform function to apply to each chunk.</param>
	/// <returns>A sequence of elements returned by <paramref name="resultSelector"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> is
	/// null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <paramref name="size"/>, specifically the final chunk of <paramref
	/// name="source"/>.
	/// </para>
	/// <para>
	/// In this overload of <c>Batch</c>, a single array of length <paramref name="size"/> is allocated as a buffer for
	/// all subsequences.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(resultSelector);
		Guard.IsGreaterThanOrEqualTo(size, 1);

		return BatchImpl(source, new TSource[size], size, resultSelector);
	}

	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <c><paramref name="array"/>.Length</c>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="resultSelector"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="array">An array to use as a buffer for each chunk.</param>
	/// <param name="resultSelector">A transform function to apply to each chunk.</param>
	/// <returns>A sequence of elements returned by <paramref name="resultSelector"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="resultSelector"/>, or
	/// <paramref name="array"/> is null.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <c><paramref name="array"/>.Length</c>, specifically the final chunk of
	/// <paramref name="source"/>.
	/// </para>
	/// <para>
	/// In this overload of <c>Batch</c>, <paramref name="array"/> is used as a common buffer for all subsequences.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(array);
		Guard.IsNotNull(resultSelector);

		return BatchImpl(source, array, array.Length, resultSelector);
	}

	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value return by <paramref name="resultSelector"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <param name="array">An array to use as a buffer for each chunk.</param>
	/// <param name="resultSelector">A transform function to apply to each chunk.</param>
	/// <returns>A sequence of elements returned by <paramref name="resultSelector"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="resultSelector"/>, or
	/// <paramref name="array"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1 or above <c><paramref
	/// name="array"/>.Length</c>.</exception>
	/// <remarks>
	/// <para>
	/// A chunk can contain fewer elements than <paramref name="size"/>, specifically the final chunk of <paramref
	/// name="source"/>.
	/// </para>
	/// <para>
	/// In this overload of <c>Batch</c>, <paramref name="array"/> is used as a common buffer for all subsequences.<br/>
	/// This overload is provided to ease usage of common buffers, such as those rented from <see
	/// cref="System.Buffers.ArrayPool{T}"/>, which may return an array larger than requested.
	/// </para>
	/// <para>
	/// This operator uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(array);
		Guard.IsNotNull(resultSelector);
		Guard.IsBetweenOrEqualTo(size, 1, array.Length);

		return BatchImpl(source, array, size, resultSelector);
	}

	private static IEnumerable<TResult> BatchImpl<TSource, TResult>(
		IEnumerable<TSource> source,
		TSource[] array,
		int size,
		Func<IReadOnlyList<TSource>, TResult> resultSelector)
	{
		if (source is ICollection<TSource> coll
			&& coll.Count < size)
		{
			coll.CopyTo(array, 0);
			yield return resultSelector(new ArraySegment<TSource>(array, 0, coll.Count));
			yield break;
		}

		var n = 0;
		foreach (var item in source)
		{
			array[n++] = item;
			if (n == size)
			{
				yield return resultSelector(new ArraySegment<TSource>(array, 0, n));
				n = 0;
			}
		}

		if (n != 0)
			yield return resultSelector(new ArraySegment<TSource>(array, 0, n));
	}
}
