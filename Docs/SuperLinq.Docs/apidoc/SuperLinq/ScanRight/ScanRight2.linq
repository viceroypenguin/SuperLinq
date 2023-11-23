<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);

// execute a scan of the sequence
var result = sequence
	.ScanRight(
		"6",
		(a, b) => $"({a}+{b})");

Console.WriteLine(
	"[ \"" +
	string.Join("\", \"", result) +
	"\" ]");

// This code produces the following output:
// [ "(1+(2+(3+(4+(5+6)))))", "(2+(3+(4+(5+6))))", "(3+(4+(5+6)))", "(4+(5+6))", "(5+6)", "6" ]
