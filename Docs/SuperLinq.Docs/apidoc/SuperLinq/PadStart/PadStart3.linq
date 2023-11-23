<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 3);

// Pad a sequence until it is at least a certain length
var result = sequence
	.PadStart(6, i => -i);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [0, -1, -2, 1, 2, 3]
