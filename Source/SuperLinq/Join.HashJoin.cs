namespace SuperLinq;

public static partial class SuperEnumerable
{
	private static IEnumerable<TResult> HashJoin<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer)
	{
		var rLookup = right.ToLookup(rightKeySelector, comparer);
		foreach (var result in HashJoin(
			left, rLookup, joinOperation,
			leftKeySelector, rightKeySelector,
			leftResultSelector, rightResultSelector,
			bothResultSelector,
			comparer))
		{
			yield return result;
		}
	}

	private static IEnumerable<TResult> HashJoin<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		ILookup<TKey, TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer)
	{
		var used = new HashSet<TKey>(comparer);
		foreach (var l in left)
		{
			var lKey = leftKeySelector(l);

			if (!right.Contains(lKey))
			{
				if (joinOperation.HasFlag(JoinOperation.LeftOuter))
					yield return Debug.AssertNotNull(leftResultSelector)(l);
				continue;
			}

			_ = used.Add(lKey);
			foreach (var r in right[lKey])
				yield return bothResultSelector(l, r);
		}

		if (joinOperation.HasFlag(JoinOperation.RightOuter))
		{
			foreach (var g in right)
			{
				if (used.Contains(g.Key))
					continue;

				foreach (var r in g)
					yield return Debug.AssertNotNull(rightResultSelector)(r);
			}
		}
	}
}
