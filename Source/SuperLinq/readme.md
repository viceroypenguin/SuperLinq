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

## Operators

### Aggregate

Applies multiple accumulators sequentially in a single pass over a sequence.

This method has 7 overloads.

### AggregateRight

Applies a right-associative accumulator function over a sequence.
This operator is the right-associative version of the Aggregate LINQ operator.

This method has 3 overloads.

### AtLeast

Determines whether or not the number of elements in the sequence is greater
than or equal to the given integer.

### AtMost

Determines whether or not the number of elements in the sequence is lesser
than or equal to the given integer.

### Backsert

Inserts the elements of a sequence into another sequence at a
specified index from the tail of the sequence, where zero always represents
the last position, one represents the second-last element, two represents
the third-last element and so on.

(Obsoleted in favor of Insert)

### BindByIndex

Extracts elements from a sequence according to a a sequence of indices.

This method has 2 overloads.

### Cartesian

Returns the Cartesian product of two or more sequences by combining each
element from the sequences and applying a user-defined projection to the
set.

This method has 7 overloads.

### Choose

Applies a function to each element of the source sequence and returns a new
sequence of result elements for source elements where the function returns a
couple (2-tuple) having a `true` as its first element and result as the
second.

### CollectionEqual

Determines if two sequences contain the same value and number of elements,
without requiring the elements to be in the same order.

This method has 2 overloads.

### CompareCount

Compares two sequences and returns an integer that indicates whether the
first sequence has fewer, the same or more elements than the second sequence.

### Consume

Completely consumes the given sequence. This method uses immediate execution,
and doesn't store any data during execution.

### CopyTo

Copies all of the elements of the given sequence into the specified list.

This method has 4 overloads

### CountBetween

Determines whether or not the number of elements in the sequence is between an
inclusive range of minimum and maximum integers.

### CountBy

Applies a key-generating function to each element of a sequence and returns a
sequence of unique keys and their number of occurrences in the original
sequence.

This method has 2 overloads.

### CountDown

Provides a countdown counter for a given count of elements at the tail of the
sequence where zero always represents the last element, one represents the
second-last element, two represents the third-last element and so on.

This method has 2 overloads.

### DensePartialSort

Executes a partial sort of the top K elements of a sequence, including ties. If K is less than the total number of
elements in the sequence, then this method will improve performance.

This method has 4 overloads.

### DensePartialSortBy

Executes a partial sort of the top K elements of a sequence, including ties, according to a key. If K is less than the
total number of elements in the sequence, then this method will improve performance.

This method has 4 overloads.

### DenseRank

Ranks each item in the sequence with ascending ordering according to 
the number of unique values encountered.

This method has 2 overloads.

### DenseRankBy

Ranks each item in the sequence with ascending ordering according to 
the number of unique values encountered.

This method has 2 overloads.

### DistinctBy (Hidden in .NET 6 in favor of the Framework version)

Returns all distinct elements of the given source, where "distinctness" is
determined via a projection and the default equality comparer for the
projected type.

This method has 2 overloads.

### ElementAt (pre-.NET 6)

Returns the element at a specified index in a sequence.

Backport from official sources to .net core 3.1 and .net 5.

### EndsWith

Determines whether the end of the first sequence is equivalent to the second
sequence.

This method has 2 overloads.

### EquiZip

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. An exception is thrown
if the input sequences are of different lengths.

This method has 3 overloads.

### Exactly

Determines whether or not the number of elements in the sequence is equals
to the given integer.

### ExceptBy (Hidden in .NET 6 in favor of the Framework version)

Returns the set of elements in the first sequence which aren't in the second
sequence, according to a given key selector.

This method has 2 overloads.

### Exclude

Excludes elements from a sequence starting at a given index

### FallbackIfEmpty

Returns the elements of a sequence and falls back to another if the original
sequence is empty.

This method has 2 overloads.

### FillBackward

Returns a sequence with each null reference or value in the source replaced
with the following non-null reference or value in that sequence.

