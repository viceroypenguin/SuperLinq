﻿namespace SuperLinq.Async;

#nullable enable

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns the result of applying a function to a sequence of 1 element.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 1 element.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, TResult> folder
		)
		{
			var elements = await source.AssertCount(1).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 2 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 2 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(2).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0],
				elements[1]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 3 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 3 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(3).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0],
				elements[1],
				elements[2]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 4 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 4 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(4).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 5 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 5 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(5).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 6 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 6 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(6).ToListAsync().ConfigureAwait(false);

			return folder(
				elements[0],
				elements[1],
				elements[2],
				elements[3],
				elements[4],
				elements[5]
			);
		}
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 7 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 7 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(7).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 8 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 8 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(8).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 9 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 9 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(9).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 10 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 10 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(10).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 11 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 11 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(11).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 12 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 12 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(12).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 13 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 13 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(13).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 14 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 14 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(14).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 15 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 15 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(15).ToListAsync().ConfigureAwait(false);

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
	}

	/// <summary>
	/// Returns the result of applying a function to a sequence of 16 elements.
	/// </summary>
	/// <remarks>
	/// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
	/// </remarks>
	/// <typeparam name="T">Type of element in the source sequence</typeparam>
	/// <typeparam name="TResult">Type of the result</typeparam>
	/// <param name="source">The sequence of items to fold.</param>
	/// <param name="folder">Function to apply to the elements in the sequence.</param>
	/// <returns>The folded value returned by <paramref name="folder"/>.</returns>
	/// <exception cref="global::System.ArgumentNullException"><paramref name="source"/> or <paramref name="folder"/> is null.</exception>
	/// <exception cref="global::System.InvalidOperationException">
	/// <paramref name="source"/> does not contain exactly 16 elements.
	/// </exception>
	public static global::System.Threading.Tasks.ValueTask<TResult> Fold<T, TResult>(
		this global::System.Collections.Generic.IAsyncEnumerable<T> source, 
		global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
	)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(folder);

		return Core(source, folder);

		static async global::System.Threading.Tasks.ValueTask<TResult> Core(
			global::System.Collections.Generic.IAsyncEnumerable<T> source, 
			global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder
		)
		{
			var elements = await source.AssertCount(16).ToListAsync().ConfigureAwait(false);

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
