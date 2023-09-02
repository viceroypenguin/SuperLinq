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
	"'J' starts at index {0}, after index 2-4",
	sequence.FindIndex(
		x => x.name.StartsWith("J"),
		2,
		3));

Console.WriteLine(
	"'J' starts at index {0}, after index 2-2",
	sequence.FindIndex(
		x => x.name.StartsWith("J"),
		2,
		1));

// This code produces the following output:
// 'J' starts at index 3, after index 2-4
// 'J' starts at index -1, after index 2-2
