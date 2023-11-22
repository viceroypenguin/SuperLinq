<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
foreach (var x in Enumerable.Range(3, 5))
{
	// Check that a sequence has a minimum size
	var result = sequence.AtLeast(x);

	Console.WriteLine($"AtLeast {x}: {result}");
}

// This code produces the following output:
// AtLeast 3: True
// AtLeast 4: True
// AtLeast 5: True
// AtLeast 6: False
// AtLeast 7: False
