using System.Globalization;

namespace Test;

/// <summary>
/// Verify the behavior of the OrderBy/ThenBy operators
/// </summary>
public sealed class OrderByTests
{
	/// <summary>
	/// Verify that OrderBy preserves the selector
	/// </summary>
	[Fact]
	public void TestOrderBySelectorPreserved()
	{
		var sequenceAscending = Enumerable.Range(1, 100);
		var sequenceDescending = sequenceAscending.Reverse();

		var resultAsc1 = sequenceAscending.OrderBy(SuperEnumerable.Identity, OrderByDirection.Descending);
		var resultAsc2 = sequenceAscending.OrderByDescending(SuperEnumerable.Identity);
		// ensure both order by operations produce identical results
		Assert.Equal(resultAsc2, resultAsc1);

		var resultDes1 = sequenceDescending.OrderBy(SuperEnumerable.Identity, OrderByDirection.Ascending);
		var resultDes2 = sequenceDescending.OrderBy(SuperEnumerable.Identity);
		// ensure both order by operations produce identical results
		Assert.Equal(resultDes2, resultDes1);
	}

	/// <summary>
	/// Verify that OrderBy preserves the comparer
	/// </summary>
	[Fact]
	public void TestOrderByComparerPreserved()
	{
		var sequence = Enumerable.Range(1, 100);
		var sequenceAscending = sequence.Select(x => x.ToString(CultureInfo.InvariantCulture));
		var sequenceDescending = sequenceAscending.Reverse();

		var comparer = Comparer<string>.Create(
			(a, b) => int.Parse(a, CultureInfo.InvariantCulture)
				.CompareTo(int.Parse(b, CultureInfo.InvariantCulture))
		);

		var resultAsc1 = sequenceAscending.OrderBy(SuperEnumerable.Identity, comparer, OrderByDirection.Descending);
		var resultAsc2 = sequenceAscending.OrderByDescending(SuperEnumerable.Identity, comparer);
		// ensure both order by operations produce identical results
		Assert.Equal(resultAsc2, resultAsc1);
		// ensure comparer was applied in the order by evaluation
		Assert.Equal(sequenceDescending, resultAsc1);

		var resultDes1 = sequenceDescending.OrderBy(SuperEnumerable.Identity, comparer, OrderByDirection.Ascending);
		var resultDes2 = sequenceDescending.OrderBy(SuperEnumerable.Identity, comparer);
		// ensure both order by operations produce identical results
		Assert.Equal(resultDes2, resultDes1);
		// ensure comparer was applied in the order by evaluation
		Assert.Equal(sequenceAscending, resultDes1);
	}

	/// <summary>
	/// Verify that ThenBy preserves the selector
	/// </summary>
	[Fact]
	public void TestThenBySelectorPreserved()
	{
		var sequence = new[]
		{
			new { A = 2, B = 0, },
			new { A = 1, B = 5, },
			new { A = 2, B = 2, },
			new { A = 1, B = 3, },
			new { A = 1, B = 4, },
			new { A = 2, B = 1, },
		};

		var resultA1 = sequence
			.OrderBy(x => x.A, OrderByDirection.Ascending)
			.ThenBy(y => y.B, OrderByDirection.Ascending);

		var resultA2 = sequence
			.OrderBy(x => x.A)
			.ThenBy(y => y.B);

		// ensure both produce the same order
		Assert.Equal(resultA2, resultA1);

		var resultB1 = sequence
			.OrderBy(x => x.A, OrderByDirection.Ascending)
			.ThenBy(y => y.B, OrderByDirection.Descending);

		var resultB2 = sequence
			.OrderBy(x => x.A)
			.ThenByDescending(y => y.B);

		// ensure both produce the same order
		Assert.Equal(resultB2, resultB1);
	}

	/// <summary>
	/// Verify that ThenBy preserves the comparer
	/// </summary>
	[Fact]
	public void TestThenByComparerPreserved()
	{
		var sequence = new[]
		{
			new { A = "2", B = "0", },
			new { A = "1", B = "5", },
			new { A = "2", B = "2", },
			new { A = "1", B = "3", },
			new { A = "1", B = "4", },
			new { A = "2", B = "1", },
		};

		var comparer = Comparer<string>.Create(
			(a, b) => int.Parse(a, CultureInfo.InvariantCulture)
				.CompareTo(int.Parse(b, CultureInfo.InvariantCulture))
		);

		var resultA1 = sequence
			.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
			.ThenBy(y => y.B, comparer, OrderByDirection.Ascending);

		var resultA2 = sequence
			.OrderBy(x => x.A, comparer)
			.ThenBy(y => y.B, comparer);

		// ensure both produce the same order
		Assert.Equal(resultA2, resultA1);

		var resultB1 = sequence
			.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
			.ThenBy(y => y.B, comparer, OrderByDirection.Descending);

		var resultB2 = sequence
			.OrderBy(x => x.A, comparer)
			.ThenByDescending(y => y.B, comparer);

		// ensure both produce the same order
		Assert.Equal(resultB2, resultB1);
	}
}
