namespace SuperLinq;

public static partial class SuperEnumerable
{
	// This extension method was developed (primarily) to support the
	// implementation of the Permutations() extension methods.

	/// <summary>
	/// Produces a sequence from an action based on the dynamic generation of N nested loops
	/// whose iteration counts are defined by a sequence of loop counts.
	/// </summary>
	/// <param name="action">Action delegate for which to produce a nested loop sequence</param>
	/// <param name="loopCounts">A sequence of loop repetition counts</param>
	/// <returns>A sequence of Action representing the expansion of a set of nested loops</returns>

	static IEnumerable<Action> NestedLoops(this Action action, IEnumerable<int> loopCounts)
	{
		var count = loopCounts.Aggregate((acc, x) => acc * x);

		for (var i = 0; i < count; i++)
			yield return action;
	}
}
