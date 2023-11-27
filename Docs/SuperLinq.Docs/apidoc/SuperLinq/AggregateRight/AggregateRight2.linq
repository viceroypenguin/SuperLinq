<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var numbers = new string[] { "1", "2", "3", "4", "5", };

// Enumerate strings from right to left and collect the text into larger strings.
var result = numbers
	.AggregateRight("6", (a, b) => $"({a}/{b})");

Console.WriteLine(result);

// This code produces the following output:
// (1/(2/(3/(4/(5/6)))))
