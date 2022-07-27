namespace Test.Async;

public class CollectionEqualTest
{
	[Fact]
	public async Task CollectionEqualIntSequenceInOrder()
	{
		Assert.True(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(1, 2, 3)));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrder()
	{
		Assert.True(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(3, 2, 1)));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicate()
	{
		Assert.True(await AsyncSeq(1, 1, 2, 2, 3, 3).CollectionEqual(AsyncSeq(1, 1, 2, 2, 3, 3)));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequence()
	{
		Assert.False(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(1, 2, 5)));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicate()
	{
		Assert.False(await AsyncSeq(1, 1, 2, 2, 3, 3).CollectionEqual(AsyncSeq(1, 2, 3)));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrder()
	{
		Assert.True(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("foo", "bar", "qux")));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrder()
	{
		Assert.True(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("qux", "bar", "foo")));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicate()
	{
		Assert.True(await AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux")));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequence()
	{
		Assert.False(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("foo", "bar", "baz")));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicate()
	{
		Assert.False(await AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(AsyncSeq("foo", "bar", "qux")));
	}


	[Fact]
	public async Task CollectionEqualIntSequenceInOrderComparer()
	{
		Assert.True(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(-1, -2, -3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceOutOfOrderComparer()
	{
		Assert.True(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(-3, -2, -1), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntSequenceDuplicateComparer()
	{
		Assert.True(await AsyncSeq(1, 1, 2, 2, 3, 3).CollectionEqual(AsyncSeq(-1, 1, -2, 2, -3, 3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentSequenceComparer()
	{
		Assert.False(await AsyncSeq(1, 2, 3).CollectionEqual(AsyncSeq(-1, -2, -5), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualIntDifferentDuplicateComparer()
	{
		Assert.False(await AsyncSeq(1, 1, 2, 2, 3, 3).CollectionEqual(AsyncSeq(1, 2, 3), EqualityComparer.Create<int>((a, b) => Math.Abs(a) == Math.Abs(b), x => Math.Abs(x).GetHashCode())));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceInOrderComparer()
	{
		Assert.True(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("FOO", "BAR", "QUX"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceOutOfOrderComparer()
	{
		Assert.True(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("QUX", "BAR", "FOO"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringSequenceDuplicateComparer()
	{
		Assert.True(await AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(AsyncSeq("foo", "FOO", "BAR", "bar", "QUX", "qux"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentSequenceComparer()
	{
		Assert.False(await AsyncSeq("foo", "bar", "qux").CollectionEqual(AsyncSeq("FOO", "BAR", "BAZ"), StringComparer.OrdinalIgnoreCase));
	}

	[Fact]
	public async Task CollectionEqualStringDifferentDuplicateComparer()
	{
		Assert.False(await AsyncSeq("foo", "foo", "bar", "bar", "qux", "qux").CollectionEqual(AsyncSeq("FOO", "BAR", "QUX"), StringComparer.OrdinalIgnoreCase));
	}
}