This method has 3 overloads.

### FillForward

Returns a sequence with each null reference or value in the source replaced
with the previous non-null reference or value seen in that sequence.

This method has 3 overloads.

### FindIndex

Returns the index of the first element to satisfy a predicate.

This method has 3 overloads.

### FindLastIndex

Returns the index of a last element to satisfy a predicate.

This method has 3 overloads.

### Flatten

Flattens a sequence containing arbitrarily-nested sequences.

This method has 3 overloads.

### Fold

Returns the result of applying a function to a sequence with 1 to 16 elements.

This method has 16 overloads.

### From

Returns a sequence containing the values resulting from invoking (in order)
each function in the source sequence of functions.

This method has 4 overloads.

### FullGroupJoin

Performs a Full Group Join between the and sequences.

This method has 4 overloads.

### FullJoin

Performs a full outer join between two sequences.

This method has 4 overloads.

(Obsoleted in favor of FullOuterJoin)

### FullOuterJoin

Performs a full outer join of two sequences. A parameter is available to
select between join techniques of Loop, Hash, and Merge.

This method has 4 overloads.

### Generate

Returns a sequence of values consecutively generated by a generator function

### GenerateByIndex

Returns a sequence of values based on indexes

### GetShortestPath

Finds the shortest path between two points, using either Dijkstra's algorithm
or the A* algorithm (depending on whether a heuristic value is provided when 
getting state neighbors). 

The underlying map can be a plane, a graph, or any other state system provided
by the consumer, since traversal from one state to the next is done by the
consumer in the getNeighbors functor. 

This method has 4 overloads.

### GetShortestPathCost

Finds the cost of the shortest path between two points, using either Dijkstra's algorithm
or the A* algorithm (depending on whether a heuristic value is provided when 
getting state neighbors). 

The underlying map can be a plane, a graph, or any other state system provided
by the consumer, since traversal from one state to the next is done by the
consumer in the getNeighbors functor. 

This method has 4 overloads.

### GetShortestPaths

Finds the shortest path from a starting point to every other point in the map,
using either Dijkstra's algorithm.

The underlying map can be a plane, a graph, or any other state system provided
by the consumer, since traversal from one state to the next is done by the
consumer in the getNeighbors functor. The map must have a finite number of states
in order for this method to complete.

This method has 2 overloads.

### GroupAdjacent

Groups the adjacent elements of a sequence according to a specified key
selector function.

This method has 6 overloads.

### Index

Returns a sequence of where the key is the zero-based index of the value in
the source sequence.

This method has 2 overloads.

### IndexBy

Applies a key-generating function to each element of a sequence and returns
a sequence that contains the elements of the original sequence as well its
key and index inside the group of its key. An additional argument specifies
a comparer to use for testing equivalence of keys.

This method has 2 overloads.

### IndexOf

Returns the index of a particular value within a sequence.

This method has 3 overloads.

### InnerJoin

Performs an inner join of two sequences. A parameter is available to
select between join techniques of Hash, and Merge.

This method has 4 overloads.

### Insert

Inserts the elements of a sequence into another sequence at a specified index.

This method has 2 overloads.

### Interleave

Interleaves the elements of two or more sequences into a single sequence,
skipping sequences as they are consumed.

This method has 2 overloads.

### Lag

Produces a projection of a sequence by evaluating pairs of elements separated
by a negative offset.

This method has 2 overloads.

### LastIndexOf

Returns the last index of a particular value within a sequence.

This method has 3 overloads.

### Lead

Produces a projection of a sequence by evaluating pairs of elements separated
by a positive offset.

This method has 2 overloads.

### LeftJoin

Performs a left outer join between two sequences.

This method has 4 overloads.

(Obsoleted in favor of LeftOuterJoin)

### LeftOuterJoin

Performs a left outer join of two sequences. A parameter is available to
select between join techniques of Loop, Hash, and Merge.

This method has 4 overloads.

### MaxItems

