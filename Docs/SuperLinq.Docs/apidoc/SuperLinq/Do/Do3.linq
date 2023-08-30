<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 4).Concat(SuperEnumerable.Throw<int>(new InvalidOperationException()));
	        
// Execute an action for each element, on error, and on completion
var result = sequence
	.Do(
		i => Console.Write($"{i}, "),
		ex => Console.WriteLine("Failed: " + ex.Message));
	
Console.WriteLine("Before");
try
{
	result.Consume();
}
catch (InvalidOperationException) {}
Console.WriteLine("After");

// This code produces the following output:
// Before
// 1, 2, 3, 4, Failed: Operation is not valid due to the current state of the object.
// After
