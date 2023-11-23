<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new[]
{
	(key: 2, name: "Frank"),
	(key: 3, name: "Jill"),
	(key: 5, name: "Dave"),
	(key: 8, name: "Jack"),
	(key: 12, name: "Judith"),
	(key: 14, name: "Robert"),
	(key: 1, name: "Adam"),
};
	        
// Find the first index that matches a condition
Console.WriteLine(
	"'J' starts at index {0}",
	sequence.FindLastIndex(
		x => x.name.StartsWith("J")));

Console.WriteLine(
	"'Ji' starts at index {0}",
	sequence.FindLastIndex(
		x => x.name.StartsWith("Ji")));

Console.WriteLine(
	"'K' starts at index {0}",
	sequence.FindLastIndex(
		x => x.name.StartsWith("K")));

// This code produces the following output:
// 'J' starts at index 4
// 'Ji' starts at index 1
// 'K' starts at index -1
