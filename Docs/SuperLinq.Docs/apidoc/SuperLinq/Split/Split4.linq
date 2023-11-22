<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 3).Repeat(10);

// split a sequence using a key value
var result = sequence
	.Split(2, EqualityComparer<int>.Default, 4);

Console.WriteLine(
	"[" + Environment.NewLine + 
	string.Join(
		", " + Environment.NewLine,
		result.Select(c => "   [" + string.Join(", ", c) + "]")) +
	Environment.NewLine + "]");

// This code produces the following output:
// [
//    [0, 1],
//    [0, 1],
//    [0, 1],
//    [0, 1],
//    [0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2]
// ]
