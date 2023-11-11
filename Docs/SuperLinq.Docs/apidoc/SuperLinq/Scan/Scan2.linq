<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 4);

// execute a scan of the sequence
var result = sequence
	.Scan(
		"0",
		(a, b) => $"({a} + {b})");

Console.WriteLine(
	"[ \"" +
	string.Join("\", \"", result) +
	"\" ]");

// This code produces the following output:
// [ "0", "(0 + 1)", "((0 + 1) + 2)", "(((0 + 1) + 2) + 3)", "((((0 + 1) + 2) + 3) + 4)" ]
