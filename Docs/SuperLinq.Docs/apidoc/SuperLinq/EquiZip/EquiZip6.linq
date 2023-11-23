<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, 4, };
var seq3 = new[] { 20, 5, 7, 12 };
var seq4 = new[] { "zz", "yyyy", "xxx", "w", };

// Determine if sequence ends with the ends sequence
var result = seq1
	.EquiZip(
		seq2,
		seq3,
		seq4,
		(a, b, c, d) => new 
		{
			A = a,
			B = b,
			C = c,
			D = d,
		});

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ A = aaa, B = 1, C = 20, D = zz }, { A = bb, B = 2, C = 5, D = yyyy }, { A = c, B = 3, C = 7, D = xxx }, { A = ddd, B = 4, C = 12, D = w }]
