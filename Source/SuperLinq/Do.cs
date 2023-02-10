namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	/// Lazily invokes an action for each value in the sequence.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional
	/// termination.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="source">Source sequence.</param>
	/// <param name="onNext">Action to invoke for each element.</param>
	/// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
	/// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
	/// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
	public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
	{
		throw new NotImplementedException();
	}
}
