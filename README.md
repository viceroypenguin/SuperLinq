# SuperLinq

[![Build status](https://github.com/viceroypenguin/SuperLinq/actions/workflows/build.yml/badge.svg)](https://github.com/viceroypenguin/SuperLinq/actions)
[![License](https://img.shields.io/github/license/viceroypenguin/Superlinq)](license.txt)

LINQ to Objects is missing a few desirable features.

This project enhances LINQ to Objects with extra methods, in a manner which
keeps to the spirit of LINQ.

Methods are provided to extend both `IEnumerable<T>` (via SuperLinq package)
and `IAsyncEnumerable<T>` (via SuperLinq.Async package). 

## SuperLinq

SuperLinq is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq/).
[![NuGet](https://img.shields.io/nuget/v/SuperLinq.svg?style=plastic)](https://www.nuget.org/packages/SuperLinq/)

The documentation for the SuperLinq methods can be found [here](Source/SuperLinq/readme.md).

## SuperLinq.Async

SuperLinq.Async is available for download and installation as a
[NuGet package](https://www.nuget.org/packages/superlinq.async/). 
[![NuGet](https://img.shields.io/nuget/v/SuperLinq.Async.svg?style=plastic)](https://www.nuget.org/packages/SuperLinq.Async/)

The documentation for the SuperLinq.Async methods can be found [here](Source/SuperLinq.Async/readme.md).

## Operator Support

| Operator | SuperLinq | SuperLinq.Async |
|-|:-:|:-:|
| Acquire                         | ❌<br/>(Removed[^1]) | ❌ |
| AggregateRight				  | ✔️ | ✔️ |
| AtLeast						  | ✔️ | ✔️ |
| AtMost						  | ✔️ | ✔️ |
| Backsert						  | ✔️[^2] | ❌[^2] |
| Batch							  | ❌<br/>(Removed[^3]) | ❌[^3] |
| Cartesian						  | ✔️ | ❌ |
| Choose						  | ✔️ | ✔️ |
| CountBetween					  | ✔️ | ✔️ |
| CompareCount					  | ✔️ | ✔️ |
| CountBy						  | ✔️ | ✔️ |
| CountDown						  | ✔️ | ✔️ |
| Consume						  | ✔️ | ✔️ |
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
| Flatten						  | ✔️ | ❌ |
| Fold							  | ✔️ | ❌ |
| From							  | ✔️ | ✔️ |
| FullGroupJoin					  | ✔️ | ⏱([#19](https://github.com/viceroypenguin/SuperLinq/issues/19)) |
| FullJoin						  | ✔️ | ⏱([#19](https://github.com/viceroypenguin/SuperLinq/issues/19)) |
| Generate						  | ✔️ | ✔️ |
| GenerateByIndex				  | ✔️ | ✔️ |
| GroupAdjacent					  | ✔️ | ✔️ |
| Index							  | ✔️ | ✔️ |
| IndexBy						  | ✔️ | ✔️ |
| Insert						  | ✔️ | ✔️ |
| Interleave					  | ✔️ | ✔️ |
| Lag							  | ✔️ | ✔️ |
| Lead							  | ✔️ | ✔️ |
| LeftJoin						  | ✔️ | ⏱([#19](https://github.com/viceroypenguin/SuperLinq/issues/19)) |
| MaxBy							  | ❌<br/>(Removed[^8]) | ❌[^8] |
| MinBy							  | ❌<br/>(Removed[^8]) | ❌[^8] |
| Move							  | ✔️ | ⏱([#27](https://github.com/viceroypenguin/SuperLinq/issues/27)) |
| OrderBy						  | ✔️ | ✔️ |
| OrderedMerge					  | ✔️ | ⏱([#19](https://github.com/viceroypenguin/SuperLinq/issues/19)) |
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
| Return						  | ✔️ | ❌ |
| RightJoin						  | ✔️ | ❌ |
| RunLengthEncode				  | ✔️ | ✔️ |
| Scan							  | ❌<br/>(Removed[^6]) | ❌[^6] |
| ScanBy						  | ✔️ | ✔️ |
| ScanRight						  | ✔️ | ✔️ |
| Segment						  | ✔️ | ✔️ |
| Sequence						  | ✔️ | ✔️ |
| Shuffle						  | ✔️ | ⏱([#20](https://github.com/viceroypenguin/SuperLinq/issues/20)) |
| SkipUntil						  | ✔️ | ✔️ |
| Slice							  | ✔️[^7] | ❌[^7] |
| SortedMerge					  | ✔️ | ✔️ |
| SortedMergeDescending			  | ✔️ | ✔️ |
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
| TraverseBreadthFirst			  | ✔️ | ⏱([#21](https://github.com/viceroypenguin/SuperLinq/issues/21)) |
| TraverseDepthFirst			  | ✔️ | ⏱([#21](https://github.com/viceroypenguin/SuperLinq/issues/21)) |
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
[^8]: These methods have been removed; their names conflict with the new `.MinBy()`/`.MaxBy()` in .NET 6, and their behavior is better implemented by `.PartialSort()`
[^3]: Batch has been replaced by `.Buffer()` in .NET Core 3.1/.NET 5 (implemented by System.Interactive(.Async)) and `.Chunk()` in .NET 6
[^4]: Pipe has been replaced by `.Do()` (implemented by System.Interactive(.Async))
[^5]: Rank has changed behavior, please review the breaking changes for Rank [here](Source/SuperLinq/readme.md#rank).
[^6]: Scan has been replaced by implementation in System.Interactive(.Async)
[^7]: Slice has been obsoleted in favor of `.Take(Range)`.
