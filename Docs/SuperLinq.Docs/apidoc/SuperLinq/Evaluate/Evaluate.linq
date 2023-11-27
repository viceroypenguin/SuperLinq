<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

int Func1() => 3;
int Func2() => 5;
int Func3() => 1;

// Execute an action for each element
var result = SuperEnumerable
	.Evaluate(new[] { Func1, Func2, Func3 });

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [3, 5, 1]
