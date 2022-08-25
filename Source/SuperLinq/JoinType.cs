namespace SuperLinq;

/// <summary>
/// Values defining which type of join to use in join functions.
/// </summary>
public enum JoinType
{
	/// <summary>
	/// Use nested loops over the left and right inputs to execute the join.
	/// </summary>
	Loop,

	/// <summary>
	/// Build a <see cref="ILookup{TKey, TElement}"/> for one side to execute the join.
	/// </summary>
	Hash,

	/// <summary>
	/// Simultaneously enumerates both sequences to execute the join.
	/// </summary>
	/// <remarks>
	/// A merge join assumes that the left and right inputs are already sorted.
	/// If they are not sorted, then the results will be undefined.
	/// </remarks>
	Merge,
}
