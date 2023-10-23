<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var first = Enumerable.Range(1, 5);
var second = Enumerable.Range(1, 5);
	        
// Insert one sequence into another
var result = first
	.Insert(second, 2);
	        
Console.WriteLine(string.Join(", ", result));

// This code produces the following output:
// 1, 2, 1, 2, 3, 4, 5, 3, 4, 5
