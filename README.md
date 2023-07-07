# SuperLinq

| Name            | Status | History |
| :---            | :---   | :---    |
| GitHub Actions  | ![Build](https://github.com/viceroypenguin/SuperLinq/actions/workflows/build.yml/badge.svg) | [![GitHub Actions Build History](https://buildstats.info/github/chart/viceroypenguin/superlinq?branch=master&includeBuildsFromPullRequest=false)](https://github.com/viceroypenguin/SuperLinq/actions) |

[![GitHub release](https://img.shields.io/github/release/viceroypenguin/superlinq.svg)](https://GitHub.com/viceroypenguin/superlinq/releases/)
[![GitHub license](https://img.shields.io/github/license/viceroypenguin/superlinq.svg)](https://github.com/viceroypenguin/superlinq/blob/master/license.txt) 
[![GitHub issues](https://img.shields.io/github/issues/viceroypenguin/superlinq.svg)](https://GitHub.com/viceroypenguin/superlinq/issues/) 
[![GitHub issues-closed](https://img.shields.io/github/issues-closed/viceroypenguin/superlinq.svg)](https://GitHub.com/viceroypenguin/superlinq/issues?q=is%3Aissue+is%3Aclosed) 

LINQ to Objects is missing a few desirable features.

This project enhances LINQ to Objects with extra methods, in a manner which
keeps to the spirit of LINQ.

Methods are provided to extend both `IEnumerable<T>` (via SuperLinq package)
and `IAsyncEnumerable<T>` (via SuperLinq.Async package). 

## SuperLinq

SuperLinq is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq/).
[![NuGet Badge](https://buildstats.info/nuget/SuperLinq)](https://www.nuget.org/packages/SuperLinq/)

The documentation for the SuperLinq methods can be found [here](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.html).

## SuperLinq.Async

SuperLinq.Async is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq.async/). 
[![NuGet Badge](https://buildstats.info/nuget/SuperLinq.Async)](https://www.nuget.org/packages/SuperLinq.Async/)

The documentation for the SuperLinq.Async methods can be found [here](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.html).

## Operators

<details>
<summary>
Sorting Data
</summary>

A sorting operation orders the elements of a sequence based on one or more attributes. The first sort
criterion performs a primary sort on the elements. By specifying a second sort criterion, you can sort the elements
within each primary sort group.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| OrderBy			 | Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.OrderBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.OrderBy.html) |
| ThenBy			 | Performs a subsequent ordering of elements in a sequence in a particular direction (ascending, descending) according to a key | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ThenBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ThenBy.html) |
| PartialSort		 | Executes a partial sort of the top `N` elements of a sequence. If `N` is less than the total number of elements in the sequence, then this method will improve performance. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.PartialSort.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.PartialSort.html) |
| PartialSortBy      | Executes a partial sort of the top `N` elements of a sequence according to a key. If `N` is less than the total number of elements in the sequence, then this method will improve performance. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.PartialSortBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.PartialSortBy.html) |
| DensePartialSort	 | Executes a partial sort of the top `N` elements of a sequence, including ties. If `N` is less than the total number of elements in the sequence, then this method will improve performance. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DensePartialSort.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DensePartialSort.html) |
| DensePartialSortBy | Executes a partial sort of the top `N` elements of a sequence, including ties according to a key. If `N` is less than the total number of elements in the sequence, then this method will improve performance. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DensePartialSortBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DensePartialSortBy.html) |
| Shuffle			 | Sorts the elements of a sequence in a random order. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Shuffle.html) | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| RandomSubset		 | Sorts a given number of elements of a sequence in a random order. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.RandomSubset.html) | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |

</details>

<details>
<summary>
Set Operations
</summary>

Set operations in LINQ refer to query operations that produce a result set that is based on the presence or absence of
equivalent elements within the same or separate collections (or sets).

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| DistinctBy         | Removes duplicate values from a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DistinctBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DistinctBy.html) |
| ExceptBy	         | Returns the set difference, which means the elements of one collection that do not appear in a second collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ExceptBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ExceptBy.html) |

</details>

<details>
<summary>
Filtering Data
</summary>

Filtering refers to the operation of restricting the result set to contain only those elements that satisfy a specified
condition. It is also known as selection.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Choose			 | Filters a sequence based on a projection method that returns a tuple containing `bool` value and a new projected value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Choose.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Choose.html) |
| Where              | Filters a sequence of values based on an enumeration of boolean values. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Where.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Where.html) |
| WhereLead          | Filters a sequence of values based on a predicate evaluated on the current value and a leading value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.WhereLead.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.WhereLead.html) |
| WhereLag           | Filters a sequence of values based on a predicate evaluated on the current value and a lagging value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.WhereLag.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.WhereLag.html) |

