namespace Test;

public class ReturnTest
{
	private static class SomeSingleton
	{
		public static readonly object Item = new();
		public static readonly IEnumerable<object> Sequence = SuperEnumerable.Return(Item);
		public static IList<object> List => (IList<object>)Sequence;
		public static ICollection<object> Collection => (ICollection<object>)Sequence;
	}

	private static class NullSingleton
	{
		public static readonly IEnumerable<object?> Sequence = SuperEnumerable.Return<object?>(null);
		public static IList<object?> List => (IList<object?>)Sequence;
	}

	[Fact]
	public void TestResultingSequenceContainsSingle()
	{
		_ = Assert.Single(SomeSingleton.Sequence);
	}

	[Fact]
	public void TestResultingSequenceContainsTheItemProvided()
	{
		Assert.Contains(SomeSingleton.Item, SomeSingleton.Sequence);
	}

	[Fact]
	public void TestResultingListHasCountOne()
	{
		Assert.Equal(1, SomeSingleton.List.Count);
	}

	[Fact]
	public void TestContainsReturnsTrueWhenTheResultingSequenceContainsTheItemProvided()
	{
		Assert.Contains(SomeSingleton.Item, SomeSingleton.Sequence);
	}

	[Fact]
	public void TestCopyToSetsTheValueAtTheIndexToTheItemContained()
	{
		var first = new object();
		var third = new object();

		var array = new[]
		{
			first,
			new object(),
			third,
		};

		SomeSingleton.List.CopyTo(array, 1);

		Assert.Equal(first, array[0]);
		Assert.Equal(SomeSingleton.Item, array[1]);
		Assert.Equal(third, array[2]);
	}

	[Fact]
	public void TestResultingCollectionIsReadOnly()
	{
		Assert.True(SomeSingleton.Collection.IsReadOnly);
	}

	[Fact]
	public void TestResultingCollectionHasCountOne()
	{
		Assert.Equal(1, SomeSingleton.Collection.Count);
	}

	[Fact]
	public void TestIndexZeroContainsTheItemProvided()
	{
		Assert.Equal(SomeSingleton.Item, SomeSingleton.List[0]);
	}

	[Fact]
	public void TestIndexOfTheItemProvidedIsZero()
	{
		Assert.Equal(0, SomeSingleton.List.IndexOf(SomeSingleton.Item));
	}

	[Fact]
	public void TestIndexOfAnItemNotContainedIsNegativeOne()
	{
		Assert.Equal(-1, SomeSingleton.List.IndexOf(new object()));
	}

	public static IEnumerable<object[]> UnsupportedActions() =>
		[
			[() => SomeSingleton.List.Add(new object()),],
			[() => SomeSingleton.Collection.Clear(),],
			[() => SomeSingleton.Collection.Remove(SomeSingleton.Item),],
			[() => SomeSingleton.List.RemoveAt(0),],
			[() => SomeSingleton.List.Insert(0, new object()),],
			[() => SomeSingleton.List[0] = new object(),],
		];

	[Theory, MemberData(nameof(UnsupportedActions))]
	public void TestUnsupportedMethodShouldThrow(Action unsupportedAction)
	{
		_ = Assert.Throws<NotSupportedException>(unsupportedAction);
	}

	[Fact]
	public void TestIndexingPastZeroShouldThrow()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => SomeSingleton.List[1]);
	}
}
