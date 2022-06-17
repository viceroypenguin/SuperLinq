using System.Text.RegularExpressions;

namespace Test;

public class FillForwardTest
{
	[Fact]
	public void FillForwardIsLazy()
	{
		new BreakingSequence<object>().FillForward();
	}

	[Fact]
	public void FillForward()
	{
		int? na = null;
		var input = new[] { na, na, 1, 2, na, na, na, 3, 4, na, na };
		var result = input.FillForward();
		Assert.Equal(new[] { na, na, 1, 2, 2, 2, 2, 3, 4, 4, 4 }, result);
	}

	[Fact]
	public void FillForwardWithFillSelector()
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
			select Regex.Split(line, "\x20+").Fold((cont, ctry, city, val) => new
			{
				Continent = cont,
				Country = ctry,
				City = city,
				Value = int.Parse(val),
			});

		data = data.FillForward(e => e.Continent == "-", (e, f) => new { f.Continent, e.Country, e.City, e.Value })
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

		Assert.Equal(expected, data);
	}
}
