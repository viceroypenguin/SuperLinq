<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
// Execute an action when enumeration is complete, regardless of success or fail.
var result = sequence
	.Do(i => Console.Write($"{i}, "))
	.Finally(() => Console.WriteLine("Completed"));

result.Consume();

// This code produces the following output:
// 1, 2, 3, 4, 5, Completed
