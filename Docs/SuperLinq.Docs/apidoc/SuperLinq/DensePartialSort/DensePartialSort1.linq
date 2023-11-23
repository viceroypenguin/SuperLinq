<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = new Item[]
{
	new(key: 5, text: "1"),
	new(key: 5, text: "2"),
	new(key: 4, text: "3"),
	new(key: 4, text: "4"),
	new(key: 3, text: "5"),
	new(key: 3, text: "6"),
	new(key: 2, text: "7"),
	new(key: 2, text: "8"),
	new(key: 1, text: "9"),
	new(key: 1, text: "10"),
};

// Get the top N sets of items
var result = sequence.DensePartialSort(3);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(1, 9), (1, 10), (2, 7), (2, 8), (3, 5), (3, 6)]

class Item : IComparable<Item>
{
	public Item(int key, string text)
	{
		Key = key;
		Text = text;
	}

	public int Key { get; }
	public string Text { get; }

	public int CompareTo(Item other) =>
		this.Key.CompareTo(other.Key);

	public override string ToString() =>
		$"({this.Key}, {this.Text})"; 
}
