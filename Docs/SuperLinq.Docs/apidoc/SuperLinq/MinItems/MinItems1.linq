<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	"A", "B", "C", "D", "F",
	"a", "b", "c", "d", "f",
}.AsEnumerable();

// Find the maximum items of the sequence
var result = sequence
	.MinItems();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [a]
