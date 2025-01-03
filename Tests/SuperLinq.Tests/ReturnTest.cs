using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Tests;

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

	[Test]
	public void TestResultingSequenceContainsSingle()
	{
		_ = Assert.Single(SomeSingleton.Sequence);
	}

	[Test]
	public void TestResultingSequenceContainsTheItemProvided()
	{
		Assert.Contains(SomeSingleton.Item, SomeSingleton.Sequence);
	}

	[Test]
	public void TestResultingListHasCountOne()
	{
		_ = Assert.Single(SomeSingleton.List);
	}

	[Test]
	public void TestContainsReturnsTrueWhenTheResultingSequenceContainsTheItemProvided()
	{
		Assert.Contains(SomeSingleton.Item, SomeSingleton.Sequence);
	}

	[Test]
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

	[Test]
	public void TestResultingCollectionIsReadOnly()
	{
		Assert.True(SomeSingleton.Collection.IsReadOnly);
	}

	[Test]
	public void TestResultingCollectionHasCountOne()
	{
		_ = Assert.Single(SomeSingleton.Collection);
	}

	[Test]
	public void TestIndexZeroContainsTheItemProvided()
	{
		Assert.Equal(SomeSingleton.Item, SomeSingleton.List[0]);
	}

	[Test]
	public void TestIndexOfTheItemProvidedIsZero()
	{
		Assert.Equal(0, SomeSingleton.List.IndexOf(SomeSingleton.Item));
	}

	[Test]
	public void TestIndexOfAnItemNotContainedIsNegativeOne()
	{
		Assert.Equal(-1, SomeSingleton.List.IndexOf(new object()));
	}

	[SuppressMessage("Style", "IDE0200:Remove unnecessary lambda expression", Justification = "Consistency")]
	public static IEnumerable<Action> UnsupportedActions() =>
		[
			() => SomeSingleton.List.Add(new object()),
			() => SomeSingleton.Collection.Clear(),
			() => SomeSingleton.Collection.Remove(SomeSingleton.Item),
			() => SomeSingleton.List.RemoveAt(0),
			() => SomeSingleton.List.Insert(0, new object()),
			() => SomeSingleton.List[0] = new object(),
		];

	[Test]
	[MethodDataSource(nameof(UnsupportedActions))]
	public void TestUnsupportedMethodShouldThrow(Action unsupportedAction)
	{
		_ = Assert.Throws<NotSupportedException>(unsupportedAction);
	}

	[Test]
	public void TestIndexingPastZeroShouldThrow()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => SomeSingleton.List[1]);
	}
}
