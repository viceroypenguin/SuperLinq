<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Get a sliding window over the sequence
var buffer = new int[3];
var result = sequence
	.WindowRight(
		buffer,
		c => "   [" + string.Join(", ", c) + "]");

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine,
		result) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [1],
//    [1, 2],
//    [1, 2, 3],
//    [2, 3, 4],
//    [3, 4, 5],
//    [4, 5, 6],
//    [5, 6, 7],
//    [6, 7, 8],
//    [7, 8, 9],
//    [8, 9, 10]
// ]