Returns all of the elements of the given sequence that share the maximum value.

This method has 2 overloads.

### MaxItemsBy

Returns all of the elements of the given sequence that share the maximum value.

This method has 2 overloads.

### MinItems

Returns all of the elements of the given sequence that share the minimum value.

This method has 2 overloads.

### MinItemsBy

Returns all of the elements of the given sequence that share the minimum value.

This method has 2 overloads.

### Move

Returns a sequence with a range of elements in the source sequence
moved to a new offset.

### OrderBy

Sorts the elements of a sequence in a particular direction (ascending,
descending) according to a key.

This method has 2 overloads.

### OrderedMerge

Merges two ordered sequences into one. Where the elements equal in both
sequences, the element from the first sequence is returned in the resulting
sequence.

This method has 7 overloads.

(Obsoleted in favor of FullOuterJoin)

### Pad

Pads a sequence with default values if it is narrower (shorter in length) than
a given width.

This method has 3 overloads.

### PadStart

Pads a sequence with default values in the beginning if it is narrower
(shorter in length) than a given width.

This method has 3 overloads.

### PartialSort

Executes a partial sort of the top K elements of a sequence. If K is less than the total number of elements in the
sequence, then this method will improve performance.

This method has 4 overloads.

### PartialSortBy

Executes a partial sort of the top K elements of a sequence, according to a key. If K is less than the total number of
elements in the sequence, then this method will improve performance.

This method has 4 overloads.

### Partition

Partitions a sequence by a predicate, or a grouping by Boolean keys or up to 3
sets of keys.

This method has 10 overloads.

### Permutations

Generates a sequence of lists that represent the permutations of the original
sequence

### Pipe

Executes the given action on each element in the source sequence and yields it

### PreScan

Performs a pre-scan (exclusive prefix sum) on a sequence of elements

### Random

Returns an infinite sequence of random integers using the standard .NET random
number generator.

This method has 6 overloads.

### RandomDouble

Returns an infinite sequence of random double values between 0.0 and 1.0.

This method has 2 overloads.

### RandomSubset

Returns a sequence of a specified size of random elements from the original
sequence.

This method has 2 overloads.

### Rank

Ranks each item in the sequence with ascending ordering according to 
the total number of items encountered.

This method has 2 overloads.

### RankBy

Ranks each item in the sequence with ascending ordering according to 
the total number of items encountered.

This method has 2 overloads.

### Replace

Replaces a single value in a sequence at a specified index
with the given replacement value.

### Return

Returns a single-element sequence containing the item provided.

### RightOuterJoin

Performs a right outer join of two sequences. A parameter is available to
select between join techniques of Hash, and Merge.

This method has 4 overloads.

### RightJoin

Performs a right outer join between two sequences.

This method has 4 overloads.

(Obsoleted in favor of RightOuterJoin)

### RunLengthEncode

Run-length encodes a sequence by converting consecutive instances of the same
element into a `KeyValuePair<T, int>` representing the item and its occurrence
count.

This method has 2 overloads.

### ScanBy

Applies an accumulator function over sequence element keys, returning the keys
along with intermediate accumulator states.

This method has 2 overloads.

### ScanEx

Peforms a scan (inclusive prefix sum) on a sequence of elements.

This method has 2 overloads.

### ScanRight

Peforms a right-associative scan (inclusive prefix) on a sequence of elements.
This operator is the right-associative version of the Scan operator.

This method has 2 overloads.

### Segment

Divides a sequence into multiple sequences by using a segment detector based
on the original sequence.

This method has 3 overloads.

### Sequence

Generates a sequence of integral numbers within the (inclusive) specified range.

This method has 2 overloads.

### Shuffle

Returns a sequence of elements in random order from the original sequence.

This method has 2 overloads.

### SkipUntil

Skips items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last skipped

### Slice

Extracts elements from a sequence at a particular zero-based starting index

### SortedMerge

Merges two or more sequences that are in a common order (either ascending or
descending) into a single sequence that preserves that order.

