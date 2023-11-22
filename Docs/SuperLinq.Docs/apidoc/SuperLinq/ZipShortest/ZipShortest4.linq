<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", };
var seq2 = new[] { 1, 2, 3, 4, };
var seq3 = new[] { 20, 5, 7, 12, 42 };

// Zip three sequences together
var result = seq1
	.ZipShortest(
		seq2,
		seq3,
		(a, b, c) => new { A = a, B = b, C = c, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ A = aaa, B = 1, C = 20 }, { A = bb, B = 2, C = 5 }, { A = c, B = 3, C = 7 }]
