<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 4);

// Replace a value in a sequence
var result = sequence
	.TagFirstLast(
		(item, first, last) => new 
		{ 
			Item = item, 
			IsEdge = first || last, 
		});

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [{ Item = 1, IsEdge = True }, { Item = 2, IsEdge = False }, { Item = 3, IsEdge = False }, { Item = 4, IsEdge = True }]