This method has 4 overloads.

### SortedMergeBy

Merges two or more sequences that are in a common order (either ascending or descending)
according to a key into a single sequence that preserves that order.

This method has 4 overloads.

### SortedMergeDescending

Merges two or more sequences that are in a common descending order
into a single sequence that preserves that order.

This method has 2 overloads.

### SortedMergeByDescending

Merges two or more sequences that are in a common descending order
according to a key into a single sequence that preserves that order.

This method has 2 overloads.

### Split

Splits the source sequence by a separator.

This method has 12 overloads.

### StartsWith

Determines whether the beginning of the first sequence is equivalent to the
second sequence.

This method has 2 overloads.

### Subsets

Returns a sequence of representing all of the subsets of any size that are
part of the original sequence.

This method has 2 overloads.

### TagFirstLast

Returns a sequence resulting from applying a function to each element in the
source sequence with additional parameters indicating whether the element is
the first and/or last of the sequence

This method has 2 overloads.

### Take (pre-.NET 6)

Returns a specified range of contiguous elements from a sequence using the
range operator.

Backport from official sources to .net core 3.1 and .net 5.

### TakeEvery

Returns every N-th element of a source sequence

### TakeUntil

Returns items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last returned

### ThenBy

Performs a subsequent ordering of elements in a sequence in a particular
direction (ascending, descending) according to a key.

This method has 2 overloads.

### ToArrayByIndex

Creates an array from an IEnumerable<T> where a function is used to determine
the index at which an element will be placed in the array.

This method has 6 overloads.

### ToDataTable

Appends elements in the sequence as rows of a given object with a set of
lambda expressions specifying which members (property or field) of each
element in the sequence will supply the column values.

This method has 4 overloads.

### ToDelimitedString

Creates a delimited string from a sequence of values. The delimiter used
depends on the current culture of the executing thread.

This method has 15 overloads.

### ToDictionary

Creates a [dictionary][dict] from a sequence of [key-value pair][kvp] elements
or tuples of 2.

This method has 4 overloads.
### ToLookup

Creates a [lookup][lookup] from a sequence of [key-value pair][kvp] elements
or tuples of 2.

This method has 4 overloads.

### Transpose

Transposes the rows of a sequence into columns.

### TraverseBreadthFirst

Traverses a tree in a breadth-first fashion, starting at a root node and using
a user-defined function to get the children at each node of the tree.

### TraverseDepthFirst

Traverses a tree in a depth-first fashion, starting at a root node and using a
user-defined function to get the children at each node of the tree.

### Trace

Traces the elements of a source sequence for diagnostics.

This method has 3 overloads.

### TrySingle

Returns the only element of a sequence that has just one element. If the
sequence has zero or multiple elements, then returns a user-defined value
that indicates the cardinality of the result sequence.

This method has 2 overloads.

### Unfold

Returns a sequence generated by applying a state to the generator function,
and from its result, determines if the sequence should have a next element and
its value, and the next state in the recursive call.

### Where

Returns a sequence filtered by a matching sequence of boolean values. Useful if
you have a fixed list of boolean values that should be used to filter multiple 
similar sequences.

### Window

Processes a sequence into a series of subsequences representing a windowed
subset of the original

### WindowLeft

Creates a left-aligned sliding window over the source sequence of a given size.

### WindowRight

Creates a right-aligned sliding window over the source sequence of a given size.

### ZipLongest

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. The resulting sequence
will always be as long as the longest of input sequences where the
default value of each of the shorter sequence element types is used
for padding.

This method has 3 overloads.

### ZipMap

Returns a sequence of tuples, where each tuple contains both the original
element as well as a projection from that element.

### ZipShortest

Returns a projection of tuples, where each tuple contains the N-th
element from each of the argument sequences. The resulting sequence
is as short as the shortest input sequence.

This method has 3 overloads.


[dict]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.Dictionary-2
[kvp]: https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.KeyValuePair-2
[lookup]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.lookup-2
