namespace SuperLinq;

public static partial class SuperEnumerable
{
	private static IEnumerable<TResult> LoopJoin<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector)
	{
		foreach (var l in left)
		{
			var lKey = leftKeySelector(l);
			var flag = false;

			foreach (var r in right)
			{
				var rKey = rightKeySelector(r);
				if (EqualityComparer<TKey>.Default.Equals(lKey, rKey))
				{
					flag = true;
					yield return bothResultSelector(l, r);
				}
			}

			if (joinOperation.HasFlag(JoinOperation.LeftOuter)
				&& !flag)
			{
				yield return leftResultSelector!(l);
			}
		}
	}

	private static IEnumerable<TResult> LoopJoin<TLeft, TRight,TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer)
	{
		foreach (var l in left)
		{
			var lKey = leftKeySelector(l);
			var flag = false;

			foreach (var r in right)
			{
				var rKey = rightKeySelector(r);
				if (comparer.Equals(lKey, rKey))
				{
					flag = true;
					yield return bothResultSelector(l, r);
				}
			}

			if (joinOperation.HasFlag(JoinOperation.LeftOuter)
				&& !flag)
			{
				yield return leftResultSelector!(l);
			}
		}
	}
}
