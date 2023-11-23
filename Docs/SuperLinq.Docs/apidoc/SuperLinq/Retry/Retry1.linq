<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// this sequence will throw an exception on the 4th element
var sequence = Enumerable.Range(1, 3)
	.Concat(SuperEnumerable.Throw<int>(new InvalidOperationException()));

// Re-enumerate the sequence on failure indefinitely
var result = sequence
	.Retry()
	.Take(12);

Console.WriteLine(
	"[" + 
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3]
