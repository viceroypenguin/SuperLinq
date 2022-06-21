using System.Text.RegularExpressions;

namespace Test.Async;

public class FillForwardTest
{
	[Fact]
	public void FillForwardIsLazy()
	{
		new AsyncBreakingSequence<object>().FillForward();
	}

	[Fact]
	public Task FillForward()
	{
		int? na = null;
		var input = AsyncSeq(na, na, 1, 2, na, na, na, 3, 4, na, na);
		var result = input.FillForward();
		return result.AssertSequenceEqual(na, na, 1, 2, 2, 2, 2, 3, 4, 4, 4);
	}

	[Fact]
	public Task FillForwardWithFillSelector()
	{
		const string table = @"
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

		var data =
			from line in table.Split('\n')
			select line.Trim() into line
			where !string.IsNullOrEmpty(line)
			let x = Regex.Split(line, "\x20+")
			select new
			{
				Continent = x[0],
				Country = x[1],
				City = x[2],
				Value = int.Parse(x[3]),
			};

		var fData = data.ToAsyncEnumerable()
			.FillForward(e => e.Continent == "-", (e, f) => new { f.Continent, e.Country, e.City, e.Value })
			.FillForward(e => e.Country == "-", (e, f) => new { e.Continent, f.Country, e.City, e.Value });

		var expected = new[]
		{
			new { Continent = "Europe", Country = "UK",      City = "London",     Value = 123 },
			new { Continent = "Europe", Country = "UK",      City = "Manchester", Value = 234 },
			new { Continent = "Europe", Country = "UK",      City = "Glasgow",    Value = 345 },
			new { Continent = "Europe", Country = "Germany", City = "Munich",     Value = 456 },
			new { Continent = "Europe", Country = "Germany", City = "Frankfurt",  Value = 567 },
			new { Continent = "Europe", Country = "Germany", City = "Stuttgart",  Value = 678 },
			new { Continent = "Africa", Country = "Egypt",   City = "Cairo",      Value = 789 },
			new { Continent = "Africa", Country = "Egypt",   City = "Alexandria", Value = 890 },
			new { Continent = "Africa", Country = "Kenya",   City = "Nairobi",    Value = 901 },
		};

		return fData.AssertSequenceEqual(expected);
	}
}
