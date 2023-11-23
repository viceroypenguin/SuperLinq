<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Repeat(1, 3)
	.Concat(Enumerable.Repeat(2, 4))
	.Concat(Enumerable.Repeat(3, 2));

// Get the run-length encoding of a sequence
var result = sequence.RunLengthEncode();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, 3), (2, 4), (3, 2)]
