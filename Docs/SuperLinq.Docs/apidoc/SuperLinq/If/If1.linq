<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// Use a function to select which sequence to return values from.
var selector = true;
var result = SuperEnumerable
	.If(
		() => selector,
		sequence);

Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = false;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = true;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");

// This code produces the following output:
// Selector: True; result.Count(): 5.
// Selector: False; result.Count(): 0.
// Selector: True; result.Count(): 5.
