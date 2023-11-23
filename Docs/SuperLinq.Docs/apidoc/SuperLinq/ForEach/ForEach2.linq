<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Fold a sequence into a single value.
sequence
	.ForEach((x, i) => Console.Write($"({x}, {i}), "));

// This code produces the following output:
// (1, 0), (2, 1), (3, 2), (4, 3), (5, 4), 
