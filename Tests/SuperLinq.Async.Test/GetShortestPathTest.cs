namespace Test.Async;

public class GetShortestPathTest
{
	private static ILookup<string, (string to, int cost)> BuildStringIntMap(IEqualityComparer<string>? stateComparer)
	{
		var costs =
			new[]
			{
				(from: "start", to: "a", cost: 1),
				(from: "a", to: "b", cost: 2),
				(from: "b", to: "c", cost: 3),
				(from: "c", to: "d", cost: 4),
				(from: "d", to: "end", cost: 5),
				(from: "start", to: "A", cost: 10),
				(from: "A", to: "B", cost: 20),
				(from: "B", to: "C", cost: 30),
				(from: "C", to: "D", cost: 40),
				(from: "D", to: "end", cost: 50),
				(from: "start", to: "END", cost: 10),
				(from: "start", to: "END", cost: 1000),
			};
		var map = costs
			.Concat(costs.Select(x => (from: x.to, to: x.from, x.cost)))
			.Where(x =>
				x.to != "start"
				&& x.from != "end")
			.ToLookup(x => x.from, x => (x.to, x.cost), stateComparer);
		return map;
	}

	public class Dijkstra
	{
		public static IEnumerable<object?[]> GetStringIntCostData { get; } =
			new[]
			{
				new object?[] { null, null, 15, 7, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, null, 10, 4, },
				new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 150, 6, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 1000, 1, },
			};

		[Theory]
		[MemberData(nameof(GetStringIntCostData))]
		public async Task GetStringIntCost(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			int expectedCost,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;
			var actualCost = await AsyncSuperEnumerable.GetShortestPathCost(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.ToAsyncEnumerable();
				},
				"end",
				stateComparer,
				costComparer);

