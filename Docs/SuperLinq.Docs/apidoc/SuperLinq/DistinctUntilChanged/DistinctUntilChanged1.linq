<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new Item[]
{
	new(key: 3, text: "1"),
	new(key: 3, text: "2"),
	new(key: 2, text: "3"),
	new(key: 2, text: "4"),
	new(key: 1, text: "5"),
	new(key: 1, text: "6"),
	new(key: 3, text: "7"),
	new(key: 3, text: "8"),
	new(key: 2, text: "9"),
	new(key: 2, text: "10"),
	new(key: 1, text: "11"),
	new(key: 1, text: "12"),
};

// Get distinct 
var result = sequence.DistinctUntilChanged();

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(3, 1), (2, 3), (1, 5), (3, 7), (2, 9), (1, 11)]

class Item : IEquatable<Item>
{
	public Item(int key, string text)
	{
		Key = key;
		Text = text;
	}

	public int Key { get; }
	public string Text { get; }

	public bool Equals(Item other) =>
		this.Key == other.Key;

	public override int GetHashCode() =>
		this.Key.GetHashCode();

	public override string ToString() =>
		$"({this.Key}, {this.Text})";
}
