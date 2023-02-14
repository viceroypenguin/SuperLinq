namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	private static async IAsyncEnumerable<TResult> HashJoin<TLeft, TRight, TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var rLookup = await right.ToLookupAsync(rightKeySelector, comparer, cancellationToken).ConfigureAwait(false);
		await foreach (var result in HashJoin(
			left, rLookup, joinOperation,
			leftKeySelector,
			leftResultSelector, rightResultSelector,
			bothResultSelector,
			comparer, cancellationToken).WithCancellation(cancellationToken).ConfigureAwait(false))
		{
			yield return result;
		}
	}

	private static async IAsyncEnumerable<TResult> HashJoin<TLeft, TRight, TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		ILookup<TKey, TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IEqualityComparer<TKey> comparer,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var used = new HashSet<TKey>(comparer);
		await foreach (var l in left.WithCancellation(cancellationToken).ConfigureAwait(false))
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
