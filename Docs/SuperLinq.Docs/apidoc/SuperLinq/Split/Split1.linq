<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 11);

// split a sequence using a key value
var result = sequence
	.Split(5);

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine,
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [0, 1, 2, 3, 4],
//    [6, 7, 8, 9, 10]
// ]
