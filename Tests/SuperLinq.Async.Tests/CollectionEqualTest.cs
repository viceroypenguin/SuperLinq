namespace SuperLinq.Async.Tests;

public sealed class CollectionEqualTest
{
	[Fact]
	public async Task CollectionEqualIntSequenceInOrder()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(1, 2, 3);
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrder()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(3, 2, 1);
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicate()
	{
		await using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		await using var ys = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequence()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(1, 2, 5);
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicate()
	{
		await using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		await using var ys = TestingSequence.Of(1, 2, 3);
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrder()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("foo", "bar", "qux");
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrder()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("qux", "bar", "foo");
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicate()
	{
		await using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		await using var ys = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequence()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("foo", "bar", "baz");
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicate()
	{
		await using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		await using var ys = TestingSequence.Of("foo", "bar", "qux");
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceInOrderComparer()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(-1, -2, -3);
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrderComparer()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(-3, -2, -1);
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicateComparer()
	{
		await using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		await using var ys = TestingSequence.Of(-1, 1, -2, 2, -3, 3);
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequenceComparer()
	{
		await using var xs = TestingSequence.Of(1, 2, 3);
		await using var ys = TestingSequence.Of(-1, -2, -5);
		Assert.False(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicateComparer()
	{
		await using var xs = TestingSequence.Of(1, 1, 2, 2, 3, 3);
		await using var ys = TestingSequence.Of(1, 2, 3);
		Assert.False(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrderComparer()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("FOO", "BAR", "QUX");
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrderComparer()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("QUX", "BAR", "FOO");
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicateComparer()
	{
		await using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		await using var ys = TestingSequence.Of("foo", "FOO", "BAR", "bar", "QUX", "qux");
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequenceComparer()
	{
		await using var xs = TestingSequence.Of("foo", "bar", "qux");
		await using var ys = TestingSequence.Of("FOO", "BAR", "BAZ");
		Assert.False(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicateComparer()
	{
		await using var xs = TestingSequence.Of("foo", "foo", "bar", "bar", "qux", "qux");
		await using var ys = TestingSequence.Of("FOO", "BAR", "QUX");
		Assert.False(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}
}
