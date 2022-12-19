namespace Test.Async;

public class CollectionEqualTest
{
	[Fact]
	public async Task CollectionEqualIntSequenceInOrder()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(1, 2, 3).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrder()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(3, 2, 1).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicate()
	{
		await using var xs = AsyncSeq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		await using var ys = AsyncSeq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequence()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(1, 2, 5).AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicate()
	{
		await using var xs = AsyncSeq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		await using var ys = AsyncSeq(1, 2, 3).AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrder()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrder()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("qux", "bar", "foo").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicate()
	{
		await using var xs = AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequence()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("foo", "bar", "baz").AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicate()
	{
		await using var xs = AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys));
	}


	[Fact]
	public async Task CollectionEqualIntSequenceInOrderComparer()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(-1, -2, -3).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrderComparer()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(-3, -2, -1).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicateComparer()
	{
		await using var xs = AsyncSeq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		await using var ys = AsyncSeq(-1, 1, -2, 2, -3, 3).AsTestingSequence();
		Assert.True(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequenceComparer()
	{
		await using var xs = AsyncSeq(1, 2, 3).AsTestingSequence();
		await using var ys = AsyncSeq(-1, -2, -5).AsTestingSequence();
		Assert.False(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicateComparer()
	{
		await using var xs = AsyncSeq(1, 1, 2, 2, 3, 3).AsTestingSequence();
		await using var ys = AsyncSeq(1, 2, 3).AsTestingSequence();
		Assert.False(await xs.CollectionEqual(
			ys, EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrderComparer()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("FOO", "BAR", "QUX").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrderComparer()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("QUX", "BAR", "FOO").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicateComparer()
	{
		await using var xs = AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("foo", "FOO", "BAR", "bar", "QUX", "qux").AsTestingSequence();
		Assert.True(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequenceComparer()
	{
		await using var xs = AsyncSeq("foo", "bar", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("FOO", "BAR", "BAZ").AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicateComparer()
	{
		await using var xs = AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").AsTestingSequence();
		await using var ys = AsyncSeq("FOO", "BAR", "QUX").AsTestingSequence();
		Assert.False(await xs.CollectionEqual(ys, StringComparer.OrdinalIgnoreCase));
	}
}
