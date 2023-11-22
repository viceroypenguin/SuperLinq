<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5)
	.Do(i => Console.Write($"{i}, "));
	        
// Consume the provided sequence
sequence.Consume();

// This code produces the following output:
// 1, 2, 3, 4, 5,
