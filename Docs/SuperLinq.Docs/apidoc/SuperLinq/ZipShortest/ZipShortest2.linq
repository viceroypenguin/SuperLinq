<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, 4, 5 };

// Zip two sequences together
var result = seq1
	.ZipShortest(
		seq2,
		(a, b) => new { A = a, B = b, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ A = aaa, B = 1 }, { A = bb, B = 2 }, { A = c, B = 3 }, { A = ddd, B = 4 }]
