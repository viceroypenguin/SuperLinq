using NUnit.Framework;

namespace SuperLinq.Test;

[TestFixture]
public class SplitTest
{
	[Test]
	public void SplitWithSeparatorAndResultTransformation()
	{
		var result = "the quick brown fox".ToCharArray().Split(' ', chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown", "fox");
	}

	[Test]
	public void SplitUptoMaxCount()
	{
		var result = "the quick brown fox".ToCharArray().Split(' ', 2, chars => new string(chars.ToArray()));
		result.AssertSequenceEqual("the", "quick", "brown fox");
	}

	[Test]
	public void SplitWithSeparatorSelector()
	{
		var result = new int?[] { 1, 2, null, 3, null, 4, 5, 6 }.Split(n => n == null);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3);
		reader.Read().AssertSequenceEqual(4, 5, 6);
		reader.ReadEnd();
	}

	[Test]
	public void SplitWithSeparatorSelectorUptoMaxCount()
	{
		var result = new int?[] { 1, 2, null, 3, null, 4, 5, 6 }.Split(n => n == null, 1);

		using var reader = result.Read();
		reader.Read().AssertSequenceEqual(1, 2);
		reader.Read().AssertSequenceEqual(3, null, 4, 5, 6);
		reader.ReadEnd();
	}
}
