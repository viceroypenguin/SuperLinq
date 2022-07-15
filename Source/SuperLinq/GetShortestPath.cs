// temporary until all methods are implemented
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using SuperLinq.Collections;

namespace SuperLinq;

public partial class SuperEnumerable
{
	#region Dijkstra

	#region Simple Cost

	/// <summary>
	/// Calculate the cost of the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state
	/// and the total cost to get to that state based on the 
	/// traversal cost at the current state.
	/// </param>
	/// <param name="end">The target state</param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive,
	///  that is to say, that it is not possible to go a negative
	///  distance from one state to the next. Violating this assumption
	///  will have undefined behavior.
	/// </para>
	/// <para>
	///	 This method uses <see cref="EqualityComparer{T}.Default"/>
	///	 to compare <typeparamref name="TState"/>s and 
	///	 <see cref="Comparer{T}.Default"/> to compare traversal
	///	 <typeparamref name="TCost"/>s.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static TCost? GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		TState end)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPathCost(
			start, getNeighbors, end,
			stateComparer: null,
			costComparer: null);
	}

	/// <summary>
	/// Calculate the cost of the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state
	/// and the total cost to get to that state based on the 
	/// traversal cost at the current state.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive,
	///  that is to say, that it is not possible to go a negative
	///  distance from one state to the next. Violating this assumption
	///  will have undefined behavior.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static TCost? GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		TState end,
		IEqualityComparer<TState>? stateComparer,
		IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		getNeighbors.ThrowIfNull();

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, TCost?>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, TCost>(16, costComparer, stateComparer);

		TState current = start;
		TCost? cost = default;
		do
		{
			if (totalCost.TryGetValue(current, out _))
				continue;

			totalCost[current] = cost;
			if (stateComparer.Equals(current, end))
				break;

			var newStates = getNeighbors(current, cost);
			queue.EnqueueRangeMinimum(newStates);
		} while (queue.TryDequeue(out current!, out cost));

		return cost;
	}

	#endregion

	#region Single Shortest Path

	/// <summary>
	/// Find the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state
	/// and the total cost to get to that state based on the 
	/// traversal cost at the current state.
	/// </param>
	/// <param name="end">The target state</param>
	/// <returns>
	/// The traversal path and cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive,
	///  that is to say, that it is not possible to go a negative
	///  distance from one state to the next. Violating this assumption
	///  will have undefined behavior.
	/// </para>
	/// <para>
	///	 This method uses <see cref="EqualityComparer{T}.Default"/>
	///	 to compare <typeparamref name="TState"/>s and 
	///	 <see cref="Comparer{T}.Default"/> to compare traversal
	///	 <typeparamref name="TCost"/>s.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TState nextState, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPath(
			start,
			getNeighbors,
			end,
			stateComparer: null,
			costComparer: null);
	}

	/// <summary>
	/// Find the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state
	/// and the total cost to get to that state based on the 
	/// traversal cost at the current state.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <returns>
	/// The traversal path and cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive,
	///  that is to say, that it is not possible to go a negative
	///  distance from one state to the next. Violating this assumption
	///  will have undefined behavior.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TState state, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		getNeighbors.ThrowIfNull();

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, (TState? parent, TCost? cost)>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, (TState? parent, TCost? cost)>(
			16,
			priorityComparer: Comparer<(TState? parent, TCost? cost)>.Create(
				(x, y) => costComparer.Compare(x.cost, y.cost)),
			stateComparer);

		TState current = start;
		(TState? parent, TCost? cost) from = default;
		do
		{
			if (totalCost.TryGetValue(current, out _))
				continue;

			totalCost[current] = from;
			if (stateComparer.Equals(current, end))
				break;

			var cost = from.cost;
			var newStates = getNeighbors(current, cost)
				.Select(s => (s.nextState, (current, s.cost)));
			queue.EnqueueRangeMinimum(newStates!);
		} while (queue.TryDequeue(out current!, out from));

		return Generate(end, x => totalCost[x].parent!)
			.TakeUntil(x => stateComparer.Equals(x, start))
			.Select(x => (x, totalCost[x].cost))
			.Reverse();
	}

	#endregion

	#region Full Map Cost

	public static IReadOnlyDictionary<TState, (TState previousState, TCost cost)>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	public static IReadOnlyDictionary<TState, (TState previousState, TCost cost)>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	#endregion

	#endregion

	#region A*

	#region Simple Cost

	public static TCost GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
		TState end)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	public static TCost GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
		TState end,
		IEqualityComparer<TState>? stateComparer,
		IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Single Shortest Path

	public static IEnumerable<(TState nextState, TCost cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
			TState end)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	public static IEnumerable<(TState nextState, TCost cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
			TState end,
			IEqualityComparer<TState> stateComparer,
			IComparer<TCost> costComparer)
		where TState : notnull
		where TCost : notnull
	{
		throw new NotImplementedException();
	}

	#endregion

	#endregion
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
