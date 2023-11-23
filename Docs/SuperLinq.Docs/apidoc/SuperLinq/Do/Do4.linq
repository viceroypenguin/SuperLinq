<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var flag = false;
var sequence = SuperEnumerable.If(
	() => flag,
	Enumerable.Range(1, 5),
	Enumerable.Range(1, 4).Concat(SuperEnumerable.Throw<int>(new InvalidOperationException())));
	        
// Execute an action for each element, on error, and on completion
var result = sequence
	.Do(
		i => Console.Write($"{i}, "),
		ex => Console.WriteLine("Failed: " + ex.Message),
		() => Console.WriteLine("Completed"));
	
Console.WriteLine("Before 1");
try
{
	result.Consume();
}
catch (InvalidOperationException) {}
Console.WriteLine("After 1");

flag = true;
Console.WriteLine("Before 2");
result.Consume();
Console.WriteLine("After 2");

// This code produces the following output:
// Before 1
// 1, 2, 3, 4, Failed: Operation is not valid due to the current state of the object.
// After 1
// Before 2
// 1, 2, 3, 4, 5, Completed
// After 2