			Assert.Equal(expectedCost, actualCost);
			Assert.Equal(expectedCount, count);
		}

		public static IEnumerable<object?[]> GetStringIntPathData { get; } =
			new[]
			{
				new object?[] { null, null, Seq(("start", 0), ("a", 1), ("b", 3), ("c", 6), ("d", 10), ("end", 15)), 7, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, null, Seq(("start", 0), ("end", 10)), 4, },
				new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("A", 10), ("B", 30), ("C", 60), ("D", 100), ("end", 150)), 6, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("end", 1000)), 1, },
			};

		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public async Task GetStringIntPath(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;
			var path = await AsyncSuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.ToAsyncEnumerable();
				},
				"end",
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);
		}

		public static IEnumerable<object?[]> GetStringIntPathsData { get; } =
			new[]
			{
				new object?[] { null, null, Seq(
					("start", (null, 0)),
					("a", ("start", 1)),
					("b", ("a", 3)),
					("c", ("b", 6)),
					("d", ("c", 10)),
					("end", ("d", 15)),
					("A", ("start", 10)),
					("B", ("A", 30)),
					("C", ("B", 60)),
					("D", ("C", 100)),
					("END", ("start", 10))),
				},
				new object?[] { StringComparer.InvariantCultureIgnoreCase, null, Seq(
					("start", (null, 0)),
					("a", ("start", 1)),
					("b", ("a", 3)),
					("c", ("b", 6)),
					("d", ("c", 10)),
					("end", ("start", 10))),
				},
				new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(
					("start", (null, 0)),
					("a", ("start", 1)),
					("b", ("a", 3)),
					("c", ("b", 6)),
					("d", ("c", 10)),
					("A", ("start", 10)),
					("B", ("A", 30)),
					("C", ("B", 60)),
					("D", ("C", 100)),
					("end", ("D", 150)),
					("END", ("start", 1000))),
				},
				new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(
					("start", (null, 0)),
					("a", ("start", 10)),
					("b", ("a", 30)),
					("c", ("b", 60)),
					("d", ("c", 100)),
					("end", ("start", 1000))),
				},
			};

		[Theory]
		[MemberData(nameof(GetStringIntPathsData))]
		public async Task GetStringIntPaths(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, (string? prevState, int cost))> expectedMap)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;
			var paths = await AsyncSuperEnumerable.GetShortestPaths(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.ToAsyncEnumerable();
				},
				stateComparer,
				costComparer);

			stateComparer ??= StringComparer.Ordinal;

			Assert.Equal(expectedMap.Count(), count);
			Assert.Equal(expectedMap.Count(), paths.Count);
			foreach (var (key, ev) in expectedMap)
			{
				Assert.True(paths.TryGetValue(key, out var av));
				Assert.Equal(ev.prevState, av.previousState, stateComparer!);
				Assert.Equal(ev.cost, av.cost);
			}
		}

		[Fact]
		public async Task GetRegularMapCost()
		{
			var count = 0;
			async IAsyncEnumerable<((int x, int y) p, double cost)> GetNeighbors((int x, int y) p, double cost)
			{
				await Task.Yield();

				count++;
				yield return ((p.x + 1, p.y), cost + 1.001d);
				yield return ((p.x, p.y + 1), cost + 1.002d);
				yield return ((p.x - 1, p.y), cost + 1.003d);
				yield return ((p.x, p.y - 1), cost + 1.004d);
			}

			var actualCost = await AsyncSuperEnumerable.GetShortestPathCost<(int, int), double>(
				(0, 0),
				GetNeighbors,
				(2, 2));

			Assert.Equal(4.006d, actualCost, 3);
			Assert.Equal(27, count);
		}

		[Fact]
		public async Task GetRegularMapPath()
		{
			var count = 0;
			async IAsyncEnumerable<((int x, int y) p, double cost)> GetNeighbors((int x, int y) p, double cost)
			{
				await Task.Yield();

				count++;
				yield return ((p.x + 1, p.y), cost + 1.001d);
				yield return ((p.x, p.y + 1), cost + 1.002d);
				yield return ((p.x - 1, p.y), cost + 1.003d);
				yield return ((p.x, p.y - 1), cost + 1.004d);
			}

			var path = await AsyncSuperEnumerable.GetShortestPath<(int, int), double>(
				(0, 0),
				GetNeighbors,
				(2, 2));

			path.AssertSequenceEqual(
				(a, b) => (a.nextState == b.nextState && Math.Abs(a.cost - b.cost) < 0.001d),
				(nextState: (0, 0), cost: 0d),
				(nextState: (1, 0), cost: 1.001d),
				(nextState: (2, 0), cost: 2.002d),
				(nextState: (2, 1), cost: 3.004d),
				(nextState: (2, 2), cost: 4.006d));
			Assert.Equal(27, count);
		}

		[Fact]
		public async Task InvalidMapThrowsException()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await AsyncSuperEnumerable.GetShortestPathCost<int, int>(1, (a, b) => AsyncSeq<(int, int)>(), 2));
			await Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await AsyncSuperEnumerable.GetShortestPath<int, int>(1, (a, b) => AsyncSeq<(int, int)>(), 2));
		}
	}

	public class AStar
	{
		public static IEnumerable<object?[]> GetStringIntCostData { get; } =
			new[]
			{
				new object?[] { null, null, 15, 7, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, null, 10, 4, },
				new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 150, 6, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 1000, 1, },
			};

		// No heuristic means this operates the same as Dijkstra; this is
		// to prove the base algorithm still works.
		// Primary improvement of A* is the heuristic to reduce nodes visited.
		[Theory]
		[MemberData(nameof(GetStringIntCostData))]
		public async Task GetStringIntCost(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			int expectedCost,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;
			var actualCost = await AsyncSuperEnumerable.GetShortestPathCost(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost, c + y.cost))
						.ToAsyncEnumerable();
				},
				"end",
				stateComparer,
				costComparer);

			Assert.Equal(expectedCost, actualCost);
			Assert.Equal(expectedCount, count);
		}

		public static IEnumerable<object?[]> GetStringIntPathData { get; } =
			new[]
			{
				new object?[] { null, null, Seq(("start", 0), ("a", 1), ("b", 3), ("c", 6), ("d", 10), ("end", 15)), 7, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, null, Seq(("start", 0), ("end", 10)), 4, },
				new object?[] { null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("A", 10), ("B", 30), ("C", 60), ("D", 100), ("end", 150)), 6, },
				new object?[] { StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("end", 1000)), 1, },
			};

		// No heuristic means this operates the same as Dijkstra; this is
		// to prove the base algorithm still works.
		// Primary improvement of A* is the heuristic to reduce nodes visited.
		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public async Task GetStringIntPath(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;
			var path = await AsyncSuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost, c + y.cost))
						.ToAsyncEnumerable();
				},
				"end",
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);
		}

		[Fact]
		public async Task GetRegularMapCost()
		{
			var start = (x: 0, y: 0);
			var end = (x: 2, y: 2);
			((int x, int y) p, double cost, double bestGuess) GetNeighbor((int x, int y) p, double newCost)
			{
				var xD = p.x - end.x;
				var yD = p.y - end.y;
				var dist = Math.Sqrt(xD * xD + yD * yD);
				return (p, newCost, newCost + dist);
			}

			var count = 0;
			async IAsyncEnumerable<((int x, int y) p, double cost, double bestGuess)> GetNeighbors((int x, int y) p, double cost)
			{
				await Task.Yield();

				count++;
				yield return GetNeighbor((p.x + 1, p.y), cost + 1.001d);
				yield return GetNeighbor((p.x, p.y + 1), cost + 1.002d);
				yield return GetNeighbor((p.x - 1, p.y), cost + 1.003d);
				yield return GetNeighbor((p.x, p.y - 1), cost + 1.004d);
			}

			var actualCost = await AsyncSuperEnumerable.GetShortestPathCost<(int, int), double>(
				start,
				GetNeighbors,
				end);

			Assert.Equal(4.006d, actualCost, 3);
			Assert.Equal(8, count);
		}

		[Fact]
		public async Task GetRegularMapPath()
		{
			var start = (x: 0, y: 0);
			var end = (x: 2, y: 2);
			((int x, int y) p, double cost, double bestGuess) GetNeighbor((int x, int y) p, double newCost)
			{
				var xD = p.x - end.x;
				var yD = p.y - end.y;
				var dist = Math.Sqrt(xD * xD + yD * yD);
				return (p, newCost, newCost + dist);
			}

			var count = 0;
			async IAsyncEnumerable<((int x, int y) p, double cost, double bestGuess)> GetNeighbors((int x, int y) p, double cost)
			{
				await Task.Yield();

				count++;
				yield return GetNeighbor((p.x + 1, p.y), cost + 1.001d);
				yield return GetNeighbor((p.x, p.y + 1), cost + 1.002d);
				yield return GetNeighbor((p.x - 1, p.y), cost + 1.003d);
				yield return GetNeighbor((p.x, p.y - 1), cost + 1.004d);
			}

			var actualPath = await AsyncSuperEnumerable.GetShortestPath<(int, int), double>(
				start,
				GetNeighbors,
				end);

			actualPath.AssertSequenceEqual(
				(a, b) => a.nextState == b.nextState && Math.Abs(a.cost - b.cost) < 0.001d,
				(nextState: (0, 0), cost: 0),
				(nextState: (1, 0), cost: 1.001d),
				(nextState: (2, 0), cost: 2.002d),
				(nextState: (2, 1), cost: 3.004d),
				(nextState: (2, 2), cost: 4.006d));
			Assert.Equal(8, count);
		}

		[Fact]
		public async Task InvalidMapThrowsException()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await AsyncSuperEnumerable.GetShortestPathCost<int, int>(1, (a, b) => AsyncSeq<(int, int, int)>(), 2));
			await Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await AsyncSuperEnumerable.GetShortestPath<int, int>(1, (a, b) => AsyncSeq<(int, int, int)>(), 2));
		}
	}
}
