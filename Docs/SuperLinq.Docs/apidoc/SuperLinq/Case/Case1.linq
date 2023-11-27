<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequences = Enumerable.Range(1, 5)
	.ToDictionary(
		x => x,
		x => Enumerable.Range(1, x));

// Use a function to select which sequence to return values from.
var selector = 1;
var result = SuperEnumerable
	.Case(
		() => selector,
		sequences);

Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = 4;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");
selector = 2;
Console.WriteLine($"Selector: {selector}; result.Count(): {result.Count()}.");

// This code produces the following output:
// Selector: 1; result.Count(): 1.
// Selector: 4; result.Count(): 4.
// Selector: 2; result.Count(): 2.
