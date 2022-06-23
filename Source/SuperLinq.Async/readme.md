# SuperLinq.Async

Async LINQ to Objects is missing a few desirable features.

This project enhances Async LINQ to Objects with extra methods, 
in a manner which keeps to the spirit of LINQ.

SuperLinq.Async is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq.async/).

## Usage

SuperLinq.Async can be used in one of two ways. The simplest is to just import the
`SuperLinq.Async` namespace and all extension methods become instantly available for
you to use on the types they extend (typically some instantiation of
`IAsyncEnumerable<T>`).

Apart from extension methods, SuperLinq.Async also offers regular static method
that *generate* (instead of operating on) sequences, like `Unfold`,
`Random`, `Sequence` and others. 

## .NET Versions

Base library is supported on .NET Core 3.1 and .NET 5.0+.

## Operators

### AtLeast

Determines whether or not the number of elements in the sequence is greater
than or equal to the given integer.

### AtMost

Determines whether or not the number of elements in the sequence is lesser
than or equal to the given integer.

### CompareCount

Compares two sequences and returns an integer that indicates whether the
first sequence has fewer, the same or more elements than the second sequence.

### Consume

Completely consumes the given sequence. This method does not store any data 
during execution

### CountBy

Applies a key-generating function to each element of a sequence and returns a
sequence of unique keys and their number of occurrences in the original
sequence.

This method has 2 overloads.

### EndsWith

Determines whether the end of the first sequence is equivalent to the second
sequence.

This method has 4 overloads.

### Exactly

Determines whether or not the number of elements in the sequence is equals
to the given integer.

### FillBackward

Returns a sequence with each null reference or value in the source replaced
with the following non-null reference or value in that sequence.

This method has 7 overloads.

### FillForward

Returns a sequence with each null reference or value in the source replaced
with the previous non-null reference or value seen in that sequence.

This method has 7 overloads.

### Fold

Returns the result of applying a function to a sequence with 1 to 16 elements.

This method has 16 overloads.

### From

Returns a sequence containing the values resulting from invoking (in order)
each function in the source sequence of functions.

This method has 4 overloads.

### Insert

Inserts the elements of a sequence into another sequence at a specified index.

### Random

Returns an infinite sequence of random integers using the standard .NET random
number generator.

This method has 6 overloads.

### Segment

Divides a sequence into multiple sequences by using a segment detector based
on the original sequence.

This method has 6 overloads.

### SkipUntil

Skips items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last skipped

### StartsWith

Determines whether the beginning of the first sequence is equivalent to the
second sequence.

This method has 4 overloads.

### TakeUntil

Returns items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last returned

[dict]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2
[kvp]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.KeyValuePair-2
[lookup]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.lookup-2
