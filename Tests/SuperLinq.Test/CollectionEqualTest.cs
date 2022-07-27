namespace Test;

public class CollectionEqualTest
{
	[Fact]
	public void CollectionEqualIntSequenceInOrder()
	{
		Assert.True(Seq(1, 2, 3).CollectionEqual(Seq(1, 2, 3)));
	}

	[Fact]
	public void CollectionEqualIntSequenceOutOfOrder()
	{
		Assert.True(Seq(1, 2, 3).CollectionEqual(Seq(3, 2, 1)));
	}

	[Fact]
	public void CollectionEqualIntSequenceDuplicate()
	{
		Assert.True(Seq(1, 1, 2, 2, 3, 3).CollectionEqual(Seq(1, 1, 2, 2, 3, 3)));
	}

	[Fact]
	public void CollectionEqualIntDifferentSequence()
	{
		Assert.False(Seq(1, 2, 3).CollectionEqual(Seq(1, 2, 5)));
	}

	[Fact]
	public void CollectionEqualIntDifferentDuplicate()
	{
		Assert.False(Seq(1, 1, 2, 2, 3, 3).CollectionEqual(Seq(1, 2, 3)));
	}

	[Fact]
	public void CollectionEqualStringSequenceInOrder()
	{
		Assert.True(Seq("foo", "bar", "qux").CollectionEqual(Seq("foo", "bar", "qux")));
	}

	[Fact]
	public void CollectionEqualStringSequenceOutOfOrder()
	{
		Assert.True(Seq("foo", "bar", "qux").CollectionEqual(Seq("qux", "bar", "foo")));
	}

	[Fact]
	public void CollectionEqualStringSequenceDuplicate()
	{
		Assert.True(Seq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(Seq("foo", "foo", "bar", "bar", "qux", "qux")));
	}

	[Fact]
	public void CollectionEqualStringDifferentSequence()
	{
		Assert.False(Seq("foo", "bar", "qux").CollectionEqual(Seq("foo", "bar", "baz")));
	}

	[Fact]
	public void CollectionEqualStringDifferentDuplicate()
	{
		Assert.False(Seq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(Seq("foo", "bar", "qux")));
	}


	[Fact]
	public void CollectionEqualIntSequenceInOrderComparer()
	{
		Assert.True(Seq(1, 2, 3).CollectionEqual(Seq(-1, -2, -3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntSequenceOutOfOrderComparer()
	{
		Assert.True(Seq(1, 2, 3).CollectionEqual(Seq(-3, -2, -1), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntSequenceDuplicateComparer()
	{
		Assert.True(Seq(1, 1, 2, 2, 3, 3).CollectionEqual(Seq(-1, 1, -2, 2, -3, 3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntDifferentSequenceComparer()
	{
		Assert.False(Seq(1, 2, 3).CollectionEqual(Seq(-1, -2, -5), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntDifferentDuplicateComparer()
	{
		Assert.False(Seq(1, 1, 2, 2, 3, 3).CollectionEqual(Seq(1, 2, 3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualStringSequenceInOrderComparer()
	{
		Assert.True(Seq("foo", "bar", "qux").CollectionEqual(Seq("FOO", "BAR", "QUX"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringSequenceOutOfOrderComparer()
	{
		Assert.True(Seq("foo", "bar", "qux").CollectionEqual(Seq("QUX", "BAR", "FOO"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringSequenceDuplicateComparer()
	{
		Assert.True(Seq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(Seq("foo", "FOO", "BAR", "bar", "QUX", "qux"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringDifferentSequenceComparer()
	{
		Assert.False(Seq("foo", "bar", "qux").CollectionEqual(Seq("FOO", "BAR", "BAZ"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringDifferentDuplicateComparer()
	{
		Assert.False(Seq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(Seq("FOO", "BAR", "QUX"), StringComparer.OrdinalIgnoreCase));
	}
}
