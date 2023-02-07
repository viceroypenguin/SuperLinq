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

### AggregateRight

Applies a right-associative accumulator function over a sequence.
This operator is the right-associative version of the Aggregate LINQ operator.

This method has 9 overloads.

### AssertCount

Asserts that a source sequence contains a given count of elements.

### AtLeast

Determines whether or not the number of elements in the sequence is greater
than or equal to the given integer.

### AtMost

Determines whether or not the number of elements in the sequence is lesser
than or equal to the given integer.

### BindByIndex

Extracts elements from a sequence according to a a sequence of indices.

This method has 2 overloads.

### Choose

Applies a function to each element of the source sequence and returns a new
sequence of result elements for source elements where the function returns a
couple (2-tuple) having a `true` as its first element and result as the
second.

This method has 2 overloads.

### CollectionEqual

Determines if two sequences contain the same value and number of elements,
without requiring the elements to be in the same order.

This method has 2 overloads.

### CompareCount

Compares two sequences and returns an integer that indicates whether the
first sequence has fewer, the same or more elements than the second sequence.

### ConcurrentMerge

Merges multiple async sequences into a single async sequence, iterating the
sequences in parallel and returning values as they are received.

This method has 3 overloads.

### Consume

Completely consumes the given sequence. This method does not store any data 
during execution.

### CopyTo

Copies all of the elements of the given sequence into the specified list.

This method has 2 overloads.

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

### DistinctBy

Returns all distinct elements of the given source, where "distinctness" is
determined via a projection and the default equality comparer for the
projected type.

This method has 2 overloads.

### ElementAt

Returns the element at a specified index in a sequence.

### EndsWith

Determines whether the end of the first sequence is equivalent to the second
sequence.

This method has 4 overloads.

### Exactly

Determines whether or not the number of elements in the sequence is equals
to the given integer.

### ExceptBy

Returns the set of elements in the first sequence which aren't in the second
sequence, according to a given key selector.

This method has 2 overloads.

### FallbackIfEmpty

Returns the elements of a sequence and falls back to another if the original
sequence is empty.

This method has 3 overloads.

### FillBackward

Returns a sequence with each null reference or value in the source replaced
with the following non-null reference or value in that sequence.

This method has 7 overloads.

### FillForward

Returns a sequence with each null reference or value in the source replaced
with the previous non-null reference or value seen in that sequence.

This method has 7 overloads.

### FindIndex

Returns the index of the first element to satisfy a predicate.

This method has 3 overloads.

### FindLastIndex

Returns the index of a last element to satisfy a predicate.

This method has 3 overloads.

### FullOuterJoin

Performs a full outer join of two sequences. A parameter is available to
select between join techniques of Hash, and Merge.

This method has 4 overloads.

### Fold

Returns the result of applying a function to a sequence with 1 to 16 elements.

This method has 16 overloads.

### From

Returns a sequence containing the values resulting from invoking (in order)
each function in the source sequence of functions.

This method has 4 overloads.

### Generate

Returns a sequence of values consecutively generated by a generator function

### GenerateByIndex

Returns a sequence of values based on indexes

This method has 3 overloads.

### GetShortestPath

Finds the shortest path between two points, using either Dijkstra's algorithm
or the A* algorithm (depending on whether a heuristic value is provided when 
getting state neighbors). 

The underlying map can be a plane, a graph, or any other state system provided
by the consumer, since traversal from one state to the next is done by the
consumer in the getNeighbors functor. 

This method has 8 overloads.

### GetShortestPathCost

Finds the cost of the shortest path between two points, using either Dijkstra's 
algorithm or the A* algorithm (depending on whether a heuristic value is provided 
when getting state neighbors). 

The underlying map can be a plane, a graph, or any other state system provided
by the consumer, since traversal from one state to the next is done by the
consumer in the getNeighbors functor. 

This method has 8 overloads.

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
select between join techniques of Loop, Hash, and Merge.

This method has 4 overloads.

### Insert

Inserts the elements of a sequence into another sequence at a specified index.

This method has 2 overloads.

### Interleave

Interleaves the elements of two or more sequences into a single sequence,
skipping sequences as they are consumed.

This method has 2 overloads.

### LastIndexOf

Returns the last index of a particular value within a sequence.

This method has 3 overloads.

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

### OrderBy

Sorts the elements of a sequence in a particular direction (ascending,
descending) according to a key.

This method has 2 overloads.

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

### Random

Returns an infinite sequence of random integers using the standard .NET random
number generator.

This method has 6 overloads.

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

### RightOuterJoin

Performs a right outer join of two sequences. A parameter is available to
select between join techniques of Hash, and Merge.

This method has 4 overloads.

### RunLengthEncode

Run-length encodes a sequence by converting consecutive instances of the same
element into a `KeyValuePair<T, int>` representing the item and its occurrence
count.

This method has 2 overloads.

### ScanBy

Applies an accumulator function over sequence element keys, returning the keys
along with intermediate accumulator states.

This method has 6 overloads.

### ScanEx

Performs a scan (inclusive prefix sum) on a sequence of elements.

This method has 2 overloads.

### ScanRight

Performs a right-associative scan (inclusive prefix) on a sequence of elements.
This operator is the right-associative version of the Scan operator.

This method has 6 overloads.

### Segment

Divides a sequence into multiple sequences by using a segment detector based
on the original sequence.

This method has 6 overloads.

### Sequence

Generates a sequence of integral numbers within the (inclusive) specified range.

This method has 2 overloads.

### SkipUntil

Skips items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last skipped

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

This method has 4 overloads.

### TagFirstLast

Returns a sequence resulting from applying a function to each element in the
source sequence with additional parameters indicating whether the element is
the first and/or last of the sequence

This method has 2 overloads.

### Take

Returns a specified range of contiguous elements from a sequence using the
range operator.

### TakeEvery

Returns every N-th element of a source sequence

### TakeUntil

Returns items from the input sequence until the given predicate returns true
when applied to the current source item; that item will be the last returned

### ThenBy

Performs a subsequent ordering of elements in a sequence in a particular
direction (ascending, descending) according to a key.

This method has 2 overloads.

### TraverseBreadthFirst

Traverses a tree in a breadth-first fashion, starting at a root node and using
a user-defined function to get the children at each node of the tree.

### TraverseDepthFirst

Traverses a tree in a depth-first fashion, starting at a root node and using a
user-defined function to get the children at each node of the tree.

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
