<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Break the sequence of numbers into three chunks of 3 and one chunk of 1
var buffer = new int[5];
var result = sequence
	.Batch(
		buffer,
		3,
		c => "[" + string.Join(", ", c) + "]");

Console.WriteLine(
	"[" + 
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [[1, 2, 3], [4, 5, 6], [7, 8, 9], [10]]
