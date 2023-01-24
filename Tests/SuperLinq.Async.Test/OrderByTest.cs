namespace Test.Async;

/// <summary>
/// Verify the behavior of the OrderBy/ThenBy operators
/// </summary>
public class OrderByTests
{
	/// <summary>
	/// Verify that OrderBy preserves the selector
	/// </summary>
	[Fact]
	public async Task TestOrderBySelectorPreserved()
	{
		var sequenceAscending = AsyncEnumerable.Range(1, 100);
		var sequenceDescending = sequenceAscending.Reverse();

		var resultAsc1 = sequenceAscending.OrderBy(SuperEnumerable.Identity, OrderByDirection.Descending);
		var resultAsc2 = sequenceAscending.OrderByDescending(SuperEnumerable.Identity);
		// ensure both order by operations produce identical results
		Assert.Equal(await resultAsc2.ToListAsync(), await resultAsc1.ToListAsync());

		var resultDes1 = sequenceDescending.OrderBy(SuperEnumerable.Identity, OrderByDirection.Ascending);
		var resultDes2 = sequenceDescending.OrderBy(SuperEnumerable.Identity);
		// ensure both order by operations produce identical results
		Assert.Equal(await resultDes2.ToListAsync(), await resultDes1.ToListAsync());
	}

	/// <summary>
	/// Verify that OrderBy preserves the comparer
	/// </summary>
	[Fact]
	public async Task TestOrderByComparerPreserved()
	{
		var sequence = AsyncEnumerable.Range(1, 100);
		var sequenceAscending = sequence.Select(x => x.ToString());
		var sequenceDescending = sequenceAscending.Reverse();

		var comparer = Comparer<string>.Create((a, b) => int.Parse(a).CompareTo(int.Parse(b)));

		var resultAsc1 = sequenceAscending.OrderBy(SuperEnumerable.Identity, comparer, OrderByDirection.Descending);
		var resultAsc2 = sequenceAscending.OrderByDescending(SuperEnumerable.Identity, comparer);
		// ensure both order by operations produce identical results
		Assert.Equal(await resultAsc2.ToListAsync(), await resultAsc1.ToListAsync());
		// ensure comparer was applied in the order by evaluation
		Assert.Equal(await sequenceDescending.ToListAsync(), await resultAsc1.ToListAsync());

		var resultDes1 = sequenceDescending.OrderBy(SuperEnumerable.Identity, comparer, OrderByDirection.Ascending);
		var resultDes2 = sequenceDescending.OrderBy(SuperEnumerable.Identity, comparer);
		// ensure both order by operations produce identical results
		Assert.Equal(await resultDes2.ToListAsync(), await resultDes1.ToListAsync());
		// ensure comparer was applied in the order by evaluation
		Assert.Equal(await sequenceAscending.ToListAsync(), await resultDes1.ToListAsync());
	}

	/// <summary>
	/// Verify that ThenBy preserves the selector
	/// </summary>
	[Fact]
	public async Task TestThenBySelectorPreserved()
	{
		var sequence = AsyncSeq(
			new { A = 2, B = 0, },
			new { A = 1, B = 5, },
			new { A = 2, B = 2, },
			new { A = 1, B = 3, },
			new { A = 1, B = 4, },
			new { A = 2, B = 1, });

		var resultA1 = sequence
			.OrderBy(x => x.A, OrderByDirection.Ascending)
			.ThenBy(y => y.B, OrderByDirection.Ascending);
		var resultA2 = sequence
			.OrderBy(x => x.A)
			.ThenBy(y => y.B);
		// ensure both produce the same order
		Assert.Equal(await resultA2.ToListAsync(), await resultA1.ToListAsync());

		var resultB1 = sequence
			.OrderBy(x => x.A, OrderByDirection.Ascending)
			.ThenBy(y => y.B, OrderByDirection.Descending);
		var resultB2 = sequence
			.OrderBy(x => x.A)
			.ThenByDescending(y => y.B);
		// ensure both produce the same order
		Assert.Equal(await resultB2.ToListAsync(), await resultB1.ToListAsync());
	}

	/// <summary>
	/// Verify that ThenBy preserves the comparer
	/// </summary>
	[Fact]
	public async Task TestThenByComparerPreserved()
	{
		var sequence = AsyncSeq(
			new { A = "2", B = "0", },
			new { A = "1", B = "5", },
			new { A = "2", B = "2", },
			new { A = "1", B = "3", },
			new { A = "1", B = "4", },
			new { A = "2", B = "1", });

		var comparer = Comparer<string>.Create((a, b) => int.Parse(a).CompareTo(int.Parse(b)));

		var resultA1 = sequence
			.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
			.ThenBy(y => y.B, comparer, OrderByDirection.Ascending);
		var resultA2 = sequence
			.OrderBy(x => x.A, comparer)
			.ThenBy(y => y.B, comparer);
		// ensure both produce the same order
		Assert.Equal(await resultA2.ToListAsync(), await resultA1.ToListAsync());

		var resultB1 = sequence
			.OrderBy(x => x.A, comparer, OrderByDirection.Ascending)
			.ThenBy(y => y.B, comparer, OrderByDirection.Descending);
		var resultB2 = sequence
			.OrderBy(x => x.A, comparer)
			.ThenByDescending(y => y.B, comparer);
		// ensure both produce the same order
		Assert.Equal(await resultB2.ToListAsync(), await resultB1.ToListAsync());
	}
}
