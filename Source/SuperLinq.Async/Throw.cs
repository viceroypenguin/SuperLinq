namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns a sequence that throws an exception upon enumeration.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="exception">Exception to throw upon enumerating the resulting sequence.</param>
	/// <returns>Sequence that throws the specified exception upon enumeration.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// The provided value (<paramref name="exception"/>) will be thrown when the first element is enumerated. 
	/// </para>
	/// <para>
	/// If the returned <see cref="IAsyncEnumerable{T}"/> is enumerated multiple times, the same value will be thrown each
	/// time.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> Throw<TSource>(Exception exception)
	{
		Guard.IsNotNull(exception);

		return Core(exception);

		static async IAsyncEnumerable<TSource> Core(Exception exception)
		{
			throw exception;
#pragma warning disable CS0162
			await Task.Yield();
			yield break;
#pragma warning restore CS0162
		}
	}
}
