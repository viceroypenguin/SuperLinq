#region License and Terms
// SuperLinq - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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

namespace SuperLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 1 element.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 1 element</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 1)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 1, Actual: {elements.Count})");

			return folder(
				elements[0]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 2 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 2 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 2)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 2, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 3 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 3 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 3)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 3, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 4 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 4 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 4)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 4, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 5 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 5 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 5)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 5, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 6 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 6 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 6)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 6, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 7 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 7 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 7)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 7, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 8 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 8 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 8)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 8, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 9 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 9 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 9)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 9, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 10 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 10 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 10)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 10, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 11 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 11 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 11)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 11, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 12 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 12 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 12)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 12, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10],
				elements[11]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 13 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 13 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 13)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 13, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10],
				elements[11],
				elements[12]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 14 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 14 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 14)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 14, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10],
				elements[11],
				elements[12],
				elements[13]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 15 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 15 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 15)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 15, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10],
				elements[11],
				elements[12],
				elements[13],
				elements[14]
			);
        }

        /// <summary>
        /// Returns the result of applying a function to a sequence of
        /// 16 elements.
        /// </summary>
        /// <remarks>
        /// This operator uses immediate execution and effectively buffers
        /// as many items of the source sequence as necessary.
        /// </remarks>
        /// <typeparam name="T">Type of element in the source sequence</typeparam>
        /// <typeparam name="TResult">Type of the result</typeparam>
        /// <param name="source">The sequence of items to fold.</param>
        /// <param name="folder">Function to apply to the elements in the sequence.</param>
        /// <returns>The folded value returned by <paramref name="folder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="folder"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> does not contain exactly 16 elements</exception>

        public static TResult Fold<T, TResult>(this IEnumerable<T> source, Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
        {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (folder == null) throw new ArgumentNullException(nameof(folder));

			var elements = source.ToList();
			if (elements.Count != 16)
				throw new InvalidOperationException(
					$"Sequence contained an incorrect number of elements. (Expected: 16, Actual: {elements.Count})");

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5],
				elements[6],
				elements[7],
				elements[8],
				elements[9],
				elements[10],
				elements[11],
				elements[12],
				elements[13],
				elements[14],
				elements[15]
			);
        }

    }
}
