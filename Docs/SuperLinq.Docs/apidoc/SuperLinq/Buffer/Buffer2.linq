<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Break the sequence of numbers into overlapping chunks of size 3
var result = sequence.Buffer(3, 2);

Console.WriteLine(
	"[" + 
	string.Join(
		", ", 
		result.Select(c => "[" + string.Join(", ", c) + "]")) +
	"]");

// This code produces the following output:
// [[1, 2, 3], [3, 4, 5], [5, 6, 7], [7, 8, 9], [9, 10]]
