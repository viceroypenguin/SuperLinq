﻿using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	private sealed class ComparerEqualityComparer<TKey> : IEqualityComparer<TKey>
	{
		private readonly IComparer<TKey> _comparer;

		public ComparerEqualityComparer(IComparer<TKey> comparer)
		{
			_comparer = comparer;
		}

		public bool Equals([AllowNull] TKey x, [AllowNull] TKey y) => _comparer.Compare(x, y) == 0;
		public int GetHashCode([DisallowNull] TKey obj) => throw new NotSupportedException();
	}

	private static async IAsyncEnumerable<TResult> MergeJoin<TLeft, TRight, TKey, TResult>(
		IAsyncEnumerable<TLeft> left,
		IAsyncEnumerable<TRight> right,
		JoinOperation joinOperation,
		Func<TLeft, TKey> leftKeySelector,
		Func<TRight, TKey> rightKeySelector,
		Func<TLeft, TResult>? leftResultSelector,
		Func<TRight, TResult>? rightResultSelector,
		Func<TLeft, TRight, TResult> bothResultSelector,
		IComparer<TKey> comparer,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await using var e1 = left.GroupAdjacent(leftKeySelector, new ComparerEqualityComparer<TKey>(comparer)).GetConfiguredAsyncEnumerator(cancellationToken);
		await using var e2 = right.GroupAdjacent(rightKeySelector, new ComparerEqualityComparer<TKey>(comparer)).GetConfiguredAsyncEnumerator(cancellationToken);

		var gotLeft = await e1.MoveNextAsync();
		var gotRight = await e2.MoveNextAsync();

		while (gotLeft && gotRight)
		{
			var l = e1.Current;
			var r = e2.Current;
			var comparison = comparer.Compare(l.Key, r.Key);

			if (comparison < 0)
			{
				if (joinOperation.HasFlag(JoinOperation.LeftOuter))
				{
					foreach (var e in l)
						yield return Debug.AssertNotNull(leftResultSelector)(e);
				}

				gotLeft = await e1.MoveNextAsync();
			}
			else if (comparison > 0)
			{
				if (joinOperation.HasFlag(JoinOperation.RightOuter))
				{
					foreach (var e in r)
						yield return Debug.AssertNotNull(rightResultSelector)(e);
				}

				gotRight = await e2.MoveNextAsync();
			}
			else
			{
				foreach (var el in l)
				{
					foreach (var er in r)
						yield return bothResultSelector(el, er);
				}

				gotLeft = await e1.MoveNextAsync();
				gotRight = await e2.MoveNextAsync();
			}
		}

		if (gotLeft && joinOperation.HasFlag(JoinOperation.LeftOuter))
		{
			do
			{
				foreach (var e in e1.Current)
					yield return Debug.AssertNotNull(leftResultSelector)(e);
			} while (await e1.MoveNextAsync());
			yield break;
		}

		if (gotRight && joinOperation.HasFlag(JoinOperation.RightOuter))
		{
			do
			{
				foreach (var e in e2.Current)
					yield return Debug.AssertNotNull(rightResultSelector)(e);
			} while (await e2.MoveNextAsync());
			yield break;
		}
	}
}
