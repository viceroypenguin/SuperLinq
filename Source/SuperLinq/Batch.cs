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
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	public static IEnumerable<IList<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size) =>
#if NET6_0_OR_GREATER
		source.Chunk(size);
#else
		source.Buffer(size);
#endif

	/// <summary>
	/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>
	/// and applies a projection to each chunk.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by <paramref name="resultSelector"/>.</typeparam>
	/// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
	/// <param name="size">The maximum size of each chunk.</param>
	/// <param name="resultSelector">The projection that .</param>
	/// <returns>An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size size.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
	/// <exception cref="ArgumentNullException"><paramref name="resultSelector"/> is null.</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
	public static IEnumerable<TResult> Batch<TSource, TResult>(
		this IEnumerable<TSource> source, int size,
		Func<IList<TSource>, TResult> resultSelector)
	{
		resultSelector.ThrowIfNull();
		return source.Batch(size).Select(resultSelector);
	}
}
