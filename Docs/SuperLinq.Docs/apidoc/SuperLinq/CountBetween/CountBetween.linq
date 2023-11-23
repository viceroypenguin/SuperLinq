<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
foreach (var x in Enumerable.Range(3, 5))
{
	// Check that a sequence has a length between two numbers
	var result = sequence.CountBetween(x, x + 1);

	Console.WriteLine($"CountBetween {x}-{x + 1}: {result}");
}

// This code produces the following output:
// CountBetween 3-4: False
// CountBetween 4-5: True
// CountBetween 5-6: True
// CountBetween 6-7: False
// CountBetween 7-8: False
