<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var seq1 = new[] { 1, 2, 3, };
var seq2 = new[] { "foo", "bar", "quz", };

// Take a slice of the sequence
var result = seq1.Cartesian(seq2);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, foo), (1, bar), (1, quz), (2, foo), (2, bar), (2, quz), (3, foo), (3, bar), (3, quz)]
