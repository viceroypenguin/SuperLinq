using System.Diagnostics.CodeAnalysis;

namespace Test;

public class ValueTupleEqualityComparerTest
{
	private record TestObject(string Value);
	private class TestComparer<T>(Func<T?, T?, bool> comparer) : IEqualityComparer<T>
	{
		public bool Equals(T? x, T? y) => comparer(x, y);
		public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
	}

	[Fact]
	public void ValueTupleEqualityComparerWithOneTypeArgShouldCreateWhenNoComparerProvided()
	{
		var comparer = ValueTupleEqualityComparer.Create<int>(null);
		ValueTuple<int> left = new(1);
		ValueTuple<int> right = new(1);
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}

	[Fact]
	public void ValueTupleEqualityComparerWithOneTypeArgShouldCreateWhenComparerProvided()
	{
		var innerComparer = new TestComparer<TestObject>((x, y) => x?.Value == y?.Value);
		var comparer = ValueTupleEqualityComparer.Create(innerComparer);
		ValueTuple<TestObject> left = new(new("testing"));
		ValueTuple<TestObject> right = new(new("testing"));
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}

	[Fact]
	public void ValueTupleEqualityComparerWithTwoTypeArgsShouldCreateWhenNoComparerProvided()
	{
		var comparer = ValueTupleEqualityComparer.Create<int, int>(null, null);
		ValueTuple<int, int> left = new(1, 2);
		ValueTuple<int, int> right = new(1, 2);
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}
}
