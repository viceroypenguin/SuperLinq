<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 4);

// check that sequence starts with a known sequence of values
var result = sequence
	.Subsets();

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine,
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [],
//    [0],
//    [1],
//    [2],
//    [3],
//    [0, 1],
//    [0, 2],
//    [0, 3],
//    [1, 2],
//    [1, 3],
//    [2, 3],
//    [0, 1, 2],
//    [0, 1, 3],
//    [0, 2, 3],
//    [1, 2, 3],
//    [0, 1, 2, 3]
// ]
