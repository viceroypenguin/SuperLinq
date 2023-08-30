<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var executionCount = 0;
var sequence = Enumerable.Range(1, 5)
	.Do(_ => executionCount++);
	        
// Execute an action for each element
var loopCount = 0;
var result = sequence
	.DoWhile(() => loopCount++ < 2);

Console.WriteLine($"Before (execCount: {executionCount})");

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

Console.WriteLine($"After (execCount: {executionCount})");

// This code produces the following output:
// Before (execCount: 0)
// [1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5]
// After (execCount: 5)
