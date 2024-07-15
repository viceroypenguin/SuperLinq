#if !NETCOREAPP

namespace Test;

public class FutureTests
{
	[Fact]
	public void SortedSetTryGetValue()
	{
		using var list = new[]
		{
			(key: 5, text: "1"),
		}.AsTestingSequence();

		var comparer = Comparer<(int key, string text)>.Create((a, b) => a.key.CompareTo(b.key));
		var set = new SortedSet<(int key, string text)>(list, comparer);

		Assert.True(SuperLinq.Future.TryGetValue(set, (key: 5, text: "2"), out var actual));
		Assert.Equal((key: 5, text: "1"), actual);

		Assert.False(SuperLinq.Future.TryGetValue(set, (key: 4, text: "3"), out actual));
	}
}

#endif
