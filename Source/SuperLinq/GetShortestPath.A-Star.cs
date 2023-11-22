using SuperLinq.Collections;

namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	///	    Find the shortest path from state <paramref name="start"/> to state <paramref name="end"/>, using the A*
	///     algorithm.
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
	///	    A function that returns the neighbors for a given state; the total cost to get to that state based on the
	///     traversal cost at the current state; and the predicted or best-guess total (already traversed plus
	///     remaining) cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">
	///	    The target state
	/// </param>
	/// <returns>
	///	    The traversal path and cost of the shortest path from <paramref name="start"/> to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	///	    The map is entirely explored and no path to <paramref name="end"/> is found.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses the A* algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///     to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage
	///     the list of <typeparamref name="TState"/>s to process, to reduce the computation cost of this operator.
	///     Overall performance of this method will depend on the reliability of the best-guess cost provided by
	///     <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    The A* algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This method will operate on an infinite map, however, performance will depend on how many states are
	///     required to be evaluated before reaching the target point.
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
	public static IEnumerable<(TState nextState, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
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
	///	    Find the shortest path from state <paramref name="start"/> to state <paramref name="end"/>, using the A*
	///     algorithm.
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
	///	    A function that returns the neighbors for a given state; the total cost to get to that state based on the
	///     traversal cost at the current state; and the predicted or best-guess total (already traversed plus
	///     remaining) cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">
	///	    The target state
	/// </param>
	/// <param name="stateComparer">
	///	    A custom equality comparer for <typeparamref name="TState"/>
	/// </param>
	/// <param name="costComparer">
	///	    A custom comparer for <typeparamref name="TCost"/>
	/// </param>
	/// <returns>
	///	    The traversal path and cost of the shortest path from <paramref name="start"/> to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	///	    The map is entirely explored and no path to <paramref name="end"/> is found.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses the A* algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///     to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage
	///     the list of <typeparamref name="TState"/>s to process, to reduce the computation cost of this operator.
	///     Overall performance of this method will depend on the reliability of the best-guess cost provided by
	///     <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    The A* algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This method will operate on an infinite map, however, performance will depend on how many states are
	///     required to be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TState nextState, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost traversed, TCost bestGuess)>> getNeighbors,
			TState end,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		ArgumentNullException.ThrowIfNull(start);
		ArgumentNullException.ThrowIfNull(getNeighbors);
		ArgumentNullException.ThrowIfNull(end);

		stateComparer ??= EqualityComparer<TState>.Default;

		return GetShortestPath(
			start, getNeighbors, s => stateComparer.Equals(s, end),
			stateComparer, costComparer);
	}

	/// <summary>
	///	    Find the shortest path from state <paramref name="start"/> to a state that satisfies the conditions
	///     expressed by <paramref name="predicate"/>, using the A* algorithm.
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
	///	    A function that returns the neighbors for a given state; the total cost to get to that state based on the
	///     traversal cost at the current state; and the predicted or best-guess total (already traversed plus
	///     remaining) cost to get to a state that satisfies the conditions expressed by <paramref name="predicate"/>.
	/// </param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <returns>
	///	    The traversal path and cost of the shortest path from <paramref name="start"/> to a state that satisfies the
	///     conditions expressed by <paramref name="predicate"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	///	    The map is entirely explored and no state that satisfies the conditions expressed by <paramref
	///     name="predicate"/> is found.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses the A* algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///     to a state that satisfies the conditions expressed by <paramref name="predicate"/>. An <see
	///     cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///     name="TState"/>s to process, to reduce the computation cost of this operator. Overall performance of this
	///     method will depend on the reliability of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    The A* algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This method will operate on an infinite map, however, performance will depend on how many states are
	///     required to be evaluated before reaching the target point.
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
	public static IEnumerable<(TState nextState, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
			Func<TState, bool> predicate)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPath(
			start,
			getNeighbors,
			predicate,
			stateComparer: null,
			costComparer: null);
	}

	/// <summary>
	///	    Find the shortest path from state <paramref name="start"/> to a state that satisfies the conditions
	///     expressed by <paramref name="predicate"/>, using the A* algorithm.
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
	///	    A function that returns the neighbors for a given state; the total cost to get to that state based on the
	///     traversal cost at the current state; and the predicted or best-guess total (already traversed plus
	///     remaining) cost to get to a state that satisfies the conditions expressed by <paramref name="predicate"/>.
	/// </param>
	/// <param name="predicate">
	///	    The predicate that defines the conditions of the element to search for.
	/// </param>
	/// <param name="stateComparer">
	///	    A custom equality comparer for <typeparamref name="TState"/>
	/// </param>
	/// <param name="costComparer">
	///	    A custom comparer for <typeparamref name="TCost"/>
	/// </param>
	/// <returns>
	///	    The traversal path and cost of the shortest path from <paramref name="start"/> to a state that satisfies the
	///     conditions expressed by <paramref name="predicate"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">
	///	    <paramref name="getNeighbors"/> is <see langword="null"/>.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	///	    The map is entirely explored and no state that satisfies the conditions expressed by <paramref
	///     name="predicate"/> is found.
	/// </exception>
	/// <remarks>
	/// <para>
	///	    This method uses the A* algorithm to explore a map and find the shortest path from <paramref name="start"/>
	///     to a state that satisfies the conditions expressed by <paramref name="predicate"/>. An <see
	///     cref="UpdatablePriorityQueue{TElement, TPriority}"/> is used to manage the list of <typeparamref
	///     name="TState"/>s to process, to reduce the computation cost of this operator. Overall performance of this
	///     method will depend on the reliability of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///	    Loops and cycles are automatically detected and handled correctly by this operator; only the cheapest path
	///     to a given <typeparamref name="TState"/> is used, and other paths (including loops) are discarded.
	/// </para>
	/// <para>
	///	    The A* algorithm assumes that all costs are positive, that is to say, that it is not possible to go a
	///     negative distance from one state to the next. Violating this assumption will have undefined behavior.
	/// </para>
	/// <para>
	///	    This method will operate on an infinite map, however, performance will depend on how many states are
	///     required to be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///	    This operator executes immediately.
	/// </para>
	/// </remarks>
	public static IEnumerable<(TState nextState, TCost? cost)>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IEnumerable<(TState nextState, TCost traversed, TCost bestGuess)>> getNeighbors,
			Func<TState, bool> predicate,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer)
		where TState : notnull
		where TCost : notnull
	{
		ArgumentNullException.ThrowIfNull(start);
		ArgumentNullException.ThrowIfNull(getNeighbors);
		ArgumentNullException.ThrowIfNull(predicate);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, (TState? parent, TCost? traversed)>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, (TState? parent, TCost bestGuess, TCost traversed)>(
			16,
			priorityComparer: Comparer<(TState? parent, TCost bestGuess, TCost traversed)>.Create(
				(x, y) =>
				{
					var cmp = costComparer.Compare(x.bestGuess, y.bestGuess);
					return cmp != 0 ? cmp : costComparer.Compare(x.traversed, y.traversed);
				}),
			stateComparer);

		var current = start;
		TState? end = default;
		(TState? parent, TCost bestGuess, TCost traversed) from = default;
		do
		{
			if (!totalCost.TryGetValue(current, out var oldCost)
				|| costComparer.Compare(from.traversed, oldCost.traversed) < 0)
			{
				totalCost[current] = (from.parent, from.traversed);
				if (predicate(current))
				{
					end = current;
					break;
				}

				var cost = from.traversed;
				var newStates = getNeighbors(current, cost);
				ArgumentNullException.ThrowIfNull(newStates, $"{nameof(getNeighbors)}()");

				foreach (var (s, p, h) in newStates)
					queue.EnqueueMinimum(s, (current, h, p));
			}

			if (!queue.TryDequeue(out current, out from))
				ThrowHelper.ThrowInvalidOperationException("Unable to find path to 'end'.");
		} while (true);

		return Generate(end, x => totalCost[x].parent!)
			.TakeUntil(x => stateComparer.Equals(x, start))
			.ZipMap(x => totalCost[x].traversed)
			.Reverse()
			.ToList();
	}
}
