﻿namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="onNext">
	///	    Action to invoke for each element.
	/// </param>
	/// <returns>
	///	    Sequence exhibiting the specified side-effects upon enumeration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/> or <paramref name="onNext"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
	{
		return Do(source, onNext, onCompleted: delegate { });
	}

	/// <summary>
	///	    Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="onNext">
	///	    Action to invoke for each element.
	/// </param>
	/// <param name="onCompleted">
	///	    Action to invoke on successful termination of the sequence.
	/// </param>
	/// <returns>
	///	    Sequence exhibiting the specified side-effects upon enumeration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="onNext"/>, or <paramref name="onCompleted"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	///	    This method uses deferred execution and streams its results.
	/// </remarks>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return DoCore(source, onNext, onCompleted);
	}

	private static IEnumerable<TSource> DoCore<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
	{
		foreach (var el in source)
		{
			onNext(el);
			yield return el;
		}
		onCompleted();
	}

	/// <summary>
	///	    Lazily invokes an action for each value in the sequence, and executes an action upon exceptional
	///     termination.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="onNext">
	///	    Action to invoke for each element.
	/// </param>
	/// <param name="onError">
	///	    Action to invoke on exceptional termination of the sequence.
	/// </param>
	/// <returns>
	///	    Sequence exhibiting the specified side-effects upon enumeration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="onNext"/>, or <paramref name="onError"/> is <see
	///     langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The exception is caught and passed to <paramref name="onNext"/>, and then it is re-thrown. Appropriate
	///     error-handling is still required in order to safely consume the sequence.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
	{
		return Do(source, onNext, onError, onCompleted: delegate { });
	}

	/// <summary>
	///	    Lazily invokes an action for each value in the sequence, and executes an action upon successful or
	///     exceptional termination.
	/// </summary>
	/// <typeparam name="TSource">
	///	    Source sequence element type.
	/// </typeparam>
	/// <param name="source">
	///	    Source sequence.
	/// </param>
	/// <param name="onNext">
	///	    Action to invoke for each element.
	/// </param>
	/// <param name="onError">
	///	    Action to invoke on exceptional termination of the sequence.
	/// </param>
	/// <param name="onCompleted">
	///	    Action to invoke on successful termination of the sequence.
	/// </param>
	/// <returns>
	///	    Sequence exhibiting the specified side-effects upon enumeration.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="source"/>, <paramref name="onNext"/> <paramref name="onError"/>, or <paramref
	///     name="onCompleted"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    The exception is caught and passed to <paramref name="onNext"/>, and then it is re-thrown. Appropriate
	///     error-handling is still required in order to safely consume the sequence.
	/// </para>
	/// <para>
	///	    This method uses deferred execution and streams its results.
	/// </para>
	/// </remarks>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(onNext);
		ArgumentNullException.ThrowIfNull(onError);
		ArgumentNullException.ThrowIfNull(onCompleted);

		return DoCore(source, onNext, onError, onCompleted);
	}

	private static IEnumerable<TSource> DoCore<TSource>(IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
	{
		using var iter = source.GetEnumerator();
		while (true)
		{
			try
			{
				if (!iter.MoveNext())
					break;
			}
			catch (Exception ex)
			{
				onError(ex);
				throw;
			}

			var current = iter.Current;
			onNext(current);
			yield return current;
		}
		onCompleted();
	}

}