</details>

<details>
<summary>
Quantifier Operations
</summary>

Quantifier operations return a boolean value that indicates whether the sequence length matches some criteria.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| AtLeast			 | Determines whether or not the number of elements in the sequence is greater than or equal to the given integer. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.AtLeast.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.AtLeast.html) |
| AtMost			 | Determines whether or not the number of elements in the sequence is lesser than or equal to the given integer. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.AtMost.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.AtMost.html) |
| CountBetween		 | Determines whether or not the number of elements in the sequence is between an inclusive range of minimum and maximum integers. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CountBetween.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CountBetween.html) |
| Exactly			 | Determines whether or not the number of elements in the sequence is equals to the given integer. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Exactly.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Exactly.html) |
| TrySingle			 | Determines the cardinality of the sequence in the set `{ 0, 1, >1 }`. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TrySingle.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TrySingle.html) |
| HasDuplicates		 | Determines whether the sequence contains duplicates | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.HasDuplicates.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.HasDuplicates.html) |

</details>

<details>
<summary>
Projection Operations
</summary>

Projection refers to the operation of transforming an object into a new form that may contain related information.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| EquiZip			 | Joins the corresponding elements of up to four sequences producing a sequence of tuples containing them, asserting that all sequences have exactly the same length. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.EquiZip.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.EquiZip.html) |
| ZipLongest		 | Joins the corresponding elements of up to four sequences producing a sequence of tuples containing them, using `default` values for sequences that are shorter than the longest sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ZipLongest.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ZipLongest.html) |
| ZipShortest		 | Joins the corresponding elements of up to four sequences producing a sequence of tuples containing them, which has the same length as the shortest sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ZipShortest.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ZipShortest.html) |
| CountDown			 | Provides a countdown counter for a given count of elements at the tail of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CountDown.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CountDown.html) |
| TagFirstLast		 | Provides `bool` values indicating for each element whether it is the first or last element of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TagFirstLast.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TagFirstLast.html) |
| Index				 | Provides an `int` value indicating the current index of each element of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Index.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Index.html) |
| IndexBy			 | Provides an `int` value indicating the current index of each element of the sequence within a group of items defined by a common attribute. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.IndexBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.IndexBy.html) |
| Lag				 | Joins each element of the sequence with n-th previous element of the same sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Lag.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Lag.html) |
| Lead				 | Joins each element of the sequence with n-th next element of the same sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Lead.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Lead.html) |
| Rank				 | Provides an `int` value indicating the current rank of each element of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Rank.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Rank.html) |
| RankBy			 | Provides an `int` value indicating the current rank of each element of the sequence according to a key. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.RankBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.RankBy.html) |
| DenseRank			 | Provides an `int` value indicating the current rank of each element of the sequence, counting ties as a single element. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DenseRank.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DenseRank.html) |
| DenseRankBy		 | Provides an `int` value indicating the current rank of each element of the sequence according to a key, counting ties as a single element. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DenseRankBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DenseRankBy.html) |
| Evaluate			 | Transforms a sequence of functions to a sequence of values returned by the functions. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Evaluate.html) | N/A[^1] |
| ZipMap			 | Applies a function to each element in a sequence and returns a sequence of tuples containing both the original item as well as the function result. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ZipMap.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ZipMap.html) |

</details>

<details>
<summary>
Partitioning Data
</summary>

