namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns the identity function for a given type.
	/// </summary>
	/// <typeparam name="T">The type of identity function</typeparam>
	/// <returns>A reference to the identity function</returns>
	public static T Identity<T>(T x) => x;
}
