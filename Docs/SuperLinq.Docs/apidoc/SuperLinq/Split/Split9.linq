<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 11);

// split a sequence using a key value
var result = sequence
	.Split(x => x % 3 == 2);

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine,
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [0, 1],
//    [3, 4],
//    [6, 7],
//    [9, 10]
// ]