Partitioning in LINQ refers to the operation of dividing an input sequence into one or more sections. 

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| TakeEvery			 | Takes every n-th element of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TakeEvery.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TakeEvery.html) |
| Take				 | Takes elements from a specified range of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Take.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Take.html) |
| Exclude			 | Excludes elements from a specified range of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Exclude.html) | ⏱([#10](https://github.com/viceroypenguin/SuperLinq/issues/10)) |
| Move				 | Moves elements from a specified range of the sequence to a new index in the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Move.html) | ⏱([#27](https://github.com/viceroypenguin/SuperLinq/issues/27)) |
| SkipUntil			 | Skips elements based on a predicate function until an element satisfies the condition, skipping this element as well. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.SkipUntil.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.SkipUntil.html) |
| TakeUntil			 | Takes elements based on a predicate function until an element satisfies the condition, taking this element as well. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TakeUntil.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TakeUntil.html) |
| Batch				 | Splits the elements of a sequence into chunks of a specified maximum size. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Batch.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Batch.html) |
| Buffer			 | Splits the elements of a sequence into chunks of a specified maximum size, where the chunks may be overlapping or have gaps. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Buffer.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Buffer.html) |
| Partition			 | Splits the elements of a sequence based on a common attribute and known key values. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Partition.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Partition.html) |
| Segment			 | Splits the elements of a sequence based on a condition function. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Segment.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Segment.html) |
| Split				 | Splits the elements of a sequence based on a separator value that is not returned. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Split.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Split.html) |
| Window			 | Returns a sequence of sequential windows of size `N` over the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Window.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Window.html) |
| WindowLeft		 | Returns a sequence of sequential windows of up to size `N` over the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.WindowLeft.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.WindowLeft.html) |
| WindowRight		 | Returns a sequence of sequential windows of up to size `N` over the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.WindowRight.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.WindowRight.html) |

</details>

<details>
<summary>
Join Operations
</summary>

A join of two data sources is the association of objects in one data source with objects that share a common attribute in another data source.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Cartesian			 | Executes a cartesian product (join without any key) of up to eight sequences. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Cartesian.html) | N/A[^1] |
| FullGroupJoin		 | Joins two sequences based on key selector functions, returning two lists containing the values on each side that match according to the key. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FullGroupJoin.html) | N/A[^1] |
| FullOuterJoin		 | Joins two sequences based on key selector functions, returning `default` values if either sequence does not have a matching key. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FullOuterJoin.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FullOuterJoin.html) |
| InnerJoin			 | Joins two sequences based on key selector functions. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.InnerJoin.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.InnerJoin.html) |
| LeftOuterJoin		 | Joins two sequences based on key selector functions, returning `default` values if the second sequence does not have a matching key. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.LeftOuterJoin.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.LeftOuterJoin.html) |
| RightOuterJoin	 | Joins two sequences based on key selector functions, returning `default` values if the first sequence does not have a matching key. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.RightOuterJoin.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.RightOuterJoin.html) |

</details>

<details>
<summary>
Grouping Data
</summary>

Grouping refers to the operation of putting data into groups so that the elements in each group share a common attribute.

### Methods

| Method Name          | Description | Sync doc | Async doc |
| -----------          | --- | --- | --- |
| DistinctUntilChanged | Takes the first element of each adjacent group of elements that share a common attribute. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DistinctUntilChanged.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DistinctUntilChanged.html) |
| GroupAdjacent		   | Groups adjacent elements that share a common attribute. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.GroupAdjacent.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.GroupAdjacent.html) |
| RunLengthEncode	   | Takes the first element of each adjacent group of equivalent elements along with the number of elements in the group. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.RunLengthEncode.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.RunLengthEncode.html) |

</details>

<details>
<summary>
Generation Operations
</summary>

