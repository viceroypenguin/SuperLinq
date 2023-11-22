<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 19);
	        
// Count elements in a sequence grouped by key
var result = sequence.CountBy(x => x % 3);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, 7), (2, 6), (0, 6)]
