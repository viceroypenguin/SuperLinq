using System.Diagnostics.CodeAnalysis;

namespace Test;

public class ValueTupleComparerTest
{
	private class TestComparer : IComparer<string>
	{
		public int Compare([AllowNull] string x, [AllowNull] string y)
		{
			var xLen = x?.Length ?? 0;
			var yLen = y?.Length ?? 0;
			return xLen.CompareTo(yLen);
		}
	}

	[Fact]
	public void ValueTupleComparerShouldCreateWithDefaultComparers()
	{
		var comparer = ValueTupleComparer.Create<int, int>(null, null);
		ValueTuple<int, int> left = new(1, 2);
		ValueTuple<int, int> right = new(3, 4);
		var result = comparer.Compare(left, right);
		Assert.Equal(-1, result);
	}

	[Fact]
	public void ValueTupleComparerShouldCheckSecondItemIfFirstIsZero()
	{
		var comparer = ValueTupleComparer.Create<int, int>(null, null);
		ValueTuple<int, int> left = new(1, 3);
		ValueTuple<int, int> right = new(1, 2);
		var result = comparer.Compare(left, right);
		Assert.Equal(1, result);
	}

	[Fact]
	public void ValueTupleComparerShouldAcceptCustomComparers()
	{
		TestComparer innerLeftComparer = new();
		TestComparer innerRightComparer = new();
		var comparer = ValueTupleComparer.Create(innerLeftComparer, innerRightComparer);
		ValueTuple<string, string> left = new("123", "1");
		ValueTuple<string, string> right = new("123", "123");
		var result = comparer.Compare(left, right);
		Assert.Equal(-1, result);
	}
}
