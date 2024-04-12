using SuperLinq.Collections;

namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Find the shortest path from state <paramref name="start"/> to every other <typeparamref name="TState"/> in
	///     the map, using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">
	///	    The type of each state in the map
	/// </typeparam>
	/// <typeparam name="TCost">
	///	    The type of the cost to traverse between states
	/// </typeparam>
	/// <param name="start">
	///	    The starting state
	/// </param>
	/// <param name="getNeighbors">
	///	    A function that returns the neighbors for a given state and the total cost to get to that state based on the
	///     traversal cost at the current state.
	/// </param>
	/// <returns>
	///	    A map that contains, for every <typeparamref name="TState"/>, the previous <typeparamref name="TState"/> in
	///     the shortest path from <paramref name="start"/> to this <typeparamref name="TState"/>, as well as the total
	///     cost to travel from <paramref name="start"/> to this <typeparamref name="TState"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses Dijkstra's algorithm to explore a map and find the shortest path from <paramref
	///     name="start"/> to every other <typeparamref name="TState"/> in the map. An <see
	///     cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///     name="TState"/>s to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    While <see cref="GetShortestPathCost{TState, TCost}(TState, Func{TState, TCost,
	///     IEnumerable{ValueTuple{TState, TCost}}}, TState)"/> and <see cref="GetShortestPath{TState, TCost}(TState,
	///     Func{TState, TCost, IEnumerable{ValueTuple{TState, TCost}}}, TState)"/> will work work on infinite maps,
	///     this method will execute an infinite loop on infinite maps. This is because this method will attempt to
	///     visit every point in the map. This method will terminate only when any points returned by <paramref
	///     name="getNeighbors"/> have all already been visited.
	/// </para>
	/// <para>
	///	    Dijkstra's algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This method uses <see cref="EqualityComparer{T}.Default"/> to compare <typeparamref name="TState"/>s and
	///	    <see cref="Comparer{T}.Default"/> to compare traversal
	///	 <typeparamref name="TCost"/>s.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IReadOnlyDictionary<TState, (TState? previousState, TCost? cost)>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPaths(
			start,
			getNeighbors,
			stateComparer: null,
			costComparer: null);
	}

	/// <summary>
	///	    Find the shortest path from state <paramref name="start"/> to every other <typeparamref name="TState"/> in
	///     the map, using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">
	///	    The type of each state in the map
	/// </typeparam>
	/// <typeparam name="TCost">
	///	    The type of the cost to traverse between states
	/// </typeparam>
	/// <param name="start">
	///	    The starting state
	/// </param>
	/// <param name="getNeighbors">
	///	    A function that returns the neighbors for a given state and the total cost to get to that state based on the
	///     traversal cost at the current state.
	/// </param>
	/// <param name="stateComparer">
	///	    A custom equality comparer for <typeparamref name="TState"/>
	/// </param>
	/// <param name="costComparer">
	///	    A custom comparer for <typeparamref name="TCost"/>
	///	</param>
	/// <returns>
	///	    A map that contains, for every <typeparamref name="TState"/>, the previous <typeparamref name="TState"/> in
	///     the shortest path from <paramref name="start"/> to this <typeparamref name="TState"/>, as well as the total
	///     cost to travel from <paramref name="start"/> to this <typeparamref name="TState"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses Dijkstra's algorithm to explore a map and find the shortest path from <paramref
	///     name="start"/> to every other <typeparamref name="TState"/> in the map. An <see
	///     cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///     name="TState"/>s to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    While <see cref="GetShortestPathCost{TState, TCost}(TState, Func{TState, TCost,
	///     IEnumerable{ValueTuple{TState, TCost}}}, TState)"/> and <see cref="GetShortestPath{TState, TCost}(TState,
	///     Func{TState, TCost, IEnumerable{ValueTuple{TState, TCost}}}, TState)"/> will work work on infinite maps,
	///     this method will execute an infinite loop on infinite maps. This is because this method will attempt to
	///     visit every point in the map. This method will terminate only when any points returned by <paramref
	///     name="getNeighbors"/> have all already been visited.
	/// </para>
	/// <para>
	///	    Dijkstra's algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IReadOnlyDictionary<TState, (TState? previousState, TCost? cost)>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		ArgumentNullException.ThrowIfNull(start);
		ArgumentNullException.ThrowIfNull(getNeighbors);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, (TState? parent, TCost? cost)>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, (TState? parent, TCost? cost)>(
			16,
			priorityComparer: Comparer<(TState? parent, TCost? cost)>.Create(
				(x, y) => costComparer.Compare(x.cost, y.cost)),
			stateComparer);

		var current = start;
		(TState? parent, TCost? cost) from = default;
		do
		{
			totalCost[current] = from;

			var cost = from.cost;
			var newStates = getNeighbors(current, cost);
			ArgumentNullException.ThrowIfNull(newStates, $"{nameof(getNeighbors)}()");

			foreach (var (s, p) in newStates)
			{
				if (!totalCost.TryGetValue(s, out _))
					queue.EnqueueMinimum(s, (current, p));
			}
		}
		while (queue.TryDequeue(out current, out from));

		return totalCost;
	}
}
