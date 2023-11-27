<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = "O,l,2,3,4,S,6,7,B,9".Split(','); 

// Use a function to choose and project elements in the sequence
var result = sequence
	.Choose(s => (int.TryParse(s, out var n), n));

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [2, 3, 4, 6, 7, 9]
