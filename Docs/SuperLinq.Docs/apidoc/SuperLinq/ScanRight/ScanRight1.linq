<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5)
	.Select(x => x.ToString());

// execute a scan of the sequence
var result = sequence
	.ScanRight((a, b) => $"({a}+{b})");

Console.WriteLine(
	"[ \"" +
	string.Join("\", \"", result) +
	"\" ]");

// This code produces the following output:
// [ "(1+(2+(3+(4+5))))", "(2+(3+(4+5)))", "(3+(4+5))", "(4+5)", "5" ]
