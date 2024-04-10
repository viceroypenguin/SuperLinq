namespace Test;

public sealed class ReturnTest
{
	private static class SomeSingleton
	{
		public static object Item { get; } = new();
		public static IEnumerable<object> Sequence { get; } = SuperEnumerable.Return(Item);
		public static IList<object> List => (IList<object>)Sequence;
		public static ICollection<object> Collection => (ICollection<object>)Sequence;
	}

	private static class NullSingleton
	{
		private static readonly IEnumerable<object?> s_sequence = SuperEnumerable.Return<object?>(item: null);
		public static IList<object?> List => (IList<object?>)s_sequence;
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
		_ = Assert.Single(SomeSingleton.List);
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
		_ = Assert.Single(SomeSingleton.Collection);
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

#pragma warning disable IDE0200 // Remove unnecessary lambda expression
#pragma warning disable IDE0300 // Collection initialization can be simplified
	public static IEnumerable<object[]> UnsupportedActions() =>
		new Action[][]
		{
			[() => SomeSingleton.List.Add(new object()),],
			[() => SomeSingleton.Collection.Clear(),],
			[() => SomeSingleton.Collection.Remove(SomeSingleton.Item),],
			[() => SomeSingleton.List.RemoveAt(0),],
			[() => SomeSingleton.List.Insert(0, new object()),],
			[() => SomeSingleton.List[0] = new object(),],
		};
#pragma warning restore IDE0300 // Collection initialization can be simplified
#pragma warning restore IDE0200 // Remove unnecessary lambda expression

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
