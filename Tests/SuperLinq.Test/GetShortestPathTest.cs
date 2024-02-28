namespace Test;

public static class GetShortestPathTest
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

	internal static IEnumerable<T> AddTestingSequenceToList<T>(this IEnumerable<T> source, List<TestingSequence<T>> list)
	{
		var seq = source.AsTestingSequence();
		list.Add(seq);
		return seq;
	}

	internal static void VerifySequences<T>(this List<TestingSequence<T>> list)
	{
		foreach (IDisposable s in list)
			s.Dispose();
	}

	public class Dijkstra
	{
		public static IEnumerable<object?[]> GetStringIntCostData { get; } =
			[
				[null, null, 15, 7,],
				[StringComparer.InvariantCultureIgnoreCase, null, 10, 4,],
				[null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 150, 6,],
				[StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 1000, 1,],
			];

		[Theory]
		[MemberData(nameof(GetStringIntCostData))]
		public void GetStringIntCostByState(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			int expectedCost,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int)>>();
			var actualCost = SuperEnumerable.GetShortestPathCost(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				"end",
				stateComparer,
				costComparer);

			Assert.Equal(expectedCost, actualCost);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		[Theory]
		[MemberData(nameof(GetStringIntCostData))]
		public void GetStringIntCostByFunction(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			int expectedCost,
			int expectedCount)
		{
			stateComparer ??= EqualityComparer<string>.Default;

			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int)>>();
			var actualCost = SuperEnumerable.GetShortestPathCost(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				s => stateComparer.Equals(s[..1], "e"),
				stateComparer,
				costComparer);

			Assert.Equal(expectedCost, actualCost);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		public static IEnumerable<object?[]> GetStringIntPathData { get; } =
			[
				[null, null, Seq(("start", 0), ("a", 1), ("b", 3), ("c", 6), ("d", 10), ("end", 15)), 7,],
				[StringComparer.InvariantCultureIgnoreCase, null, Seq(("start", 0), ("END", 10)), 4,],
				[null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("A", 10), ("B", 30), ("C", 60), ("D", 100), ("end", 150)), 6,],
				[StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("END", 1000)), 1,],
			];

		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public void GetStringIntPathByState(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int)>>();
			var path = SuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				"end",
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public void GetStringIntPathByFunction(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			stateComparer ??= EqualityComparer<string>.Default;

			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int)>>();
			var path = SuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.AsTestingSequence();
				},
				s => stateComparer.Equals(s[..1], "e"),
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		public static IEnumerable<object?[]> GetStringIntPathsData { get; } =
			[
				[
					null,
					null,
					Seq<(string, (string?, int))>(
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
				],
				[
					StringComparer.InvariantCultureIgnoreCase,
					null,
					Seq<(string, (string?, int))>(
						("start", (null, 0)),
						("a", ("start", 1)),
						("b", ("a", 3)),
						("c", ("b", 6)),
						("d", ("c", 10)),
						("end", ("start", 10))),
				],
				[
					null,
					Comparer<int>.Create((x, y) => -x.CompareTo(y)),
					Seq<(string, (string?, int))>(
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
				],
				[
					StringComparer.InvariantCultureIgnoreCase,
					Comparer<int>.Create((x, y) => -x.CompareTo(y)),
					Seq<(string, (string?, int))>(
						("start", (null, 0)),
						("a", ("start", 10)),
						("b", ("a", 30)),
						("c", ("b", 60)),
						("d", ("c", 100)),
						("end", ("start", 1000))),
				],
			];

		[Theory]
		[MemberData(nameof(GetStringIntPathsData))]
		public void GetStringIntPaths(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, (string? prevState, int cost))> expectedMap)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int)>>();
			var paths = SuperEnumerable.GetShortestPaths(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost))
						.AddTestingSequenceToList(sequences);
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

			sequences.VerifySequences();
		}

		[Fact]
		public void GetRegularMapCost()
		{
			var count = 0;
			IEnumerable<((int x, int y) p, double cost)> GetNeighbors((int x, int y) p, double cost)
			{
				count++;
				yield return ((p.x + 1, p.y), cost + 1.001d);
				yield return ((p.x, p.y + 1), cost + 1.002d);
				yield return ((p.x - 1, p.y), cost + 1.003d);
				yield return ((p.x, p.y - 1), cost + 1.004d);
			}

			var sequences = new List<TestingSequence<((int, int), double)>>();
			var actualCost = SuperEnumerable.GetShortestPathCost<(int, int), double>(
				(0, 0),
				(x, c) => GetNeighbors(x, c)
					.AddTestingSequenceToList(sequences),
				(2, 2));

			Assert.Equal(4.006d, actualCost, 3);
			Assert.Equal(27, count);

			sequences.VerifySequences();
		}

		[Fact]
		public void GetRegularMapPath()
		{
			var count = 0;
			IEnumerable<((int x, int y) p, double cost)> GetNeighbors((int x, int y) p, double cost)
			{
				count++;
				yield return ((p.x + 1, p.y), cost + 1.001d);
				yield return ((p.x, p.y + 1), cost + 1.002d);
				yield return ((p.x - 1, p.y), cost + 1.003d);
				yield return ((p.x, p.y - 1), cost + 1.004d);
			}

			var sequences = new List<TestingSequence<(string, int)>>();
			var path = SuperEnumerable.GetShortestPath<(int, int), double>(
				(0, 0),
				GetNeighbors,
				(2, 2));

			path.AssertSequenceEqual(
				(a, b) => a.nextState == b.nextState && Math.Abs(a.cost - b.cost) < 0.001d,
				(nextState: (0, 0), cost: 0d),
				(nextState: (1, 0), cost: 1.001d),
				(nextState: (2, 0), cost: 2.002d),
				(nextState: (2, 1), cost: 3.004d),
				(nextState: (2, 2), cost: 4.006d));
			Assert.Equal(27, count);

			sequences.VerifySequences();
		}

		[Fact]
		public void InvalidMapThrowsException()
		{
			_ = Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.GetShortestPathCost<int, int>(1, (a, b) => Array.Empty<(int, int)>(), 2));
			_ = Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.GetShortestPath<int, int>(1, (a, b) => Array.Empty<(int, int)>(), 2));
		}
	}

	public class AStar
	{
		public static IEnumerable<object?[]> GetStringIntCostData { get; } =
			[
				[null, null, 15, 7,],
				[StringComparer.InvariantCultureIgnoreCase, null, 10, 4,],
				[null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 150, 6,],
				[StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), 1000, 1,],
			];

		// No heuristic means this operates the same as Dijkstra; this is
		// to prove the base algorithm still works.
		// Primary improvement of A* is the heuristic to reduce nodes visited.
		[Theory]
		[MemberData(nameof(GetStringIntCostData))]
		public void GetStringIntCost(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			int expectedCost,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int, int)>>();
			var actualCost = SuperEnumerable.GetShortestPathCost(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				"end",
				stateComparer,
				costComparer);

			Assert.Equal(expectedCost, actualCost);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		public static IEnumerable<object?[]> GetStringIntPathData { get; } =
			[
				[null, null, Seq(("start", 0), ("a", 1), ("b", 3), ("c", 6), ("d", 10), ("end", 15)), 7,],
				[StringComparer.InvariantCultureIgnoreCase, null, Seq(("start", 0), ("END", 10)), 4,],
				[null, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("A", 10), ("B", 30), ("C", 60), ("D", 100), ("end", 150)), 6,],
				[StringComparer.InvariantCultureIgnoreCase, Comparer<int>.Create((x, y) => -x.CompareTo(y)), Seq(("start", 0), ("END", 1000)), 1,],
			];

		// No heuristic means this operates the same as Dijkstra; this is
		// to prove the base algorithm still works.
		// Primary improvement of A* is the heuristic to reduce nodes visited.
		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public void GetStringIntPathByState(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int, int)>>();
			var path = SuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				"end",
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		[Theory]
		[MemberData(nameof(GetStringIntPathData))]
		public void GetStringIntPathByFunction(
			IEqualityComparer<string>? stateComparer,
			IComparer<int>? costComparer,
			IEnumerable<(string state, int cost)> expectedPath,
			int expectedCount)
		{
			stateComparer ??= EqualityComparer<string>.Default;

			var map = BuildStringIntMap(stateComparer);
			var count = 0;

			var sequences = new List<TestingSequence<(string, int, int)>>();
			var path = SuperEnumerable.GetShortestPath(
				"start",
				(x, c) =>
				{
					count++;
					return map[x].Select(y => (y.to, c + y.cost, c + y.cost))
						.AddTestingSequenceToList(sequences);
				},
				s => stateComparer.Equals(s[..1], "e"),
				stateComparer,
				costComparer);

			path.AssertSequenceEqual(expectedPath);
			Assert.Equal(expectedCount, count);

			sequences.VerifySequences();
		}

		[Fact]
		public void GetRegularMapCost()
		{
			var start = (x: 0, y: 0);
			var end = (x: 2, y: 2);
			((int x, int y) p, double cost, double bestGuess) GetNeighbor((int x, int y) p, double newCost)
			{
				var xD = p.x - end.x;
				var yD = p.y - end.y;
				var dist = Math.Sqrt((xD * xD) + (yD * yD));
				return (p, newCost, newCost + dist);
			}

			var count = 0;
			IEnumerable<((int x, int y) p, double cost, double bestGuess)> GetNeighbors((int x, int y) p, double cost)
			{
				count++;
				yield return GetNeighbor((p.x + 1, p.y), cost + 1.001d);
				yield return GetNeighbor((p.x, p.y + 1), cost + 1.002d);
				yield return GetNeighbor((p.x - 1, p.y), cost + 1.003d);
				yield return GetNeighbor((p.x, p.y - 1), cost + 1.004d);
			}

			var sequences = new List<TestingSequence<((int x, int y) p, double cost, double bestGuess)>>();
			var actualCost = SuperEnumerable.GetShortestPathCost<(int, int), double>(
				start,
				(x, c) => GetNeighbors(x, c)
					.AddTestingSequenceToList(sequences),
				end);

			Assert.Equal(4.006d, actualCost, 3);
			Assert.Equal(8, count);

			sequences.VerifySequences();
		}

		[Fact]
		public void GetRegularMapPath()
		{
			var start = (x: 0, y: 0);
			var end = (x: 2, y: 2);
			((int x, int y) p, double cost, double bestGuess) GetNeighbor((int x, int y) p, double newCost)
			{
				var xD = p.x - end.x;
				var yD = p.y - end.y;
				var dist = Math.Sqrt((xD * xD) + (yD * yD));
				return (p, newCost, newCost + dist);
			}

			var count = 0;
			IEnumerable<((int x, int y) p, double cost, double bestGuess)> GetNeighbors((int x, int y) p, double cost)
			{
				count++;
				yield return GetNeighbor((p.x + 1, p.y), cost + 1.001d);
				yield return GetNeighbor((p.x, p.y + 1), cost + 1.002d);
				yield return GetNeighbor((p.x - 1, p.y), cost + 1.003d);
				yield return GetNeighbor((p.x, p.y - 1), cost + 1.004d);
			}

			var sequences = new List<TestingSequence<((int, int), double, double)>>();
			var actualPath = SuperEnumerable.GetShortestPath<(int, int), double>(
				start,
				(x, c) => GetNeighbors(x, c)
					.AddTestingSequenceToList(sequences),
				end);

			actualPath.AssertSequenceEqual(
				(a, b) => a.nextState == b.nextState && Math.Abs(a.cost - b.cost) < 0.001d,
				(nextState: (0, 0), cost: 0),
				(nextState: (1, 0), cost: 1.001d),
				(nextState: (2, 0), cost: 2.002d),
				(nextState: (2, 1), cost: 3.004d),
				(nextState: (2, 2), cost: 4.006d));
			Assert.Equal(8, count);

			sequences.VerifySequences();
		}

		[Fact]
		public void InvalidMapThrowsException()
		{
			_ = Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.GetShortestPathCost<int, int>(1, (a, b) => Array.Empty<(int, int, int)>(), 2));
			_ = Assert.Throws<InvalidOperationException>(() =>
				SuperEnumerable.GetShortestPath<int, int>(1, (a, b) => Array.Empty<(int, int, int)>(), 2));
		}
	}
}
