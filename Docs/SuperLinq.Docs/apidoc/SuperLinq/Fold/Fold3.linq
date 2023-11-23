<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 3);
	        
// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c) => a + b + c);

Console.WriteLine(result);

// This code produces the following output:
// 6
