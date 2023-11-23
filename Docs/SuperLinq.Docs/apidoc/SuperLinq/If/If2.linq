<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence1 = Enumerable.Range(1, 5);
var sequence2 = Enumerable.Range(1, 10);

// Use a function to select which sequence to return values from.
var selector = true;
var result = SuperEnumerable
	.If(
		() => selector,
		sequence1,
		sequence2);

Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = false;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = true;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");

// This code produces the following output:
// Selector: True; result.Count(): 5.
// Selector: False; result.Count(): 10.
// Selector: True; result.Count(): 5.
