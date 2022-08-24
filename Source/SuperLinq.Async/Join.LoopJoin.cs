namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	private static async IAsyncEnumerable<TResult> LoopJoin<TLeft, TRight,TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var rList = await right.ToListAsync(cancellationToken).ConfigureAwait(false);
		await foreach (var l in left.WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			var lKey = leftKeySelector(l);
			var flag = false;

			foreach (var r in rList)
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
