<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

// Return a sequence of a single element
var result = SuperEnumerable.Return(123);

Console.WriteLine(
	"[" + 
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [123]
