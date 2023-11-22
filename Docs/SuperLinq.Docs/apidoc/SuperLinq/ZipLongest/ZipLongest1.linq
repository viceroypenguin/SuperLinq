<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { "aaa", "bb", "c", "ddd", };
var seq2 = new[] { 1, 2, 3, 4, 5 };

// Zip two sequences together
var result = seq1.ZipLongest(seq2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(aaa, 1), (bb, 2), (c, 3), (ddd, 4), (, 5)]
