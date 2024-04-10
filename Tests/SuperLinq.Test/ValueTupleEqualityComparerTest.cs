using System.Diagnostics.CodeAnalysis;

namespace Test;

public sealed class ValueTupleEqualityComparerTest
{
	private sealed record TestObject(string Value);
	private sealed class TestComparer<T>(Func<T?, T?, bool> comparer) : IEqualityComparer<T>
	{
		public bool Equals(T? x, T? y) => comparer(x, y);
		public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
	}

	[Fact]
	public void ValueTupleEqualityComparerWithOneTypeArgShouldCreateWhenNoComparerProvided()
	{
		var comparer = ValueTupleEqualityComparer.Create<int>(comparer1: null);
		ValueTuple<int> left = new(1);
		ValueTuple<int> right = new(1);
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}

	[Fact]
	public void ValueTupleEqualityComparerWithOneTypeArgShouldGetHashCode()
	{
		var comparer = ValueTupleEqualityComparer.Create<int?>(comparer1: null);
		ValueTuple<int?> first = new(item1: null);
		var firstHashCode = comparer.GetHashCode(first);
		Assert.Equal(0, firstHashCode);

		ValueTuple<int?> second = new(2);
		var secondHashCode = comparer.GetHashCode(second);
		Assert.Equal(2.GetHashCode(), secondHashCode);
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
		var comparer = ValueTupleEqualityComparer.Create<int, int>(comparer1: null, comparer2: null);
		ValueTuple<int, int> left = new(1, 2);
		ValueTuple<int, int> right = new(1, 2);
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}

	[Fact]
	public void ValueTupleEqualityComparerWithTwoTypeArgsShouldCreateWhenComparerProvided()
	{
		var innerComparerLeft = new TestComparer<TestObject>((x, y) => x?.Value == y?.Value);
		var innerComparerRight = new TestComparer<TestObject>((x, y) => x?.Value == y?.Value);
		var comparer = ValueTupleEqualityComparer.Create(innerComparerLeft, innerComparerRight);
		ValueTuple<TestObject, TestObject> left = new(new("1"), new("2"));
		ValueTuple<TestObject, TestObject> right = new(new("1"), new("2"));
		var result = comparer.Equals(left, right);
		Assert.True(result);
	}

	[Fact]
	public void ValueTupleEqualityComparerWithTwoTypeArgsShouldGetHashCode()
	{
		var comparer = ValueTupleEqualityComparer.Create<int, int>(comparer1: null, comparer2: null);
		ValueTuple<int, int> first = new(1, 2);
		var firstHashCode = comparer.GetHashCode(first);
		var expectedHashCode = HashCode.Combine(1.GetHashCode(), 2.GetHashCode());
		Assert.Equal(expectedHashCode, firstHashCode);
	}
}
