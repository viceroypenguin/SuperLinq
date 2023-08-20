<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// this sequence will throw an exception on the 6th element
var sequence = Enumerable.Range(1, 5).Select(i => i.ToString())
	.Concat(SuperEnumerable.Throw<string>(new InvalidOperationException()));

// Use a function to determine how to handle an exception
var result = sequence
	.Catch(
		(InvalidOperationException ex) => SuperEnumerable.Return(ex.Message));

Console.WriteLine(
	"[" + 
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [1, 2, 3, 4, 5, Operation is not valid due to the current state of the object.]
