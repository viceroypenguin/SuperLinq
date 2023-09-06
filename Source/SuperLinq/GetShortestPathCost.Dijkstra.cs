using SuperLinq.Collections;

namespace SuperLinq;

public partial class SuperEnumerable
{
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
	/// <exception cref="InvalidOperationException">The map is entirely explored and no path to <paramref name="end"/> is found.</exception>
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
	///  This method will operate on an infinite map, however, 
	///  performance will depend on how many states are required to
	///  be evaluated before reaching the target point.
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
	/// <exception cref="InvalidOperationException">The map is entirely explored and no path to <paramref name="end"/> is found.</exception>
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
	///  This method will operate on an infinite map, however, 
	///  performance will depend on how many states are required to
	///  be evaluated before reaching the target point.
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
		Guard.IsNotNull(start);
		Guard.IsNotNull(getNeighbors);
		Guard.IsNotNull(end);

		stateComparer ??= EqualityComparer<TState>.Default;

		return GetShortestPathCost(
			start, getNeighbors, s => stateComparer.Equals(s, end),
			stateComparer, costComparer);
	}

	/// <summary>
	/// Calculate the cost of the shortest path from state <paramref name="start"/> to a state that satisfies the
	/// conditions expressed by <paramref name="predicate"/>, using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state and the total cost to get to that state based on the
	/// traversal cost at the current state.
	/// </param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/> to a state that satisfies the conditions
	/// expressed by <paramref name="predicate"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">The map is entirely explored and no state that satisfies the
	/// conditions expressed by <paramref name="predicate"/> is found.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///  to a state that satisfies the conditions expressed by <paramref name="predicate"/>. An <see
	///  cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///  name="TState"/>s to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path to a
	///  given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///  negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///  This method will operate on an infinite map, however, performance will depend on how many states are required
	///  to be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///	 This method uses <see cref="EqualityComparer{T}.Default"/> to compare <typeparamref name="TState"/>s and <see
	///	 cref="Comparer{T}.Default"/> to compare traversal
	///	 <typeparamref name="TCost"/>s.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static TCost? GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		Func<TState, bool> predicate)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPathCost(
			start, getNeighbors, predicate,
			stateComparer: null,
			costComparer: null);
	}

	/// <summary>
	/// Calculate the cost of the shortest path from state <paramref name="start"/> to a state that satisfies the
	/// conditions expressed by <paramref name="predicate"/>, using Dijkstra's algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state and the total cost to get to that state based on the
	/// traversal cost at the current state.
	/// </param>
	/// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/> to a state that satisfies the conditions
	/// expressed by <paramref name="predicate"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">The map is entirely explored and no state that satisfies the
	/// conditions expressed by <paramref name="predicate"/> is found.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///  to a state that satisfies the conditions expressed by <paramref name="predicate"/>. An <see
	///  cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///  name="TState"/>s to process, to reduce the computation cost of this operator.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path to a
	///  given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  Dijkstra's algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///  negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///  This method will operate on an infinite map, however, performance will depend on how many states are required
	///  to be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static TCost? GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		Func<TState, bool> predicate,
		IEqualityComparer<TState>? stateComparer,
		IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(start);
		Guard.IsNotNull(getNeighbors);
		Guard.IsNotNull(predicate);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, TCost?>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, TCost>(16, costComparer, stateComparer);

		var current = start;
		TCost? cost = default;
		do
		{
			totalCost[current] = cost;
			if (predicate(current))
				break;

			var newStates = getNeighbors(current, cost);
			Guard.IsNotNull(newStates, $"{nameof(getNeighbors)}()");

			foreach (var (s, p) in newStates)
			{
				if (!totalCost.TryGetValue(s, out _))
					queue.EnqueueMinimum(s, p);
			}

			if (!queue.TryDequeue(out current, out cost))
				ThrowHelper.ThrowInvalidOperationException("Unable to find path to 'end'.");
		} while (true);

		return cost;
	}
}
