namespace Test;

public sealed class ForEachTest
{
	[Fact]
	public void ForEachWithSequence()
	{
		using var seq = TestingSequence.Of(1, 2, 3);

		var results = new List<int>();
		seq.ForEach(results.Add);

		results.AssertSequenceEqual(1, 2, 3);
	}

	[Fact]
	public void ForEachIndexedWithSequence()
	{
		using var seq = TestingSequence.Of(9, 8, 7);

		var valueResults = new List<int>();
		var indexResults = new List<int>();
		seq.ForEach((x, index) => { valueResults.Add(x); indexResults.Add(index); });

		valueResults.AssertSequenceEqual(9, 8, 7);
		indexResults.AssertSequenceEqual(0, 1, 2);
	}
}
