<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, 4, };
var seq3 = new[] { 20, 5, 7, 12 };

// Determine if sequence ends with the ends sequence
var result = seq1
	.EquiZip(
		seq2,
		seq3);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(aaa, 1, 20), (bb, 2, 5), (c, 3, 7), (ddd, 4, 12)]
