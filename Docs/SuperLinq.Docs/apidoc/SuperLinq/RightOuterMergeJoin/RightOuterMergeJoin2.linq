<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var people = new Person[]
{
	new("John Doe", 1),
	new("Jane Doe", 6),
	new("Lucy Ricardo", 4),
	new("Ricky Ricardo", 2),
	new("Fred Mertz", 3),
	new("Ethel Mertz", 5),
};

var pets = new Pet[]
{
	new("Bear", 8),
	new("Polly", 2),
	new("Minnie", 2),
	new("Mittens", 1),
	new("Patches", 1),
	new("Paws", 1),
};

var results = people.OrderBy(p => p.PersonId)
	.RightOuterMergeJoin(
		pets.OrderBy(p => p.PersonId),
		p => p.PersonId,
		p => p.PersonId,
		pet => $"(N/A, {pet.Name})",
		(person, pet) => $"({person.Name}, {pet.Name})");

foreach (var str in results)
	Console.WriteLine(str);

// This code produces the following output:
// (John Doe, Mittens)
// (John Doe, Patches)
// (John Doe, Paws)
// (Ricky Ricardo, Polly)
// (Ricky Ricardo, Minnie)
// (N/A, Bear)

record Person(string Name, int PersonId);
record Pet(string Name, int PersonId);
