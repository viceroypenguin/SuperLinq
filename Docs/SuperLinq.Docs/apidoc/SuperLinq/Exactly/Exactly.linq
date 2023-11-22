<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
foreach (var x in Enumerable.Range(3, 5))
{
	// Check that a sequence has an exact size
	var result = sequence.Exactly(x);

	Console.WriteLine($"Exactly {x}: {result}");
}

// This code produces the following output:
// Exactly 3: False
// Exactly 4: False
// Exactly 5: True
// Exactly 6: False
// Exactly 7: False
