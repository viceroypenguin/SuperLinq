using System.Text.RegularExpressions;

namespace Test;

public class FillForwardTest
{
	[Fact]
	public void FillForwardIsLazy()
	{
		_ = new BreakingSequence<object>().FillForward();
		_ = new BreakingSequence<object>().FillForward(BreakingFunc.Of<object, bool>());
		_ = new BreakingSequence<object>().FillForward(BreakingFunc.Of<object, bool>(), BreakingFunc.Of<object, object, object>());
	}

	public static IEnumerable<object[]> GetIntNullSequences() =>
		Seq<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null)
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntNullSequences))]
	public void FillForwardBlank(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillForward(x => x != 200)
				.AssertSequenceEqual(default(int?), null, 1, 2, null, null, null, 3, 4, null, null);
		}
	}

	[Theory]
	[MemberData(nameof(GetIntNullSequences))]
	public void FillForward(IDisposableEnumerable<int?> seq)
	{
		using (seq)
		{
			seq
				.FillForward()
				.AssertSequenceEqual(default(int?), null, 1, 2, 2, 2, 2, 3, 4, 4, 4);
		}
	}

	public static IEnumerable<object[]> GetIntSequences() =>
		Enumerable.Range(1, 13)
			.GetBreakingCollectionSequences()
			.Select(x => new object[] { x });

	[Theory]
	[MemberData(nameof(GetIntSequences))]
	public void FillForwardWithFillSelector(IDisposableEnumerable<int> seq)
	{
		using (seq)
		{
			seq
				.FillForward(e => e % 5 != 0, (x, y) => x * y)
				.AssertSequenceEqual(1, 2, 3, 4, 5, 30, 35, 40, 45, 10, 110, 120, 130);
		}
	}

	[Fact]
	public void FillForwardExample()
	{
		const string Table = @"
                Europe UK          London      123
                -      -           Manchester  234
                -      -           Glasgow     345
                -      Germany     Munich      456
                -      -           Frankfurt   567
                -      -           Stuttgart   678
                Africa Egypt       Cairo       789
                -      -           Alexandria  890
                -      Kenya       Nairobi     901
            ";

		using var data = (
			from line in Table.Split('\n')
			select line.Trim() into line
			where !string.IsNullOrEmpty(line)
			let x = Regex.Split(line, "\x20+")
			select new
			{
				Continent = x[0],
				Country = x[1],
				City = x[2],
				Value = int.Parse(x[3]),
			}).AsTestingSequence();

		data
			.FillForward(e => e.Continent == "-", (e, f) => new { f.Continent, e.Country, e.City, e.Value })
			.FillForward(e => e.Country == "-", (e, f) => new { e.Continent, f.Country, e.City, e.Value })
			.AssertSequenceEqual(
				new { Continent = "Europe", Country = "UK", City = "London", Value = 123 },
				new { Continent = "Europe", Country = "UK", City = "Manchester", Value = 234 },
				new { Continent = "Europe", Country = "UK", City = "Glasgow", Value = 345 },
				new { Continent = "Europe", Country = "Germany", City = "Munich", Value = 456 },
				new { Continent = "Europe", Country = "Germany", City = "Frankfurt", Value = 567 },
				new { Continent = "Europe", Country = "Germany", City = "Stuttgart", Value = 678 },
				new { Continent = "Africa", Country = "Egypt", City = "Cairo", Value = 789 },
				new { Continent = "Africa", Country = "Egypt", City = "Alexandria", Value = 890 },
				new { Continent = "Africa", Country = "Kenya", City = "Nairobi", Value = 901 });
	}

	[Fact]
	public void FillForwardCollectionCount()
	{
		using var sequence = Enumerable.Range(1, 10_000)
			.AsBreakingCollection();

		var result = sequence.FillForward();
		result.AssertCollectionErrorChecking(10_000);
	}
}
