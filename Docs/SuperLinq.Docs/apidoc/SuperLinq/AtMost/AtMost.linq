<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
foreach (var x in Enumerable.Range(3, 5))
{
	// Check that a sequence has a maximum length
	var result = sequence.AtMost(x);

	Console.WriteLine($"AtMost {x}: {result}");
}

// This code produces the following output:
// AtMost 3: False
// AtMost 4: False
// AtMost 5: True
// AtMost 6: True
// AtMost 7: True
