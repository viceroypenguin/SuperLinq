<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var count = 3;

// Use a function to create a sequence at execution time
var result = SuperEnumerable
	.Defer(() => Enumerable.Range(1, count));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");
	
// changing count changes the length of the sequence 
// returned by the function given to Defer
count = 5;

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3]
// [1, 2, 3, 4, 5]
