namespace Test;

public class CollectionEqualTest
{
	[Fact]
	public void CollectionEqualIntSequenceInOrder()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(1, 2, 3).AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualIntSequenceOutOfOrder()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(3, 2, 1).AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualIntSequenceDuplicate()
	{
		using var xs = Seq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		using var ys = Seq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualIntDifferentSequence()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(1, 2, 5).AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualIntDifferentDuplicate()
	{
		using var xs = Seq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		using var ys = Seq(1, 2, 3).AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualStringSequenceInOrder()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("foo", "bar", "qux").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualStringSequenceOutOfOrder()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("qux", "bar", "foo").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualStringSequenceDuplicate()
	{
		using var xs = Seq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		using var ys = Seq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualStringDifferentSequence()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("foo", "bar", "baz").AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys));
	}

	[Fact]
	public void CollectionEqualStringDifferentDuplicate()
	{
		using var xs = Seq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		using var ys = Seq("foo", "bar", "qux").AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys));
	}


	[Fact]
	public void CollectionEqualIntSequenceInOrderComparer()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(-1, -2, -3).AsTestingSequence();
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntSequenceOutOfOrderComparer()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(-3, -2, -1).AsTestingSequence();
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntSequenceDuplicateComparer()
	{
		using var xs = Seq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		using var ys = Seq(-1, 1, -2, 2, -3, 3).AsTestingSequence();
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntDifferentSequenceComparer()
	{
		using var xs = Seq(1, 2, 3).AsTestingSequence();
		using var ys = Seq(-1, -2, -5).AsTestingSequence();
		Assert.False(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualIntDifferentDuplicateComparer()
	{
		using var xs = Seq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		using var ys = Seq(1, 2, 3).AsTestingSequence();
		Assert.False(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public void CollectionEqualStringSequenceInOrderComparer()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("FOO", "BAR", "QUX").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringSequenceOutOfOrderComparer()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("QUX", "BAR", "FOO").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringSequenceDuplicateComparer()
	{
		using var xs = Seq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		using var ys = Seq("foo", "FOO", "BAR", "bar", "QUX", "qux").AsTestingSequence();
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringDifferentSequenceComparer()
	{
		using var xs = Seq("foo", "bar", "qux").AsTestingSequence();
		using var ys = Seq("FOO", "BAR", "BAZ").AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public void CollectionEqualStringDifferentDuplicateComparer()
	{
		using var xs = Seq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		using var ys = Seq("FOO", "BAR", "QUX").AsTestingSequence();
		Assert.False(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}
}
