using System.Collections;
using System.Runtime.ExceptionServices;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	///	    Creates a buffer with a shared view over the source sequence, causing each enumerator to fetch the next
	///     element from the source sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> is <see langword="null"/>
	/// </exception>
	/// <returns>
	///	    Buffer enabling each enumerator to retrieve elements from the shared source sequence.
	/// </returns>
	/// <remarks>
	/// <para>
	///	    This operator uses deferred execution and streams its result.
	///	</para>
	/// </remarks>
	public static IBuffer<TSource> Share<TSource>(this IEnumerable<TSource> source)
	{
		ArgumentNullException.ThrowIfNull(source);

		return new SharedBuffer<TSource>(source);
	}

	private sealed class SharedBuffer<T>(
		IEnumerable<T> source
	) : IBuffer<T>
	{
		private readonly Lock _lock = new();

		private IEnumerable<T>? _source = source;

		private IEnumerator<T>? _enumerator;
		private bool _initialized;
		private int _version;

		private ExceptionDispatchInfo? _exception;

		private bool _disposed;

		public int Count => 0;

		public void Reset()
		{
			lock (_lock)
			{
				if (_disposed)
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

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
					ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

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
						ThrowHelper.ThrowObjectDisposedException<IBuffer<T>>();

					if (!_initialized
						|| version != _version)
					{
						ThrowHelper.ThrowInvalidOperationException("Buffer reset during iteration.");
					}

					_exception?.Throw();

					if (_enumerator is null)
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
