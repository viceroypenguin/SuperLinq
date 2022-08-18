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
	/// Sort both inputs and simultaneously enumerate both to execute the join.
	/// </summary>
	Merge,
}
