<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 6);
	        
// Fold a sequence into a single value.
var result = sequence
	.Fold((a, b, c, d, e, f) => a + b + c + d + e + f);

Console.WriteLine(result);

// This code produces the following output:
// 21
