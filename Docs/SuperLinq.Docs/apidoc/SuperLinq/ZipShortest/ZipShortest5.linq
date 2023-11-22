<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, };
var seq3 = new[] { 20, 5, };
var seq4 = new[] { "zz", };

// Zip four sequences together
var result = seq1
	.ZipShortest(
		seq2,
		seq3,
		seq4);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(aaa, 1, 20, zz)]
