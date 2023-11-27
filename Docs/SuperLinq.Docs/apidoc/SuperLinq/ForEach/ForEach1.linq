<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Fold a sequence into a single value.
sequence
	.ForEach(x => Console.Write($"{x}, "));

// This code produces the following output:
// 1, 2, 3, 4, 5, 
