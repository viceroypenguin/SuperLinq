﻿namespace SuperLinq.Async.Tests;

public sealed class ForEachTest
{
	[Test]
	public async Task ForEachWithSequence()
	{
		await using var seq = TestingSequence.Of(1, 2, 3);

		var results = new List<int>();
		await seq.ForEach(results.Add);

		results.AssertSequenceEqual(1, 2, 3);
	}

	[Test]
	public async Task ForEachAsyncWithSequence()
	{
		await using var seq = TestingSequence.Of(1, 2, 3);

		var results = new List<int>();
		await seq.ForEach(async i => { results.Add(i); await default(ValueTask); });

		results.AssertSequenceEqual(1, 2, 3);
	}

	[Test]
	public async Task ForEachIndexedWithSequence()
	{
		await using var seq = TestingSequence.Of(9, 8, 7);

		var valueResults = new List<int>();
		var indexResults = new List<int>();
		await seq.ForEach((x, index) => { valueResults.Add(x); indexResults.Add(index); });

		valueResults.AssertSequenceEqual(9, 8, 7);
		indexResults.AssertSequenceEqual(0, 1, 2);
	}

	[Test]
	public async Task ForEachIndexedAsyncWithSequence()
	{
		await using var seq = TestingSequence.Of(9, 8, 7);

		var valueResults = new List<int>();
		var indexResults = new List<int>();
		await seq.ForEach(async (x, index) =>
		{
			valueResults.Add(x);
			indexResults.Add(index);
			await default(ValueTask);
		});

		valueResults.AssertSequenceEqual(9, 8, 7);
		indexResults.AssertSequenceEqual(0, 1, 2);
	}
}
