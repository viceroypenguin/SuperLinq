<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 7);
	        
// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c, d, e, f, g) =>
		a + b + c + d + e + f + g);

Console.WriteLine(result);

// This code produces the following output:
// 28
