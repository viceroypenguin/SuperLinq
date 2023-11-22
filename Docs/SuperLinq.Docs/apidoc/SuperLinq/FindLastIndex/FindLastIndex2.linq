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
	"'J' starts at index {0}, before index 0",
	sequence.FindLastIndex(
		x => x.name.StartsWith("J"),
		0));

Console.WriteLine(
	"'J' starts at index {0}, before index ^2",
	sequence.FindLastIndex(
		x => x.name.StartsWith("J"),
		^2));

// This code produces the following output:
// 'J' starts at index -1, before index 0
// 'J' starts at index 4, before index ^2
