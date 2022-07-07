namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Returns the identity function for a given type.
	/// </summary>
	/// <typeparam name="T">The type of identity function</typeparam>
	/// <returns>A reference to the identity function</returns>
	public static Func<T, T> Identity<T>() =>
		static x => x;
}
