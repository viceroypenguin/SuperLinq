using System.Collections;
using System.Runtime.ExceptionServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Creates a buffer with a shared view over the source sequence, causing each enumerator to fetch the next element
	/// from the source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <returns>Buffer enabling each enumerator to retrieve elements from the shared source sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
	/// <example>
	/// <code><![CDATA[
	///     using var rng = Enumerable.Range(0, 10).Share();
	///     var e1 = rng.GetEnumerator();    // Both e1 and e2 will consume elements from
	///     var e2 = rng.GetEnumerator();    // the source sequence.
	///     Assert.IsTrue(e1.MoveNext());
	///     Assert.AreEqual(0, e1.Current);
	///     Assert.IsTrue(e1.MoveNext());
	///     Assert.AreEqual(1, e1.Current);
	///     Assert.IsTrue(e2.MoveNext());    // e2 "steals" element 2
	///     Assert.AreEqual(2, e2.Current);
	///     Assert.IsTrue(e1.MoveNext());    // e1 can't see element 2
	///     Assert.AreEqual(3, e1.Current);
	/// ]]></code>
	/// </example>
	public static IBuffer<TSource> Share<TSource>(this IEnumerable<TSource> source)
	{
		Guard.IsNotNull(source);

		return new SharedBuffer<TSource>(source);
	}

	/// <summary>
	/// Shares the source sequence within a selector function where each enumerator can fetch the next element from the
	/// source sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <typeparam name="TResult">Result sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="selector">Selector function with shared access to the source sequence for each enumerator.</param>
	/// <returns>Sequence resulting from applying the selector function to the shared view over the source
	/// sequence.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see
	/// langword="null"/>.</exception>
	public static IEnumerable<TResult> Share<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
	{
		Guard.IsNotNull(source);
		Guard.IsNotNull(selector);

		return Core(source, selector);

		static IEnumerable<TResult> Core(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> selector)
		{
			using var buffer = source.Share();
			foreach (var i in selector(buffer))
				yield return i;
		}
	}

	private sealed class SharedBuffer<T> : IBuffer<T>
	{
		private readonly object _lock = new();

		private IEnumerable<T>? _source;

		private IEnumerator<T>? _enumerator;
		private bool _initialized;
		private int _version;

		private ExceptionDispatchInfo? _exception;

		private bool _disposed;

		public SharedBuffer(IEnumerable<T> source)
		{
			_source = source;
		}

		public int Count => 0;

		public void Reset()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IBuffer<T>));

				_initialized = false;
				_version++;

				_enumerator?.Dispose();
				_enumerator = null;
				_exception = null;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			InitializeEnumerator();
			return GetEnumeratorImpl();
		}

		private void InitializeEnumerator()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException(nameof(IBuffer<T>));

				Assert.NotNull(_source);

				_exception?.Throw();

				if (_initialized)
					return;

				try
				{
					_enumerator = _source.GetEnumerator();
					_initialized = true;
				}
				catch (Exception ex)
				{
					_exception = ExceptionDispatchInfo.Capture(ex);
					throw;
				}
			}
		}

		private IEnumerator<T> GetEnumeratorImpl()
		{
			var version = _version;
			while (true)
			{
				T? element;
				lock (_lock)
				{
					if (_disposed)
						ThrowHelper.ThrowObjectDisposedException(nameof(IBuffer<T>));
					if (!_initialized
						|| version != _version)
					{
						ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
					}

					_exception?.Throw();

					if (_enumerator == null)
						break;

					var moved = false;
					try
					{
						moved = _enumerator.MoveNext();
					}
					catch (Exception ex)
					{
						_exception = ExceptionDispatchInfo.Capture(ex);
						_enumerator.Dispose();
						_enumerator = null;
						throw;
					}

					if (!moved)
					{
						_enumerator.Dispose();
						_enumerator = null;
						break;
					}

					element = _enumerator.Current;
				}

				yield return element;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Dispose()
		{
			lock (_lock)
			{
				_disposed = true;
				_enumerator?.Dispose();
				_enumerator = null;
				_source = null;
			}
		}
	}
}
