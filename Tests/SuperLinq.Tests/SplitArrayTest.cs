// Needs to be in non `SuperLinq` namespace for proper testing of overload resolution
#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Not.SuperLinq.Tests;

public sealed class SplitArrayTest
{

	[Fact]
	public void SplitArray()
	{
		var sequence = Enumerable.Range(1, 10).ToArray();
		var result = sequence.Split(5).ToList();

		result
			.AssertSequenceEqual(
				[
					Enumerable.Range(1, 4),
					Enumerable.Range(6, 5),
				]
			);
	}
}