Generation refers to creating a new sequence of values.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Generate			 | Generates a sequence based on a seed value and subsequent executions of a generator function. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Generate.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Generate.html) |
| From				 | Generates a sequence from the results of executing one or more provided functions. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.From.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.From.html) |
| Return			 | Generates a single-element sequence containing the provided value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Return.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Return.html) |
| Sequence			 | Generates a sequence of numbers between a starting and ending value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Sequence.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Sequence.html) |
| Range				 | Generates a sequence of numbers. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Range.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Range.html) |
| Random			 | Generates a sequence of random `int` values. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Random.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Random.html) |
| RandomDouble		 | Generates a sequence of random `double` values. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.RandomDouble.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.RandomDouble.html) |
| Repeat			 | Generates a sequence that infinitely repeats the input sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Repeat.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Repeat.html) |
| DoWhile			 | Generates a sequence that repeats the input sequence at least once, as long as a given condition is `true`. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.DoWhile.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.DoWhile.html) |
| While				 | Generates a sequence that repeats the input sequence as long as a given condition is `true`. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.While.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.While.html) |
| Retry				 | Generates a sequence that repeats the input sequence as long as the input sequence encounters an error. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Retry.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Retry.html) |
| Throw				 | Generates a sequence that throws an exception upon enumeration. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Throw.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Throw.html) |
| Permutations		 | Generates a sequence of every possible permutation of the input sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Permutations.html) | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| Subsets			 | Generates a sequence of every possible subset of a given size of the input sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Subsets.html) | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| BindByIndex		 | Generates a sequence from another sequence by selecting elements at given indices. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.BindByIndex.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.BindByIndex.html) |
| ToArrayByIndex	 | Generates a sequence based on an index selector function applied to each element. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ToArrayByIndex.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ToArrayByIndex.html) |
| FallbackIfEmpty	 | Replaces an empty sequence with a default sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FallbackIfEmpty.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FallbackIfEmpty.html) |
| FillBackward		 | Generates a sequence where missing values are replaced with the next good value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FillBackward.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FillBackward.html) |
| FillForward		 | Generates a sequence where missing values are replaced with the last good value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FillForward.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FillForward.html) |
| Pad				 | Generates a sequence with a minimum length, providing default values for missing elements. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Pad.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Pad.html) |
| PadStart			 | Generates a sequence with a minimum length, providing default values for missing elements.  | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.PadStart.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.PadStart.html) |

</details>

<details>
<summary>
Selection Operations
</summary>

Selection operations choose which sequence to based on a criteria evaluated at the time of enumeration.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Amb				 | Enumerates the first sequence to return the first value. | N/A[^2] | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Amb.html) |
| Case				 | Enumerates a sequence chosen by a function executed at the time of enumeration. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Case.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Case.html) |
| If				 | Enumerates a sequence chosen by a condition function executed at the time of enumeration. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.If.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.If.html) |
| Defer				 | Enumerates a sequence returned by a function executed at the time of enumeration. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Defer.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Defer.html) |
| Using				 | Creates a disposable resource at the time of execution and enumerates a sequence based on the resource. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Using.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Using.html) |

</details>

<details>
<summary>
Equality Operations
</summary>

Equality operations return a boolean value that indicates whether two sequences match according to some criteria.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| CollectionEqual	 | Determines whether two sequences contain the same elements in any order. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CollectionEqual.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CollectionEqal.html) |
| CompareCount		 | Determines whether two sequences have the same length. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CompareCount.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CompareCount.htl) |
| StartsWith		 | Determines whether a sequence contains another sequence at the start. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.StartsWith.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.StartsWith.html) |
| EndsWith			 | Determines whether a sequence contains another sequence at the end. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.EndsWith.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.EndsWith.html) |

</details>

<details>
<summary>
Element Operations
</summary>

Element operations return or find the index of a single, specific element from a sequence.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| ElementAt			 | Returns the element at a specified index in a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ElementAt.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ElementAt.html) |
| ElementAtOrDefault | Returns the element at a specified index in a collection or a default value if the index is out of range. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ElementAtOrDefault.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ElementAtOrDefault.html) |
| FindIndex			 | Returns the index of the first element that satisfies a given criteria. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FindIndex.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FindIndex.html) |
| FindLastIndex		 | Returns the index of the last element that satisfies a given criteria.  | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.FindLastIndex.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.FindLastIndex.html) |
| IndexOf			 | Returns the first index of the element. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.IndexOf.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.IndexOf.html) |
| LastIndexOf		 | Returns the last index of the element.  | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.LastIndexOf.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.LastIndexOf.html) |

