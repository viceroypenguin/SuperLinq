﻿namespace Test.Async;

public class IndexTest
{
	[Fact]
	public void IndexIsLazy()
	{
		var bs = new AsyncBreakingSequence<object>();
		bs.Index();
		bs.Index(0);
	}

	private const string One = "one";
	private const string Two = "two";
	private const string Three = "three";

	[Fact]
	public async Task IndexSequence()
	{
		await using var seq = TestingSequence.Of(One, Two, Three);
		var result = seq.Index();
		await result.AssertSequenceEqual(
			(0, One),
			(1, Two),
			(2, Three));
	}

	[Fact]
	public async Task IndexSequenceStartIndex()
	{
		await using var seq = TestingSequence.Of(One, Two, Three);
		var result = seq.Index(10);
		await result.AssertSequenceEqual(
			(10, One),
			(11, Two),
			(12, Three));
	}
}
