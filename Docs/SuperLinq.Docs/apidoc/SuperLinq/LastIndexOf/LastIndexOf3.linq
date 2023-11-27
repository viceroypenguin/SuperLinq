<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	1, 2, 3, 4, 5,
	1, 2, 3, 4, 5,
}.AsEnumerable();

// Find the element `3` in the sequence in the range [0..3]
var result = sequence
	.IndexOf(3, 0, 3);

Console.WriteLine($"Index: {result}");

// This code produces the following output:
// Index: 2
