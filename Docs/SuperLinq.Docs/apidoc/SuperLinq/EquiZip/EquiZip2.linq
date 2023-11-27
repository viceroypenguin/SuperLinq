<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, 4, };

// Determine if sequence ends with the ends sequence
var result = seq1
	.EquiZip(
		seq2,
		(a, b) => new { Key = a, Value = b, });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ Key = aaa, Value = 1 }, { Key = bb, Value = 2 }, { Key = c, Value = 3 }, { Key = ddd, Value = 4 }]
