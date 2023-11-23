<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 10);

// Filter a sequence based on a leading value
var result = sequence
	.WhereLead(1, -20, (cur, lead) => cur + lead >= 10);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [5, 6, 7, 8, 9]
