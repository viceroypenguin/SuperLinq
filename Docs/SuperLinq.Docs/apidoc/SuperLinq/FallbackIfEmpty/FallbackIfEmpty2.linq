<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var flag = false;
var sequence = SuperEnumerable.Defer(
	() =>
	{
		if (flag) return Enumerable.Empty<int>();
		flag = true;
		return Enumerable.Range(1, 5);
	});
	
// Replace a sequence if it is empty.
var result = sequence
	.FallbackIfEmpty(
		Enumerable.Range(20, 3));

Console.WriteLine(
	"Non-Empty: [" +
	string.Join(", ", result) +
	"]");

Console.WriteLine(
	"Empty: [" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// Non-Empty: [1, 2, 3, 4, 5]
// Empty: [20, 21, 22]
