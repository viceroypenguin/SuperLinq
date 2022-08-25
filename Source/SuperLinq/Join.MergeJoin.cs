using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public static partial class SuperEnumerable
{
	private class ComparerEqualityComparer<TKey> : IEqualityComparer<TKey>
	{
		private readonly IComparer<TKey> _comparer;

		public ComparerEqualityComparer(IComparer<TKey> comparer)
		{
			_comparer = comparer;
		}

		public bool Equals([AllowNull] TKey x, [AllowNull] TKey y) => _comparer.Compare(x, y) == 0;
		public int GetHashCode([DisallowNull] TKey obj) => throw new NotSupportedException();
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
		using var e1 = left.GroupAdjacent(leftKeySelector, new ComparerEqualityComparer<TKey>(comparer)).GetEnumerator();
		using var e2 = right.GroupAdjacent(rightKeySelector, new ComparerEqualityComparer<TKey>(comparer)).GetEnumerator();

		var gotLeft = e1.MoveNext();
		var gotRight = e2.MoveNext();

		while (gotLeft && gotRight)
		{
			var l = e1.Current;
			var r = e2.Current;
			var comparison = comparer.Compare(l.Key, r.Key);

			if (comparison < 0)
			{
				if (joinOperation.HasFlag(JoinOperation.LeftOuter))
					foreach (var e in l)
						yield return leftResultSelector!(e);
				gotLeft = e1.MoveNext();
			}
			else if (comparison > 0)
			{
				if (joinOperation.HasFlag(JoinOperation.RightOuter))
					foreach (var e in r)
						yield return rightResultSelector!(e);
				gotRight = e2.MoveNext();
			}
			else
			{
				foreach (var el in l)
					foreach (var er in r)
						yield return bothResultSelector(el, er);
				gotLeft = e1.MoveNext();
				gotRight = e2.MoveNext();
			}
		}

		if (gotLeft && joinOperation.HasFlag(JoinOperation.LeftOuter))
		{
			do
			{
				foreach (var e in e1.Current)
					yield return leftResultSelector!(e);
			} while (e1.MoveNext());
			yield break;
		}

		if (gotRight && joinOperation.HasFlag(JoinOperation.RightOuter))
		{
			do
			{
				foreach (var e in e2.Current)
					yield return rightResultSelector!(e);
			} while (e2.MoveNext());
			yield break;
		}
	}
}
