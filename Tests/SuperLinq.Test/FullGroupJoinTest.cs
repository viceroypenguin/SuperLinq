﻿namespace Test;

public class FullGroupJoinTest
{
	public enum OverloadCase { CustomResult, TupleResult }

	[Fact]
	public void FullGroupIsLazy()
	{
		var bs = new BreakingSequence<int>();
		var bf = BreakingFunc.Of<int, int>();
		var bfg = BreakingFunc.Of<int, IEnumerable<int>, IEnumerable<int>, int>();

		_ = bs.FullGroupJoin(bs, bf, bf, bfg);
	}

	[Theory]
	[InlineData(OverloadCase.CustomResult)]
	[InlineData(OverloadCase.TupleResult)]
	public void FullGroupJoinsResults(OverloadCase overloadCase)
	{
		using var listA = TestingSequence.Of(1, 2);
		using var listB = TestingSequence.Of(2, 3);

		var result = FullGroupJoin(overloadCase, listA, listB, SuperEnumerable.Identity).ToDictionary(a => a.Key);

		Assert.Equal(3, result.Keys.Count);

		Assert.Empty(result[1].Second);
		result[1].First.AssertSequenceEqual(1);

		Assert.Empty(result[3].First);
		result[3].Second.AssertSequenceEqual(3);

		result[2].First.AssertSequenceEqual(2);
		result[2].Second.AssertSequenceEqual(2);
	}

	[Theory]
	[InlineData(OverloadCase.CustomResult)]
	[InlineData(OverloadCase.TupleResult)]
	public void FullGroupJoinsEmptyLeft(OverloadCase overloadCase)
	{
		using var listA = Array.Empty<int>().AsTestingSequence();
		using var listB = TestingSequence.Of(2, 3);

		var result = FullGroupJoin(overloadCase, listA, listB, SuperEnumerable.Identity).ToDictionary(a => a.Key);

		Assert.Equal(2, result.Keys.Count);

		Assert.Empty(result[2].First);
		Assert.Equal(2, result[2].Second.Single());

		Assert.Empty(result[3].First);
		Assert.Equal(3, result[3].Second.Single());
	}

	[Theory]
	[InlineData(OverloadCase.CustomResult)]
	[InlineData(OverloadCase.TupleResult)]
	public void FullGroupJoinsEmptyRight(OverloadCase overloadCase)
	{
		using var listA = TestingSequence.Of(2, 3);
		using var listB = Array.Empty<int>().AsTestingSequence();

		var result = FullGroupJoin(overloadCase, listA, listB, SuperEnumerable.Identity).ToDictionary(a => a.Key);

		Assert.Equal(2, result.Keys.Count);

		Assert.Equal(2, result[2].First.Single());
		Assert.Empty(result[2].Second);

		Assert.Equal(3, result[3].First.Single());
		Assert.Empty(result[3].Second);
	}

	[Theory]
	[InlineData(OverloadCase.CustomResult)]
	[InlineData(OverloadCase.TupleResult)]
	public void FullGroupPreservesOrder(OverloadCase overloadCase)
	{
		var listA = new[]
		{
			(3, 1),
			(1, 1),
			(2, 1),
			(1, 2),
			(1, 3),
			(3, 2),
			(1, 4),
			(3, 3),
		};
		var listB = new[]
		{
			(4, 1),
			(3, 1),
			(2, 1),
			(0, 1),
			(3, 0),
		};

		var result = FullGroupJoin(overloadCase, listA, listB, x => x.Item1).ToList();

		// Order of keys is preserved
		result.Select(x => x.Key).AssertSequenceEqual(3, 1, 2, 4, 0);

		// Order of joined elements is preserved
		foreach (var (key, first, second) in result)
		{
			first.AssertSequenceEqual(listA.Where(t => t.Item1 == key).ToArray());
			second.AssertSequenceEqual(listB.Where(t => t.Item1 == key).ToArray());
		}
	}

	private static IEnumerable<(int Key, IEnumerable<T> First, IEnumerable<T> Second)> FullGroupJoin<T>(
		OverloadCase overloadCase,
		IEnumerable<T> listA,
		IEnumerable<T> listB,
		Func<T, int> getKey)
	{
		return overloadCase switch
		{
			OverloadCase.CustomResult => listA.FullGroupJoin(listB, getKey, getKey, ValueTuple.Create, comparer: null),
			OverloadCase.TupleResult => listA.FullGroupJoin(listB, getKey, getKey),
			_ => throw new ArgumentOutOfRangeException(nameof(overloadCase)),
		};
	}
}
