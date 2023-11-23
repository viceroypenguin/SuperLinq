<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var pets1 = new List<Pet> { new("Turbo", 2), new("Peanut", 8), };
var pets2 = new List<Pet> { new("Peanut", 8), new("Turbo", 2), };

// Determine if the two collections are equal, using the default equality comparer
var result = pets1.CollectionEqual(pets2);

Console.WriteLine(result);

// This code produces the following output:
// True

record Pet(string Name, int Age);