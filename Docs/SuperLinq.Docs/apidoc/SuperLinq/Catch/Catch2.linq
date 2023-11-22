<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// this sequence will throw an exception on the 6th element
var sequence = Enumerable.Range(1, 5)
	.Concat(SuperEnumerable.Throw<int>(new InvalidOperationException()));

// Use a second sequence to continue in the case of an exception
var result = sequence
	.Catch(Enumerable.Range(1, 5));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, 4, 5, 1, 2, 3, 4, 5]
