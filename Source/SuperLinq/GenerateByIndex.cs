namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Returns a sequence of values based on indexes.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the value returned by <paramref name="generator"/>
	/// and therefore the elements of the generated sequence.</typeparam>
	/// <param name="generator">
	/// Generation function to apply to each index.</param>
	/// <returns>A sequence of generated results.</returns>
	/// <remarks>
	/// <para>
	/// The sequence is (practically) infinite where the index ranges from
	/// zero to <see cref="int.MaxValue"/> inclusive.</para>
	/// <para>
	/// This function defers execution and streams the results.</para>
	/// </remarks>

	public static IEnumerable<TResult> GenerateByIndex<TResult>(Func<int, TResult> generator)
	{
		generator.ThrowIfNull();

		return SuperEnumerable.Sequence(0, int.MaxValue)
							 .Select(generator);
	}
}
