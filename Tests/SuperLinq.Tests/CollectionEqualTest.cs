namespace SuperLinq.Tests;

public sealed class CollectionEqualTest
{
	[Test]
	public void CollectionEqualIntSequenceInOrder()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(1, 2, 3);
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualIntSequenceOutOfOrder()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(3, 2, 1);
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualIntSequenceDuplicate()
	{
		using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		using var ys = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualIntDifferentSequence()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(1, 2, 5);
		Assert.False(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualIntDifferentDuplicate()
	{
		using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		using var ys = TestingSequence.Of(1, 2, 3);
		Assert.False(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualStringSequenceInOrder()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("foo", "bar", "qux");
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualStringSequenceOutOfOrder()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("qux", "bar", "foo");
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualStringSequenceDuplicate()
	{
		using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		using var ys = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		Assert.True(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualStringDifferentSequence()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("foo", "bar", "baz");
		Assert.False(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualStringDifferentDuplicate()
	{
		using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		using var ys = TestingSequence.Of("foo", "bar", "qux");
		Assert.False(xs.CollectionEqual(ys));
	}

	[Test]
	public void CollectionEqualIntSequenceInOrderComparer()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(-1, -2, -3);
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Test]
	public void CollectionEqualIntSequenceOutOfOrderComparer()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(-3, -2, -1);
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Test]
	public void CollectionEqualIntSequenceDuplicateComparer()
	{
		using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		using var ys = TestingSequence.Of(-1, 1, -2, 2, -3, 3);
		Assert.True(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Test]
	public void CollectionEqualIntDifferentSequenceComparer()
	{
		using var xs = TestingSequence.Of(1, 2, 3);
		using var ys = TestingSequence.Of(-1, -2, -5);
		Assert.False(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Test]
	public void CollectionEqualIntDifferentDuplicateComparer()
	{
		using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		using var ys = TestingSequence.Of(1, 2, 3);
		Assert.False(xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Test]
	public void CollectionEqualStringSequenceInOrderComparer()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("FOO", "BAR", "QUX");
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Test]
	public void CollectionEqualStringSequenceOutOfOrderComparer()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("QUX", "BAR", "FOO");
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Test]
	public void CollectionEqualStringSequenceDuplicateComparer()
	{
		using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		using var ys = TestingSequence.Of("foo", "FOO", "BAR", "bar", "QUX", "qux");
		Assert.True(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Test]
	public void CollectionEqualStringDifferentSequenceComparer()
	{
		using var xs = TestingSequence.Of("foo", "bar", "qux");
		using var ys = TestingSequence.Of("FOO", "BAR", "BAZ");
		Assert.False(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Test]
	public void CollectionEqualStringDifferentDuplicateComparer()
	{
		using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		using var ys = TestingSequence.Of("FOO", "BAR", "QUX");
		Assert.False(xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}
}
