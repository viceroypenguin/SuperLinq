namespace Test;

public class ValueTupleComparerTest
{
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
}
