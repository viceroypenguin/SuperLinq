<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	1, 2, 3, 4, 5,
	1, 2, 3, 4, 5,
}.AsEnumerable();

// Find the element `3` in the sequence in the range [5..7]
var result = sequence
	.IndexOf(3, 5, 2);

Console.WriteLine($"Index: {result}");

// This code produces the following output:
// Index: -1
