using SuperLinq.Collections;

namespace SuperLinq.Async;

public partial class AsyncSuperEnumerable
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	public static ValueTask<TCost?> GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		TState end,
		CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPathCost(
			start, getNeighbors, end,
			stateComparer: null,
			costComparer: null,
			cancellationToken);
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	///  This method will operate on an infinite map, however, 
	///  performance will depend on how many states are required to
	///  be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static async ValueTask<TCost?> GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
		TState end,
		IEqualityComparer<TState>? stateComparer,
		IComparer<TCost>? costComparer,
		CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(getNeighbors);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, TCost?>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, TCost>(16, costComparer, stateComparer);

		TState current = start;
		TCost? cost = default;
		do
		{
			cancellationToken.ThrowIfCancellationRequested();

			totalCost[current] = cost;
			if (stateComparer.Equals(current, end))
				break;

			var newStates = getNeighbors(current, cost);
			await foreach (var (s, p) in newStates.WithCancellation(cancellationToken).ConfigureAwait(false))
				if (!totalCost.TryGetValue(s, out _))
					queue.EnqueueMinimum(s, p);
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	public static ValueTask<IEnumerable<(TState nextState, TCost? cost)>>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPath(
			start,
			getNeighbors,
			end,
			stateComparer: null,
			costComparer: null,
			cancellationToken);
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
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
	///  This method will operate on an infinite map, however, 
	///  performance will depend on how many states are required to
	///  be evaluated before reaching the target point.
	/// </para>
	/// <para>
	///  This operator executes immediately.
	/// </para>
	/// </remarks>
	public static async ValueTask<IEnumerable<(TState state, TCost? cost)>>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			TState end,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(getNeighbors);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, (TState? parent, TCost? cost)>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, (TState? parent, TCost cost)>(
			16,
			priorityComparer: Comparer<(TState? parent, TCost cost)>.Create(
				(x, y) => costComparer.Compare(x.cost, y.cost)),
			stateComparer);

		TState current = start;
		(TState? parent, TCost cost) from = default;
		do
		{
			cancellationToken.ThrowIfCancellationRequested();

			totalCost[current] = from;
			if (stateComparer.Equals(current, end))
				break;

			var cost = from.cost;
			var newStates = getNeighbors(current, cost);
			await foreach (var (s, p) in newStates.WithCancellation(cancellationToken).ConfigureAwait(false))
				if (!totalCost.TryGetValue(s, out _))
					queue.EnqueueMinimum(s, (current, p));
		} while (queue.TryDequeue(out current!, out from));

		return SuperEnumerable.Generate(end, x => totalCost[x].parent!)
			.TakeUntil(x => stateComparer.Equals(x, start))
			.ZipMap(x => totalCost[x].cost)
			.Reverse();
	}

	#endregion

	#region Full Map Cost

	/// <summary>
	/// Find the shortest path from state <paramref name="start"/> 
	/// to every other <typeparamref name="TState"/> in the map,
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
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// A map that contains, for every <typeparamref name="TState"/>,
	/// the previous <typeparamref name="TState"/> in the shortest path
	/// from <paramref name="start"/> to this <typeparamref name="TState"/>,
	/// as well as the total cost to travel from <paramref name="start"/>
	/// to this <typeparamref name="TState"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to every other <typeparamref name="TState"/> in the map. 
	///  An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
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
	///  While <see cref="GetShortestPathCost{TState, TCost}(TState, Func{TState, TCost, IAsyncEnumerable{ValueTuple{TState, TCost}}}, TState, CancellationToken)"/>
	///  and <see cref="GetShortestPath{TState, TCost}(TState, Func{TState, TCost, IAsyncEnumerable{ValueTuple{TState, TCost}}}, TState, CancellationToken)"/>
	///  will work work on infinite maps, this method
	///  will execute an infinite loop on infinite maps. This is because
	///  this method will attemp to visit every point in the map.
	///  This method will terminate only when any points returned by
	///  <paramref name="getNeighbors"/> have all already been visited.
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
	public static ValueTask<IReadOnlyDictionary<TState, (TState? previousState, TCost? cost)>>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPaths(
			start,
			getNeighbors,
			stateComparer: null,
			costComparer: null,
			cancellationToken);
	}

	/// <summary>
	/// Find the shortest path from state <paramref name="start"/> 
	/// to every other <typeparamref name="TState"/> in the map,
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
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// A map that contains, for every <typeparamref name="TState"/>,
	/// the previous <typeparamref name="TState"/> in the shortest path
	/// from <paramref name="start"/> to this <typeparamref name="TState"/>,
	/// as well as the total cost to travel from <paramref name="start"/>
	/// to this <typeparamref name="TState"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses Dijkstra's algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to every other <typeparamref name="TState"/> in the map. 
	///  An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
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
	///  While <see cref="GetShortestPathCost{TState, TCost}(TState, Func{TState, TCost, IAsyncEnumerable{ValueTuple{TState, TCost}}}, TState, CancellationToken)"/>
	///  and <see cref="GetShortestPath{TState, TCost}(TState, Func{TState, TCost, IAsyncEnumerable{ValueTuple{TState, TCost}}}, TState, CancellationToken)"/>
	///  will work work on infinite maps, this method
	///  will execute an infinite loop on infinite maps. This is because
	///  this method will attemp to visit every point in the map.
	///  This method will terminate only when any points returned by
	///  <paramref name="getNeighbors"/> have all already been visited.
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
	public static async ValueTask<IReadOnlyDictionary<TState, (TState? previousState, TCost? cost)>>
		GetShortestPaths<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost)>> getNeighbors,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(getNeighbors);

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
			cancellationToken.ThrowIfCancellationRequested();

			totalCost[current] = from;

			var cost = from.cost;
			var newStates = getNeighbors(current, cost);
			await foreach (var (s, p) in newStates.WithCancellation(cancellationToken).ConfigureAwait(false))
				if (!totalCost.TryGetValue(s, out _))
					queue.EnqueueMinimum(s, (current, p));
		} while (queue.TryDequeue(out current!, out from));

		return totalCost;
	}

	#endregion

	#endregion

	#region A*

	#region Simple Cost

	/// <summary>
	/// Calculate the cost of the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using the A* algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state;
	/// the total cost to get to that state based on the 
	/// traversal cost at the current state; and the predicted
	/// or best-guess total (already traversed plus remaining)
	/// cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses the A* algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	///  Overall performance of this method will depend on the reliability
	///  of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  The A* algorithm assumes that all costs are positive,
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
	public static ValueTask<TCost?> GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
		TState end,
		CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPathCost(
			start,
			getNeighbors,
			end,
			stateComparer: null,
			costComparer: null,
			cancellationToken);
	}

	/// <summary>
	/// Calculate the cost of the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using the A* algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state;
	/// the total cost to get to that state based on the 
	/// traversal cost at the current state; and the predicted
	/// or best-guess total (already traversed plus remaining)
	/// cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The traversal cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses the A* algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	///  Overall performance of this method will depend on the reliability
	///  of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  The A* algorithm assumes that all costs are positive,
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
	public static async ValueTask<TCost?> GetShortestPathCost<TState, TCost>(
		TState start,
		Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
		TState end,
		IEqualityComparer<TState>? stateComparer,
		IComparer<TCost>? costComparer,
		CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(getNeighbors);

		stateComparer ??= EqualityComparer<TState>.Default;
		costComparer ??= Comparer<TCost>.Default;

		var totalCost = new Dictionary<TState, TCost?>(stateComparer);
		var queue = new UpdatablePriorityQueue<TState, (TCost bestGuess, TCost traversed)>(
			16,
			priorityComparer: Comparer<(TCost bestGuess, TCost traversed)>.Create(
				(x, y) =>
				{
					var cmp = costComparer.Compare(x.bestGuess, y.bestGuess);
					return cmp != 0 ? cmp : costComparer.Compare(x.traversed, y.traversed);
				}),
			stateComparer);

		TState current = start;
		(TCost bestGuess, TCost traversed) costs = default;
		do
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (totalCost.TryGetValue(current, out var oldCost)
				&& costComparer.Compare(costs.traversed, oldCost) >= 0)
			{
				continue;
			}

			totalCost[current] = costs.traversed;
			if (stateComparer.Equals(current, end))
				break;

			var newStates = getNeighbors(current, costs.traversed);
			await foreach (var (s, p, h) in newStates.WithCancellation(cancellationToken).ConfigureAwait(false))
				queue.EnqueueMinimum(s, (h, p));
		} while (queue.TryDequeue(out current!, out costs));

		return costs.traversed;
	}

	#endregion

	#region Single Shortest Path

	/// <summary>
	/// Find the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using the A* algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state;
	/// the total cost to get to that state based on the 
	/// traversal cost at the current state; and the predicted
	/// or best-guess total (already traversed plus remaining)
	/// cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The traversal path and cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses the A* algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	///  Overall performance of this method will depend on the reliability
	///  of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  The A* algorithm assumes that all costs are positive,
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
	public static ValueTask<IEnumerable<(TState nextState, TCost? cost)>>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost cost, TCost bestGuess)>> getNeighbors,
			TState end,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		return GetShortestPath(
			start,
			getNeighbors,
			end,
			stateComparer: null,
			costComparer: null,
			cancellationToken);
	}

	/// <summary>
	/// Find the shortest path from
	/// state <paramref name="start"/> to state <paramref name="end"/>,
	/// using the A* algorithm.
	/// </summary>
	/// <typeparam name="TState">The type of each state in the map</typeparam>
	/// <typeparam name="TCost">The type of the cost to traverse between states</typeparam>
	/// <param name="start">The starting state</param>
	/// <param name="getNeighbors">
	/// A function that returns the neighbors for a given state;
	/// the total cost to get to that state based on the 
	/// traversal cost at the current state; and the predicted
	/// or best-guess total (already traversed plus remaining)
	/// cost to get to <paramref name="end"/>.
	/// </param>
	/// <param name="end">The target state</param>
	/// <param name="stateComparer">A custom equality comparer for <typeparamref name="TState"/></param>
	/// <param name="costComparer">A custom comparer for <typeparamref name="TCost"/></param>
	/// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
	/// <returns>
	/// The traversal path and cost of the shortest path from <paramref name="start"/>
	/// to <paramref name="end"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"><paramref name="getNeighbors"/> is <see langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	///  This method uses the A* algorithm to explore a map
	///  and find the shortest path from <paramref name="start"/>
	///  to <paramref name="end"/>. An <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>
	///  is used to manage the list of <typeparamref name="TState"/>s
	///  to process, to reduce the computation cost of this operator.
	///  Overall performance of this method will depend on the reliability
	///  of the best-guess cost provided by <paramref name="getNeighbors"/>.
	/// </para>
	/// <para>
	///  Loops and cycles are automatically detected and handled
	///  correctly by this operator; only the cheapest path to
	///  a given <typeparamref name="TState"/> is used, and other
	///  paths (including loops) are discarded.
	/// </para>
	/// <para>
	///  The A* algorithm assumes that all costs are positive,
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
	public static async ValueTask<IEnumerable<(TState nextState, TCost? cost)>>
		GetShortestPath<TState, TCost>(
			TState start,
			Func<TState, TCost?, IAsyncEnumerable<(TState nextState, TCost traversed, TCost bestGuess)>> getNeighbors,
			TState end,
			IEqualityComparer<TState>? stateComparer,
			IComparer<TCost>? costComparer,
			CancellationToken cancellationToken = default)
		where TState : notnull
		where TCost : notnull
	{
		Guard.IsNotNull(getNeighbors);

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

		TState current = start;
		(TState? parent, TCost bestGuess, TCost traversed) from = default;
		do
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (totalCost.TryGetValue(current, out _))
				continue;

			totalCost[current] = (from.parent, from.traversed);
			if (stateComparer.Equals(current, end))
				break;

			var cost = from.traversed;
			var newStates = getNeighbors(current, cost);
			await foreach (var (s, p, h) in newStates.WithCancellation(cancellationToken).ConfigureAwait(false))
				queue.EnqueueMinimum(s, (current, h, p));
		} while (queue.TryDequeue(out current!, out from));

		return SuperEnumerable.Generate(end, x => totalCost[x].parent!)
			.TakeUntil(x => stateComparer.Equals(x, start))
			.ZipMap(x => totalCost[x].traversed)
			.Reverse();
	}

	#endregion

	#endregion
}
