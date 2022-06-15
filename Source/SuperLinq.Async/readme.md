# SuperLinq

Async LINQ to Objects is missing a few desirable features.

This project enhances Async LINQ to Objects with extra methods, 
in a manner which keeps to the spirit of LINQ.

SuperLinq is available for download and installation as
[NuGet packages](https://www.nuget.org/packages/superlinq/).

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

### Consume

Completely consumes the given sequence. This method does not store any data 
during execution

### From

Returns a sequence containing the values resulting from invoking (in order)
each function in the source sequence of functions.

This method has 4 overloads.

### TakeUntil

Returns items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last returned

[dict]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2
[kvp]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.KeyValuePair-2
[lookup]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.lookup-2
