<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 4);

// Partition a sequence
var result = sequence.Permutations();

Console.WriteLine(
	$"""
	[
	{string.Join(Environment.NewLine, result.Select(r => "\t[" + string.Join(", ", r) + "]"))}
	]
	""");

// This code produces the following output:
// [
//   [0, 1, 2, 3]
//   [0, 1, 3, 2]
//   [0, 2, 1, 3]
//   [0, 2, 3, 1]
//   [0, 3, 1, 2]
//   [0, 3, 2, 1]
//   [1, 0, 2, 3]
//   [1, 0, 3, 2]
//   [1, 2, 0, 3]
//   [1, 2, 3, 0]
//   [1, 3, 0, 2]
//   [1, 3, 2, 0]
//   [2, 0, 1, 3]
//   [2, 0, 3, 1]
//   [2, 1, 0, 3]
//   [2, 1, 3, 0]
//   [2, 3, 0, 1]
//   [2, 3, 1, 0]
//   [3, 0, 1, 2]
//   [3, 0, 2, 1]
//   [3, 1, 0, 2]
//   [3, 1, 2, 0]
//   [3, 2, 0, 1]
//   [3, 2, 1, 0]
// ]
