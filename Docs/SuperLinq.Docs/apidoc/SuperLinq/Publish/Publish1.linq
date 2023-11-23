<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 10);

// allow multiple consumers to cache views of the same sequence
using var rng = sequence.Publish();
using var e1 = rng.GetEnumerator();    // e1 has a view on the source starting from element 0

Debug.Assert(e1.MoveNext());
Console.WriteLine("e1.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");

Debug.Assert(e1.MoveNext());
Console.WriteLine("e1.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");

using var e2 = rng.GetEnumerator();

Debug.Assert(e2.MoveNext());    // e2 has a view on the source starting from element 2
Console.WriteLine("e2.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");
Console.WriteLine($"e2.Current: {e2.Current}");

Debug.Assert(e1.MoveNext());    // e1 continues to enumerate over its view
Console.WriteLine("e1.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");
Console.WriteLine($"e2.Current: {e2.Current}");

// This code produces the following output:
// e1.MoveNext()
// e1.Current: 0
// e1.MoveNext()
// e1.Current: 1
// e2.MoveNext()
// e1.Current: 1
// e2.Current: 2
// e1.MoveNext()
// e1.Current: 2
// e2.Current: 2
