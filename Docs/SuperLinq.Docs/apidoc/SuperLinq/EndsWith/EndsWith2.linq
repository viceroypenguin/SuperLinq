<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var pets1 = new List<Pet> { new("Turbo", 2), new("Peanut", 8), };
var pets2 = new List<Pet> { new("Peanut", 8), };

// Determine if pets1 ends with the pets2 sequence.
var result = pets1
	.EndsWith(
		pets2,
		new PetComparer());

Console.WriteLine($"EndsWith: {result}");

// This code produces the following output:
// EndsWith: True

class Pet
{
	public Pet(string name, int age)
	{
		this.Name = name;
		this.Age = age;
	}

	public string Name { get; }
	public int Age { get; }
}

class PetComparer : IEqualityComparer<Pet>
{
	public bool Equals(Pet x, Pet y) =>
		x.Name == y.Name
		&& x.Age == y.Age;
	public int GetHashCode(Pet x) =>
		HashCode.Combine(x.Name, x.Age);
}
