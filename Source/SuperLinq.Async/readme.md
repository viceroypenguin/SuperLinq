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
that *generate* (instead of operating on) sequences, like `Generate`,
`GenerateByIndex` and others. 

## .NET Versions

Base library is supported on .NET Core 3.1 and .NET 5.0+.


## Documentation

Detailed documentation on the operators is available [here](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.html).
