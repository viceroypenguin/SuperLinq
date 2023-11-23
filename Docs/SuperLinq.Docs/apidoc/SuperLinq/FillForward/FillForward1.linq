<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new int?[] { null, null, 1, 2, null, null, null, 3, 4, null, null, };
	        
// Fill in missing elements from previous elements
var result = sequence.FillForward();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [, , 1, 2, 2, 2, 2, 3, 4, 4, 4]
