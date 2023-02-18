# SuperLinq

LINQ to Objects is missing a few desirable features.

This project enhances LINQ to Objects with extra methods, in a manner which
keeps to the spirit of LINQ.

SuperLinq is available for download and installation as
[NuGet packages](https://www.nuget.org/packages/superlinq/).

## Usage

SuperLinq can be used in one of two ways. The simplest is to just import the
`SuperLinq` namespace and all extension methods become instantly available for
you to use on the types they extend (typically some instantiation of
`IEnumerable<T>`).

Apart from extension methods, SuperLinq also offers regular static method
that *generate* (instead of operating on) sequences, like `Unfold`,
`Random`, `Sequence` and others. 

## Migration from MoreLINQ

In most case, migration should be easy:

1. Remove the nuget reference to MoreLINQ and add a reference to SuperLinq. 
2. Replace any `using MoreLinq;` with `using SuperLinq;`
3. Remove any `using MoreLinq.Extensions.*`

This is because SuperLinq has been updated to be side-by-side compatible
with .NET Core 3.1 and .NET 5.0/6.0. 

### Breaking Changes

#### Framework Support
Support for earlier frameworks has been dropped. The earliest version supported
by SuperLinq is .NET Core 3.1.

#### System.Interactive
SuperLinq now holds a dependency on System.Interactive. This is because some
methods from SuperLinq overlap functions with the same and occasionally the same
name. To reduce conflicts, SuperLinq will defer to System.Interactive for 
these methods when possible. Methods removed include: `.Repeat()`, `.Scan()`, `.ForEach()`,
`.Memoize()`.

#### Acquire
Acquire has been removed. It was used internally to support other operators, but
improved data structures have been introduced to better support them.

#### AwaitQuery/Observable/Experimental Operators
These operators have been removed, as they do not fit the model of the other
SuperLinq operators. 

#### Backsert
This method has been obsoleted in favor of a new overload for `.Insert()` that
receives an `Index` parameter, which covers the same behavior.

#### Batch
The `.Batch()` method has been obsoleted in favor of the .NET method `.Chunk()`
or the System.Interactive method `.Buffer()`.

#### CountDown
An additional overload has been added that returns a stream of `(TSource item, int? count)`.

#### FullJoin
This method has been obsoleted in favor of a new method `.FullOuterJoin()`, which
has more options and capabilities.

#### Lag/Lead
Additional overloads have been added for Lag/Lead that return streams of 
`(TSource cur, TSource lag/lead)`.

#### LeftJoin
This method has been obsoleted in favor of a new method `.LeftOuterJoin()`, which
has more options and capabilities.

#### MaxBy/MinBy
MaxBy and MinBy have been removed. These methods are superceded by PartialSort and DensePartialSort, and conflict with
new .NET 6.0 MaxBy/MinBy methods that operate slightly differently. PartialSort will ignore ties and return at most K
elements. DensePartialSort will return the top K groups of elements including ties.

#### OrderedMerge
This method has been obsoleted in favor of a new method `.FullOuterJoin()`, which
has more options and capabilities.

#### Pairwise
Pairwise has been removed as it overlaps behavior with both `.Lag()` and `.Window()`

#### PartialSort
The sorting behavior of `.PartialSort()` has been changed slightly, as it now uses
a stable sorting algorithm. This means that items that have the same value (or key)
will return in the same order that they were originally encountered in the stream.
This is a minor change from old sorting behavior.

#### Rank
The behavior and return type of Rank has been updated:
* Previously, Rank would rank according the highest value by default, opposite to the sorting. 
  * Now, Rank ranks according to the lowest value, matching the sorting
* Previously, Rank would return a simple list of integers matching the original items
  * Now, Rank returns a sorted list of items with their rank
* Previously, Rank would rank each group with a sequential rank value
  * Now, Rank ranks each group according to how many total items have been encountered
    in the stream thus far. 
  * DenseRank will rank each group with a seqential value.

All of these changes are made to bring Rank/DenseRank with the
behavior expressed for Rank/DenseRank in SQL systems (Sql Server, PostgreSQL, etc.)

#### RightJoin
This method has been obsoleted in favor of a new method `.RightOuterJoin()`, which
has more options and capabilities.

#### RunLengthEncode
The return type has been changed from a stream of `KeyValuePair<T, int>` 
to a stream of `(T value, int count)`

#### Scan
The `.Scan()` method has been renamed to `.ScanEx()` in order to avoid conflict 
with the System.Interactive version of the method. However, the behavior of the 
System.Interactive version differs slightly in that it does not return the 
seed/first element. The new `.ScanEx()` method maintains the original behavior.

## .NET Versions

Base library is supported on .NET Core 3.1 and .NET 5.0+.

## Documentation

Detailed documentation on the operators is available [here](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.html).
