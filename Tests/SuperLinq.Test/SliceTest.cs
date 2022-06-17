namespace Test;

/// <summary>
/// Verify the behavior of the Slice operator
/// </summary>
public class SliceTests
{
	/// <summary>
	/// Verify that Slice evaluates in a lazy manner.
	/// </summary>
	[Fact]
	public void TestSliceIsLazy()
	{
		new BreakingSequence<int>().Slice(10, 10);
	}

	/// <summary>
	/// Verify that a slice from 0 to sequence.Count() yields the source sequence.
	/// NOTE: Since there are two different implementations of Slice() - one optimized that's
	///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
	/// </summary>
	[Fact]
	public void TestSliceIdentity()
	{
		const int count = 100;
		var sequenceA = Enumerable.Range(1, count);
		var sequenceB = sequenceA.ToList();

		var resultA = sequenceA.Slice(0, count);
		var resultB = sequenceB.Slice(0, count);

		Assert.Equal(sequenceA, resultA);
		Assert.Equal(sequenceB, resultB);
	}

	/// <summary>
	/// Verify that Slice can yield just the first item.
	/// NOTE: Since there are two different implementations of Slice() - one optimized that's
	///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
	/// </summary>
	[Fact]
	public void TestSliceFirstItem()
	{
		const int count = 10;
		var sequenceA = Enumerable.Range(1, count);
		var sequenceB = sequenceA.ToList();
		var resultA = sequenceA.Slice(0, 1);
		var resultB = sequenceB.Slice(0, 1);

		Assert.Equal(sequenceA.Take(1), resultA);
		Assert.Equal(sequenceB.Take(1), resultB);
	}

	/// <summary>
	/// Verify that Slice can yield just the last item.
	/// NOTE: Since there are two different implementations of Slice() - one optimized that's
	///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
	/// </summary>
	[Fact]
	public void TestSliceLastItem()
	{
		const int count = 10;
		var sequenceA = Enumerable.Range(1, count);
		var sequenceB = sequenceA.ToList();
		var resultA = sequenceA.Slice(count - 1, 1);
		var resultB = sequenceB.Slice(count - 1, 1);

		Assert.Equal(sequenceA.Skip(9).Take(1), resultA);
		Assert.Equal(sequenceB.Skip(9).Take(1), resultB);
	}

	/// <summary>
	/// Verify that slice yields the correct set of items when it
	/// is completely contained (start,end) within the source sequence.
	/// NOTE: Since there are two different implementations of Slice() - one optimized that's
	///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
	/// </summary>
	[Fact]
	public void TestSliceSmallerThanSequence()
	{
		const int count = 10;
		var sequenceA = Enumerable.Range(1, count);
		var sequenceB = sequenceA.ToList();
		var resultA = sequenceA.Slice(4, 5);
		var resultB = sequenceB.Slice(4, 5);

		Assert.Equal(sequenceA.Skip(4).Take(5), resultA);
		Assert.Equal(sequenceB.Skip(4).Take(5), resultB);
	}

	/// <summary>
	/// Verify that Slice yields the correct set of items when it
	/// extends past the end of the source sequence.
	/// NOTE: Since there are two different implementations of Slice() - one optimized that's
	///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
	/// </summary>
	[Fact]
	public void TestSliceLongerThanSequence()
	{
		const int count = 100;
		var sequenceA = Enumerable.Range(1, count);
		var sequenceB = sequenceA.ToList();
		var resultA = sequenceA.Slice(count / 2, count);
		var resultB = sequenceB.Slice(count / 2, count);

		Assert.Equal(sequenceA.Skip(count / 2).Take(count), resultA);
		Assert.Equal(sequenceB.Skip(count / 2).Take(count), resultB);
	}

	/// <summary>
	/// Verify that slice is optimized for <see cref="IList{T}"/> and <see cref="IReadOnlyList{T}"/> implementations and does not
	/// unnecessarily traverse items outside of the slice region.
	/// </summary>
	[Theory]
	[InlineData(SourceKind.BreakingList)]
	[InlineData(SourceKind.BreakingReadOnlyList)]
	public void TestSliceOptimization(SourceKind sourceKind)
	{
		const int sliceStart = 4;
		const int sliceCount = 3;
		var sequence = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToSourceKind(sourceKind);

		var result = sequence.Slice(sliceStart, sliceCount);

		Assert.Equal(sliceCount, result.Count());
		Assert.Equal(Enumerable.Range(5, sliceCount), result);
	}
}
