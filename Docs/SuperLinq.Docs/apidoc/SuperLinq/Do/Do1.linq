<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
// Execute an action for each element
var result = sequence
	.Do(i => Console.Write($"{i}, "));
	
Console.WriteLine("Before");
result.Consume();
Console.WriteLine("After");

// This code produces the following output:
// Before
// 1, 2, 3, 4, 5, After
