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

The documentation for the SuperLinq methods can be found [here](Source/SuperLinq/readme.md).

## SuperLinq.Async

SuperLinq.Async is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq.async/). 
[![NuGet Badge](https://buildstats.info/nuget/SuperLinq.Async)](https://www.nuget.org/packages/SuperLinq.Async/)

The documentation for the SuperLinq.Async methods can be found [here](Source/SuperLinq.Async/readme.md).

## Operator Support

| Operator | SuperLinq | SuperLinq.Async |
|-|:-:|:-:|
| Acquire                         | ❌<br/>(Removed[^1]) | ❌ |
| AggregateRight				  | ✔️ | ✔️ |
| AtLeast						  | ✔️ | ✔️ |
| AtMost						  | ✔️ | ✔️ |
| Backsert						  | ⚠️[^2] | ❌[^2] |
| Batch							  | ⚠️[^3] | ❌[^3] |
| BindByIndex					  | ✔️ | ✔️ |
| Cartesian						  | ✔️ | ❌ |
| Choose						  | ✔️ | ✔️ |
| CountBetween					  | ✔️ | ✔️ |
| CollectionEqual				  | ✔️ | ✔️ |
| CompareCount					  | ✔️ | ✔️ |
| ConcurrentMerge				  | ❌ | ✔️ |
| CopyTo						  | ✔️ | ✔️ |
| CountBy						  | ✔️ | ✔️ |
| CountDown						  | ✔️ | ✔️ |
| Consume						  | ✔️ | ✔️ |
| DensePartialSort				  | ✔️ | ✔️ |
| DensePartialSortBy			  | ✔️ | ✔️ |
| DenseRank						  | ✔️ | ✔️ |
| DenseRankBy					  | ✔️ | ✔️ |
| DistinctBy					  | ✔️ | ✔️ |
| ElementAt						  | ✔️ | ✔️ |
| EndsWith						  | ✔️ | ✔️ |
| EquiZip						  | ✔️ | ✔️ |
| Evaluate						  | ✔️ | ❌ |
| Exactly						  | ✔️ | ✔️ |
| ExceptBy						  | ✔️ | ✔️ |
| Exclude						  | ✔️ | ⏱([#10](https://github.com/viceroypenguin/SuperLinq/issues/10)) |
| FallbackIfEmpty				  | ✔️ | ✔️ |
| FillBackward					  | ✔️ | ✔️ |
| FillForward					  | ✔️ | ✔️ |
| FindIndex						  | ✔️ | ✔️ |
| Flatten						  | ✔️ | ❌ |
| Fold							  | ✔️ | ❌ |
| From							  | ✔️ | ✔️ |
| FullGroupJoin					  | ✔️ | ❌ |
| FullJoin						  | ⚠️[^9] | ❌ |
| FullOuterJoin					  | ✔️ | ✔️ |
| Generate						  | ✔️ | ✔️ |
| GenerateByIndex				  | ✔️ | ✔️ |
| GetShortestPath				  | ✔️ | ✔️ |
| GetShortestPathCost			  | ✔️ | ✔️ |
| GetShortestPaths				  | ✔️ | ✔️ |
| GroupAdjacent					  | ✔️ | ✔️ |
| Index							  | ✔️ | ✔️ |
| IndexBy						  | ✔️ | ✔️ |
| IndexOf						  | ✔️ | ✔️ |
| InnerJoin						  | ✔️ | ✔️ |
| Insert						  | ✔️ | ✔️ |
| Interleave					  | ✔️ | ✔️ |
| Lag							  | ✔️ | ✔️ |
| LastIndexOf					  | ✔️ | ✔️ |
| Lead							  | ✔️ | ✔️ |
| LeftJoin						  | ⚠️[^9] | ❌ |
| LeftOuterJoin					  | ✔️ | ✔️ |
| MaxBy							  | ❌<br/>(Removed[^8]) | ❌[^8] |
| MaxItems						  | ✔️ | ✔️ |
| MaxItemsBy					  | ✔️ | ✔️ |
| MinBy							  | ❌<br/>(Removed[^8]) | ❌[^8] |
| MinItems						  | ✔️ | ✔️ |
| MinItemsBy					  | ✔️ | ✔️ |
| Move							  | ✔️ | ⏱([#27](https://github.com/viceroypenguin/SuperLinq/issues/27)) |
| OrderBy						  | ✔️ | ✔️ |
| OrderedMerge					  | ⚠️[^9] | ❌ |
| Pad							  | ✔️ | ✔️ |
| PadStart						  | ✔️ | ✔️ |
| PartialSort					  | ✔️ | ✔️ |
| PartialSortBy					  | ✔️ | ✔️ |
| Partition						  | ✔️ | ✔️ |
| Permutations					  | ✔️ | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| Pipe							  | ✔️[^4] | ❌[^4] |
| PreScan						  | ✔️ | ✔️ |
| Random						  | ✔️ | ✔️ |
| RandomDouble					  | ✔️ | ✔️ |
| RandomSubset					  | ✔️ | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| Rank							  | ✔️[^5] | ✔️ |
| RankBy						  | ✔️[^5] | ✔️ |
| Replace						  | ✔️ | ✔️ |
| Return						  | ✔️ | ❌ |
| RightJoin						  | ⚠️[^9] | ❌ |
| RightOuterJoin				  | ✔️ | ✔️ |
| RunLengthEncode				  | ✔️ | ✔️ |
| Scan							  | ❌<br/>(Removed[^6]) | ❌[^6] |
| ScanBy						  | ✔️ | ✔️ |
| ScanEx						  | ✔️ | ✔️ |
| ScanRight						  | ✔️ | ✔️ |
| Segment						  | ✔️ | ✔️ |
| Sequence						  | ✔️ | ✔️ |
| Shuffle						  | ✔️ | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| SkipUntil						  | ✔️ | ✔️ |
| Slice							  | ⚠️[^7] | ❌[^7] |
| SortedMerge					  | ✔️ | ✔️ |
| SortedMergeBy					  | ✔️ | ✔️ |
| SortedMergeDescending			  | ✔️ | ✔️ |
| SortedMergeByDescending		  | ✔️ | ✔️ |
| Split							  | ✔️ | ✔️ |
| StartsWith					  | ✔️ | ✔️ |
| Subsets						  | ✔️ | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| TagFirstLast					  | ✔️ | ✔️ |
| Take							  | ✔️ | ✔️ |
| TakeEvery						  | ✔️ | ✔️ |
| TakeUntil						  | ✔️ | ✔️ |
| ThenBy						  | ✔️ | ✔️ |
| ToArrayByIndex				  | ✔️ | ✔️ |
| ToDataTable					  | ✔️ | ❌ |
| ToDelimitedString				  | ✔️ | ❌ |
| ToDictionary					  | ✔️ | ❌ |
| ToLookup						  | ✔️ | ❌ |
| Trace							  | ✔️ | ❌ |
| Transpose						  | ✔️ | ❌ |
| TraverseBreadthFirst			  | ✔️ | ✔️ |
| TraverseDepthFirst			  | ✔️ | ✔️ |
| TrySingle						  | ✔️ | ✔️ |
| Unfold						  | ✔️ | ❌ |
| Where							  | ✔️ | ✔️ |
| Window						  | ✔️ | ✔️ |
| WindowLeft					  | ✔️ | ✔️ |
| WindowRight					  | ✔️ | ✔️ |
| ZipLongest					  | ✔️ | ✔️ |
| ZipMap						  | ✔️ | ✔️ |
| ZipShortest					  | ✔️ | ✔️ |

[^1]: Internal operator no longer used
[^2]: Backsert has been obsoleted in favor of `.Insert(Index)`
[^8]: These methods have been renamed to `.MinItems()`/`.MaxItems()`; their names conflict with the new `.MinBy()`/`.MaxBy()` in .NET 6.
[^3]: Batch has been replaced by `.Buffer()` in .NET Core 3.1/.NET 5 (implemented by System.Interactive(.Async)) and `.Chunk()` in .NET 6; it is kept only for compatibility reasons.
[^4]: Pipe has been replaced by `.Do()` (implemented by System.Interactive(.Async))
[^5]: Rank has changed behavior, please review the breaking changes for Rank [here](Source/SuperLinq/readme.md#rank).
[^6]: Scan has been renamed to `ScanEx()`; Scan is taken by implementation in System.Interactive(.Async) which does not conform to standard Scan operator behavior
[^7]: Slice has been obsoleted in favor of `.Take(Range)`.
[^9]: FullJoin, LeftJoin, RightJoin, and OrderedMerge have been obsoleted in favor of `.InnerJoin()`/ `.LeftOuterJoin()`/ `.RightOuterJoin()`/ `.FullOuterJoin()`

