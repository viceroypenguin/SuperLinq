<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var fruits = new[]
{
	"grape", "passionfruit", "banana", "Mango",
	"Orange", "raspberry", "apple", "blueberry",
};

// Sort the strings first by their length and then
// alphabetically by passing the identity selector function.
var result = fruits
	.OrderBy(fruit => fruit.Length)
	.ThenBy(fruit => fruit);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [apple, grape, Mango, banana, Orange, blueberry, raspberry, passionfruit]
