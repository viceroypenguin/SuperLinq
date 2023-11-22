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

// Get the top N items
var result = sequence.PartialSort(3, OrderByDirection.Descending);

Console.WriteLine(
	"[" +
	string.Join(", ", result) +
	"]");

// This code produces the following output:
// [(5, 1), (5, 2), (4, 3)]

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
