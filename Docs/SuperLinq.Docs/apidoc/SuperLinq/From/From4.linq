<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

int Func1() => 3;
int Func2() => 5;
int Func3() => 1;
int Func4() => 7;

// Execute an action for each element
var result = SuperEnumerable
	.From(Func1, Func2, Func3, Func4);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [3, 5, 1, 7]
