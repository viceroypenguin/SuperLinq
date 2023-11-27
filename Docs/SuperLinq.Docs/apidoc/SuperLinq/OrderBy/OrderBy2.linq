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
	.ThenBy(fruit => fruit, StringComparer.Ordinal);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [Mango, apple, grape, Orange, banana, blueberry, raspberry, passionfruit]
