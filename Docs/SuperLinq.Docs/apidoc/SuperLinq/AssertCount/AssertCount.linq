<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var numbers = new string[] { "1", "2", "3", "4", "5", };

// Enumerate the sequence with a valid length.
// This code executes normally.
numbers
	.AssertCount(5)
	.Consume();

// Enumerate the sequence with an invalid length.
// This code throws an `ArgumentException`
try
{
	numbers
		.AssertCount(4)
		.Consume();
}
catch (ArgumentException ae)
{
	Console.WriteLine(ae.Message);

	// This code produces the following output:
	// Parameter "source.Count()" (int) must be equal to <4>, was <5>. (Parameter 'source.Count()')
}