</details>

<details>
<summary>
Converting Data Types
</summary>

Element operations return or find a single, specific element from a sequence.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| CopyTo			 | Copies the elements from a sequence into a provided list-like structure. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CopyTo.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CopyTo.html) |
| ToDataTable		 | Converts a sequence of objects into a `DataTable` object. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ToDataTable.html) | N/A[^3] |
| ToDelimitedString	 | Converts a sequence of elements to a delimited string containing the `string` form of each element. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ToDelimitedString.html) | N/A[^1] |
| ToDictionary		 | Converts a sequence of `KeyValuePair` or `(key, value)` tuples into a `Dictionary<,>` | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ToDictionary.html) | N/A[^1] |
| ToLookup			 | Converts a sequence of `KeyValuePair` or `(key, value)` tuples into a `Lookup<,>` | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ToLookup.html) | N/A[^1] |
| Transpose			 | Transposes a jagged two-dimensional array of elements, such that, for example, each row of the returned 2d array contains the first element of each inner array of the input. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Transpose.html) | N/A[^1] |


</details>

<details>
<summary>
Concatenation Operations
</summary>

Concatenation refers to the operation of appending one sequence to another.

### Methods

| Method Name             | Description | Sync doc | Async doc |
| -----------             | --- | --- | --- |
| ConcurrentMerge		  | Merges the elements of two or more asynchronous sequences into a single sequence. | N/A[^2] | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ConcurrentMerge.html) |
| Flatten				  | Flattens a sequence containing arbitrarily-nested sequences into a single sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Flatten.html) | N/A[^4] |
| Insert				  | Inserts the elements of a sequence into another sequence at a specified index. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Insert.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Insert.html) |
| Interleave			  | Interleaves the elements of two or more sequences into a single sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Interleave.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Interleave.html) |
| Replace				  | Replaces a range of elements in a sequence with the elements from another sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Replace.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Replace.html) |
| Catch					  | Concatenates one or more sequences until one is completely enumerated without error. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Catch.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Catch.html) |
| OnErrorResumeNext		  | Concatenates one or more sequences regardless of if an error occurs in any of them. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.OnErrorResumeNext.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.OnErrorResumeNext.html) |
| SortedMerge			  | Merges already-sorted sequences into a new correctly-sorted sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.SortedMerge.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.SortedMerge.html) |
| SortedMergeBy			  | Merges already-sorted sequences into a new correctly-sorted sequence according to a key value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.SortedMergeBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.SortedMergeBy.html) |
| SortedMergeByDescending | Merges already-sorted sequences into a new correctly-sorted sequence according to a key value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.SortedMergeByDescending.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.SortedMergeByDescending.html) |
| SortedMergeDescending	  | Merges already-sorted sequences into a new correctly-sorted sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.SortedMergeDescending.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.SortedMergeDescending.html) |

</details>

<details>
<summary>
Aggregation Operations
</summary>

An aggregation operation computes a single value from a collection of values.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Aggregate			 | Performs two or more custom aggregation operation on the values of a sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Aggregate.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Aggregate.html) |
| AggregateRight	 | Performs a custom aggregation on a sequence, starting from the end. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.AggregateRight.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.AggregateRight.html) |
| Scan				 | Performs a custom aggregation on a sequence, returning the intermediate aggregate value for each element in the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Scan.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Scan.html) |
| PreScan			 | Performs a custom aggregation on a sequence, returning the pre-intermediate aggregate value for each element in the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.PreScan.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.PreScan.html) |
| ScanBy			 | Performs a custom aggregation on each group of elements that share a common attribute, returning the intermediate aggregate value for each element in the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ScanBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ScanBy.html) |
| ScanRight			 | Performs a custom aggregation on a sequence, returning the intermediate aggregate value for each element in the sequence, starting from the end of the sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ScanRight.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ScanRight.html) |
| CountBy			 | Groups elements that share a common attribute and returns a sequence of attributes along with the number of elements in each group. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.CountBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.CountBy.html) |
| Fold				 | Collects the elements of an up to 16 element sequence and projects them into a single value. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Fold.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Fold.html) |
| MaxItems			 | Determines the list of maximum values in a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.MaxItems.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.MaxItems.html) |
| MaxItemsBy		 | Determines the list of maximum values in a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.MaxItemsBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.MaxItemsBy.html) |
| MinItems			 | Determines the list of minimum values in a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.MinItems.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.MinItems.html) |
| MinItemsBy		 | Determines the list of minimum values in a collection. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.MinItemsBy.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.MinItemsBy.html) |

