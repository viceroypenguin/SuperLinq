using System.Text.RegularExpressions;

namespace Test.Async;

public class FillForwardTest
{
	[Fact]
	public void FillForwardIsLazy()
	{
		_ = new AsyncBreakingSequence<object>().FillForward();
	}

	[Fact]
	public async Task FillForward()
	{
		await using var input = TestingSequence.Of<int?>(null, null, 1, 2, null, null, null, 3, 4, null, null);

		await input
			.FillForward()
			.AssertSequenceEqual(default, null, 1, 2, 2, 2, 2, 3, 4, 4, 4);
	}

	[Fact]
	public async Task FillForwardWithFillSelector()
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

		await using var data = (
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

		await data
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
}
