<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(1, 5);
	        
foreach (var x in Enumerable.Range(3, 5))
{
	// Compare the length of two sequences
	var result = sequence.CompareCount(Enumerable.Range(1, x));

	Console.WriteLine($"CompareCount {x}: {result}");
}

// This code produces the following output:
// CompareCount 3: 1
// CompareCount 4: 1
// CompareCount 5: 0
// CompareCount 6: -1
// CompareCount 7: -1
