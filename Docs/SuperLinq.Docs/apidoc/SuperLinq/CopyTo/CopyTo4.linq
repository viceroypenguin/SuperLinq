<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
var destination = new int[5];
	        
// Copy the provided sequence to an array
var result = sequence.CopyTo(destination);

Console.WriteLine(result);
Console.WriteLine(
	"[" +
	string.Join(", ", destination) +
	"]");

// This code produces the following output:
// 5
// [1, 2, 3, 4, 5]
