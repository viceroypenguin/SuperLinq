namespace SuperLinq;

public static partial class SuperEnumerable
{
	[Flags]
	private enum JoinOperation
	{
		Inner = 0x0,
		LeftOuter = 0x1,
		RightOuter = 0x2,
		FullOuter = LeftOuter | RightOuter,
	}

	private static IEnumerable<TResult> Join<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector)
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		if (joinOperation.HasFlag(JoinOperation.LeftOuter))
			ArgumentNullException.ThrowIfNull(leftResultSelector);
		if (joinOperation.HasFlag(JoinOperation.RightOuter))
			ArgumentNullException.ThrowIfNull(rightResultSelector);

		ArgumentNullException.ThrowIfNull(bothResultSelector);

		return joinType switch
		{
			JoinType.Loop => LoopJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				bothResultSelector,
				EqualityComparer<TKey>.Default),

			JoinType.Hash => HashJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				rightResultSelector,
				bothResultSelector,
				EqualityComparer<TKey>.Default),

			JoinType.Merge => MergeJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				rightResultSelector,
				bothResultSelector,
				Comparer<TKey>.Default),

			_ => ThrowHelper.ThrowArgumentException<IEnumerable<TResult>>(nameof(joinType), $"Unknown Join Type: {joinType}"),
		};
	}

	private static IEnumerable<TResult> Join<TLeft, TRight, TKey, TComparer, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinType joinType,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		TComparer comparer)
		where TComparer : notnull, IComparer<TKey>, IEqualityComparer<TKey>
	{
		ArgumentNullException.ThrowIfNull(left);
		ArgumentNullException.ThrowIfNull(right);
		ArgumentNullException.ThrowIfNull(leftKeySelector);
		ArgumentNullException.ThrowIfNull(rightKeySelector);

		if (joinOperation.HasFlag(JoinOperation.LeftOuter))
			ArgumentNullException.ThrowIfNull(leftResultSelector);
		if (joinOperation.HasFlag(JoinOperation.RightOuter))
			ArgumentNullException.ThrowIfNull(rightResultSelector);

		ArgumentNullException.ThrowIfNull(bothResultSelector);
		ArgumentNullException.ThrowIfNull(comparer);

		return joinType switch
		{
			JoinType.Loop => LoopJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				bothResultSelector,
				comparer),

			JoinType.Hash => HashJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				rightResultSelector,
				bothResultSelector,
				comparer),

			JoinType.Merge => MergeJoin(
				left,
				right,
				joinOperation,
				leftKeySelector,
				rightKeySelector,
				leftResultSelector,
				rightResultSelector,
				bothResultSelector,
				comparer),

			_ => ThrowHelper.ThrowArgumentException<IEnumerable<TResult>>(nameof(joinType), $"Unknown Join Type: {joinType}"),
		};
	}
}
