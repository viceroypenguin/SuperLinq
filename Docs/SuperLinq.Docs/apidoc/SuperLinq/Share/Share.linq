<Query Kind="Statements">
  <NuGetReference>SuperLinq</NuGetReference>
  <Namespace>SuperLinq</Namespace>
</Query>

var sequence = Enumerable.Range(0, 10);

// allow multiple consumers to cache views of the same sequence
using var rng = sequence.Share();
using var e1 = rng.GetEnumerator();    // e1 has a shared view on the source

Debug.Assert(e1.MoveNext());
Console.WriteLine("e1.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");

Debug.Assert(e1.MoveNext());
Console.WriteLine("e1.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");

using var e2 = rng.GetEnumerator();    // e2 has a shared view on the source

Debug.Assert(e2.MoveNext());    // e2 enumerates over the shared view, advancing the source
Console.WriteLine("e2.MoveNext()");
Console.WriteLine($"e1.Current: {e1.Current}");
Console.WriteLine($"e2.Current: {e2.Current}");

Debug.Assert(e1.MoveNext());    // e1 enumerates over the shared view, advancing the source
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
// e1.Current: 3
// e2.Current: 2
