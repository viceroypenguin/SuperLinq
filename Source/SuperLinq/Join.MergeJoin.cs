namespace SuperLinq;

public static partial class SuperEnumerable
{
	private static IEnumerable<TResult> MergeJoin<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector)
	{
		using var e1 = left.OrderBy(leftKeySelector).GetEnumerator();
		using var e2 = right.OrderBy(rightKeySelector).GetEnumerator();

		var gotLeft = e1.MoveNext();
		var gotRight = e2.MoveNext();

		while (gotLeft && gotRight)
		{
			var l = e1.Current;
			var lKey = leftKeySelector(l);
			var r = e2.Current;
			var rKey = rightKeySelector(r);
			var comparison = Comparer<TKey>.Default.Compare(lKey, rKey);

			if (comparison < 0)
			{
				if (joinOperation.HasFlag(JoinOperation.LeftOuter))
					yield return leftResultSelector!(l);
				gotLeft = e1.MoveNext();
			}
			else if (comparison > 0)
			{
				if (joinOperation.HasFlag(JoinOperation.RightOuter))
					yield return rightResultSelector!(r);
				gotRight = e2.MoveNext();
			}
			else
			{
				yield return bothResultSelector(l, r);
				gotLeft = e1.MoveNext();
				gotRight = e2.MoveNext();
			}
		}

		if (gotLeft && joinOperation.HasFlag(JoinOperation.LeftOuter))
		{
			do
			{
				yield return leftResultSelector!(e1.Current);
			} while (e1.MoveNext());
			yield break;
		}

		if (gotRight && joinOperation.HasFlag(JoinOperation.RightOuter))
		{
			do
			{
				yield return rightResultSelector!(e2.Current);
			} while (e2.MoveNext());
			yield break;
		}
	}

	private static IEnumerable<TResult> MergeJoin<TLeft, TRight, TKey, TResult>(
		IEnumerable<TLeft> left,
		IEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey> comparer)
	{
		using var e1 = left.OrderBy(leftKeySelector, comparer).GetEnumerator();
		using var e2 = right.OrderBy(rightKeySelector, comparer).GetEnumerator();

		var gotLeft = e1.MoveNext();
		var gotRight = e2.MoveNext();

		while (gotLeft && gotRight)
		{
			var l = e1.Current;
			var lKey = leftKeySelector(l);
			var r = e2.Current;
			var rKey = rightKeySelector(r);
			var comparison = comparer.Compare(lKey, rKey);

			if (comparison < 0)
			{
				if (joinOperation.HasFlag(JoinOperation.LeftOuter))
					yield return leftResultSelector!(l);
				gotLeft = e1.MoveNext();
			}
			else if (comparison > 0)
			{
				if (joinOperation.HasFlag(JoinOperation.RightOuter))
					yield return rightResultSelector!(r);
				gotRight = e2.MoveNext();
			}
			else
			{
				yield return bothResultSelector(l, r);
				gotLeft = e1.MoveNext();
				gotRight = e2.MoveNext();
			}
		}

		if (gotLeft && joinOperation.HasFlag(JoinOperation.LeftOuter))
		{
			do
			{
				yield return leftResultSelector!(e1.Current);
			} while (e1.MoveNext());
			yield break;
		}

		if (gotRight && joinOperation.HasFlag(JoinOperation.RightOuter))
		{
			do
			{
				yield return rightResultSelector!(e2.Current);
			} while (e2.MoveNext());
			yield break;
		}
	}
}
