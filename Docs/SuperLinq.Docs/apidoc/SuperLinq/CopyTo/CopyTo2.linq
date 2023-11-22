<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
var destination = new List<int>() { -1, -2, };
	        
// Copy the provided sequence to a list at a specified index
var result = sequence.CopyTo(destination, 2);

Console.WriteLine(result);
Console.WriteLine(
	"[" +
	string.Join(", ", destination) +
	"]");

// This code produces the following output:
// 5
// [-1, -2, 1, 2, 3, 4, 5]