</details>

<details>
<summary>
Buffering Operations
</summary>

Buffering operations allow storing and sharing data from a sequence to be used in a source-friendly wawy.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Memoize			 | Lazily cache the elements of a sequence to be used in multiple re-iterations. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Memoize.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Memoize.html) |
| Publish			 | Share a sequence among multiple consumers, such that each consumer can receive every element returned by the source since the consumer began enumerating. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Publish.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Publish.html) |
| Share				 | Share a sequence among multiple consumers, such that each element returned by the source is only obtained by a single consumer. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Share.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Share.html) |

</details>

<details>
<summary>
Sequence Operations
</summary>

Sequence operations perform some operation on a sequence as a whole.

### Methods

| Method Name        | Description | Sync doc | Async doc |
| -----------        | --- | --- | --- |
| Consume			 | Immediately consumes and discards a sequence, allowing a lazy sequence that has side-effects to be completed. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Consume.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Consume.html) |
| ForEach			 | Immediately executes an action on every element in a sequence. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.ForEach.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.ForEach.html) |
| Do				 | Performs an action on each element in a sequence as it is enumerated. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Do.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Do.html) |
| Timeout			 | Throws an exception if the async processing of an element takes longer than a specified timeout. | N/A[^2] | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Timeout.html) |
| AssertCount		 | Evalutes the length of a sequence as it is enumerated and validates that the length is the same as expected. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.AssertCount.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.AssertCount.html) |
| Finally			 | Executes an action when a sequence finishes enumerating, regardless of whether or not the sequence completed successfully. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.Finally.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.Finally.html) |

</details>

<details>
<summary>
Tree Operations
</summary>

Tree operations allow processing tree-like data structures in a data-agnostic form.

### Methods

| Method Name		   | Description | Sync doc | Async doc |
| -----------		   | --- | --- | --- |
| TraverseBreadthFirst | Returns every node in a tree-like virtual structure expressed by the input methods in a breadth-first manner. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TraverseBreadthFirst.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TraverseBreadthFirst.html) |
| TraverseDepthFirst   | Returns every node in a tree-like virtual structure expressed by the input methods in a depth-first manner. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.TraverseDepthFirst.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.TraverseDepthFirst.html) |
| GetShortestPath	   | Determine the shortest path through a graph-like virtual structure using Dijkstra's algorithm or A*. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.GetShortestPath.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.GetShortestPath.html) |
| GetShortestPathCost  | Determine the cost of shortest path through a graph-like virtual structure using Dijkstra's algorithm or A*. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.GetShortestPathCost.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.GetShortestPathCost.html) |
| GetShortestPaths	   | Determine the shortest cost to every node in a graph-like virtual structure using Dijkstra's algorithm. | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.SuperEnumerable.GetShortestPaths.html) | [link](https://viceroypenguin.github.io/SuperLinq/api/SuperLinq.Async.AsyncSuperEnumerable.GetShortestPaths.html) |

</details>

#### Footnotes:

[^1]: Not yet implemented; open a ticket if operator is desired
[^2]: Async operator without a sync equivalent
[^3]: `DataTable` is an no-longer relevant data structure, so this method is only kept for posterity and will not be migrated to a datatable.
[^4]: Will not be implemented, due to complex nature of flattening async sequences into a single sequence
