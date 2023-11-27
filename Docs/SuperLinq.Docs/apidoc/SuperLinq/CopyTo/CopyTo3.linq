<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
var destination = new int[7];
var span = destination.AsSpan();
	        
// Copy the provided sequence to a span
var result = sequence.CopyTo(span[1..]);

Console.WriteLine(result);
Console.WriteLine(
	"[" +
	string.Join(", ", destination) +
	"]");

// This code produces the following output:
// 5
// [0, 1, 2, 3, 4, 5, 0]
