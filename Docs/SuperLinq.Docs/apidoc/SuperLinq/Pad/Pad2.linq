<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 3);

// Pad a sequence until it is at least a certain length
var result = sequence
	.Pad(6, -5);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, -5, -5, -5]
