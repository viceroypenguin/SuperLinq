// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// copied from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs
// and further edited

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Collections;

/// <summary>
///  Represents a min priority queue.
/// </summary>
/// <typeparam name="TElement">Specifies the type of elements in the queue.</typeparam>
/// <typeparam name="TPriority">Specifies the type of priority associated with enqueued elements.</typeparam>
/// <remarks>
///  Implements an array-backed quaternary min-heap. Each element is enqueued with an associated priority
///  that determines the dequeue order: elements with the lowest priority get dequeued first.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(PriorityQueueDebugView<,>))]
public class UpdatablePriorityQueue<TElement, TPriority>
{
	/// <summary>
	/// Represents an implicit heap-ordered complete d-ary tree, stored as an array.
	/// </summary>
	private (TElement Element, TPriority Priority)[] _nodes;

	/// <summary>
	/// Identifies the location of an element
	/// </summary>
	private readonly NullKeyDictionary<TElement, int> _elementIndex;

	/// <summary>
	/// Custom comparer used to order the heap.
	/// </summary>
	private readonly IComparer<TPriority>? _priorityComparer;

	/// <summary>
	/// Custom comparer used identify unique elements.
	/// </summary>
	private readonly IEqualityComparer<TElement>? _elementComparer;

	/// <summary>
	/// Lazily-initialized collection used to expose the contents of the queue.
	/// </summary>
	private UnorderedItemsCollection? _unorderedItems;

	/// <summary>
	/// Version updated on mutation to help validate enumerators operate on a consistent state.
	/// </summary>
	private int _version;

	/// <summary>
	/// Specifies the arity of the d-ary heap, which here is quaternary.
	/// It is assumed that this value is a power of 2.
	/// </summary>
	private const int Arity = 4;

	/// <summary>
	/// The binary logarithm of <see cref="Arity" />.
	/// </summary>
	private const int Log2Arity = 2;

#if DEBUG
	static UpdatablePriorityQueue()
	{
		Debug.Assert(Log2Arity > 0 && Math.Pow(2, Log2Arity) == Arity);
	}
#endif

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class.
	/// </summary>
	public UpdatablePriorityQueue()
	{
		_nodes = Array.Empty<(TElement, TPriority)>();
		_priorityComparer = InitializeComparer(comparer: null);
		_elementIndex = new(_elementComparer = EqualityComparer<TElement>.Default);
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  with the specified initial capacity.
	/// </summary>
	/// <param name="initialCapacity">Initial capacity to allocate in the underlying heap array.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	///  The specified <paramref name="initialCapacity"/> was negative.
	/// </exception>
	public UpdatablePriorityQueue(int initialCapacity)
		: this(initialCapacity, priorityComparer: null, elementComparer: null)
	{
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  with the specified custom priority comparer.
	/// </summary>
	/// <param name="priorityComparer">
	///  Custom comparer dictating the ordering of elements.
	///  Uses <see cref="Comparer{T}.Default" /> if the argument is <see langword="null"/>.
	/// </param>
	public UpdatablePriorityQueue(IComparer<TPriority>? priorityComparer)
	{
		_nodes = Array.Empty<(TElement, TPriority)>();
		_priorityComparer = InitializeComparer(priorityComparer);
		_elementIndex = new(_elementComparer = EqualityComparer<TElement>.Default);
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  with the specified custom priority comparer.
	/// </summary>
	/// <param name="elementComparer">
	///  Custom comparer dictating the equality of two elements.
	///  Uses <see cref="EqualityComparer{T}.Default"/> if the argument is <see langword="null"/>.
	/// </param>
	public UpdatablePriorityQueue(IEqualityComparer<TElement>? elementComparer)
	{
		_nodes = Array.Empty<(TElement, TPriority)>();
		_priorityComparer = InitializeComparer(comparer: null);
		_elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;
		_elementIndex = new(_elementComparer);
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  with the specified initial capacity and custom priority comparer.
	/// </summary>
	/// <param name="initialCapacity">Initial capacity to allocate in the underlying heap array.</param>
	/// <param name="priorityComparer">
	///  Custom comparer dictating the ordering of elements.
	///  Uses <see cref="Comparer{T}.Default" /> if the argument is <see langword="null"/>.
	/// </param>
	/// <param name="elementComparer">
	///  Custom comparer dictating the equality of two elements.
	///  Uses <see cref="EqualityComparer{T}.Default"/> if the argument is <see langword="null"/>.
	/// </param>
	/// <exception cref="ArgumentOutOfRangeException">
	///  The specified <paramref name="initialCapacity"/> was negative.
	/// </exception>
	public UpdatablePriorityQueue(int initialCapacity, IComparer<TPriority>? priorityComparer, IEqualityComparer<TElement>? elementComparer)
	{
		Guard.IsGreaterThanOrEqualTo(initialCapacity, 0);

		_nodes = new (TElement, TPriority)[initialCapacity];
		_priorityComparer = InitializeComparer(priorityComparer);
		_elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;
		_elementIndex = new(_elementComparer);
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  that is populated with the specified elements and priorities.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities with which to populate the queue.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///  Constructs the heap using a heapify operation,
	///  which is generally faster than enqueuing individual elements sequentially.
	/// </remarks>
	public UpdatablePriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items)
		: this(items, priorityComparer: null, elementComparer: null)
	{
	}

	/// <summary>
	///  Initializes a new instance of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> class
	///  that is populated with the specified elements and priorities,
	///  and with the specified custom priority comparer.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities with which to populate the queue.</param>
	/// <param name="priorityComparer">
	///  Custom comparer dictating the ordering of elements.
	///  Uses <see cref="Comparer{T}.Default" /> if the argument is <see langword="null"/>.
	/// </param>
	/// <param name="elementComparer">
	///  Custom comparer dictating the equality of two elements.
	///  Uses <see cref="EqualityComparer{T}.Default"/> if the argument is <see langword="null"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>
	///  Constructs the heap using a heapify operation,
	///  which is generally faster than enqueuing individual elements sequentially.
	/// </remarks>
	public UpdatablePriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items, IComparer<TPriority>? priorityComparer, IEqualityComparer<TElement>? elementComparer)
	{
		Guard.IsNotNull(items);

		_nodes = items.ToArray();
		_priorityComparer = InitializeComparer(priorityComparer);
		_elementComparer = elementComparer ?? EqualityComparer<TElement>.Default;
		_elementIndex = new(_elementComparer);

		Count = _nodes.Length;
		if (Count > 1)
		{
			Heapify();
		}
	}

	/// <summary>
	///  Gets the number of elements contained in the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	///  Gets the priority comparer used by the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	public IComparer<TPriority> Comparer => _priorityComparer ?? Comparer<TPriority>.Default;

	/// <summary>
	///  Gets a collection that enumerates the elements of the queue in an unordered manner.
	/// </summary>
	/// <remarks>
	///  The enumeration does not order items by priority, since that would require N * log(N) time and N space.
	///  Items are instead enumerated following the internal array heap layout.
	/// </remarks>
	public IReadOnlyCollection<(TElement Element, TPriority Priority)> UnorderedItems => _unorderedItems ??= new UnorderedItemsCollection(this);

	/// <summary>
	///  Adds or updates the specified element with associated priority to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	/// <param name="element">The element to add to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</param>
	/// <param name="priority">The priority with which to associate the new element.</param>
	/// <remarks>If an elements already exists in this queue, the priority for that element is unconditionally updated to the new value.</remarks>
	public void Enqueue(TElement element, TPriority priority)
	{
		_version++;

		if (_elementIndex.TryGetValue(element, out var index))
		{
			if (_priorityComparer == null)
			{
				var cmp = Comparer<TPriority>.Default.Compare(_nodes[index].Priority, priority);
				if (cmp > 0)
					MoveUpDefaultComparer((element, priority), index);
				else if (cmp < 0)
					MoveDownDefaultComparer((element, priority), index);
			}
			else
			{
				var cmp = _priorityComparer.Compare(_nodes[index].Priority, priority);
				if (cmp > 0)
					MoveUpCustomComparer((element, priority), index);
				else if (cmp < 0)
					MoveDownCustomComparer((element, priority), index);
			}
		}
		else
		{
			// Virtually add the node at the end of the underlying array.
			// Note that the node being enqueued does not need to be physically placed
			// there at this point, as such an assignment would be redundant.

			var currentSize = Count++;
			if (_nodes.Length == currentSize)
			{
				Grow(currentSize + 1);
			}

			if (_priorityComparer == null)
			{
				MoveUpDefaultComparer((element, priority), currentSize);
			}
			else
			{
				MoveUpCustomComparer((element, priority), currentSize);
			}
		}
	}

	/// <summary>
	///  Adds or updates the specified element with associated priority to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  using the lessor of the existing or the new priority.
	/// </summary>
	/// <param name="element">The element to add to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</param>
	/// <param name="priority">The priority with which to associate the new element.</param>
	/// <remarks>If an elements already exists in this queue, the priority for that element is set to the lessor of the new and the old priorities.</remarks>
	public void EnqueueMinimum(TElement element, TPriority priority)
	{
		_version++;

		if (_elementIndex.TryGetValue(element, out var index))
		{
			if (_priorityComparer == null)
			{
				var cmp = Comparer<TPriority>.Default.Compare(_nodes[index].Priority, priority);
				if (cmp > 0)
					MoveUpDefaultComparer((element, priority), index);
			}
			else
			{
				var cmp = _priorityComparer.Compare(_nodes[index].Priority, priority);
				if (cmp > 0)
					MoveUpCustomComparer((element, priority), index);
			}
		}
		else
		{
			// Virtually add the node at the end of the underlying array.
			// Note that the node being enqueued does not need to be physically placed
			// there at this point, as such an assignment would be redundant.

			var currentSize = Count++;
			if (_nodes.Length == currentSize)
			{
				Grow(currentSize + 1);
			}

			if (_priorityComparer == null)
			{
				MoveUpDefaultComparer((element, priority), currentSize);
			}
			else
			{
				MoveUpCustomComparer((element, priority), currentSize);
			}
		}
	}

	/// <summary>
	///  Returns the minimal element from the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> without removing it.
	/// </summary>
	/// <exception cref="InvalidOperationException">The <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> is empty.</exception>
	/// <returns>The minimal element of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</returns>
	public TElement Peek()
	{
		if (Count == 0)
		{
			ThrowHelper.ThrowInvalidOperationException("Queue empty.");
		}

		return _nodes[0].Element;
	}

	/// <summary>
	///  Removes and returns the minimal element from the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">The queue is empty.</exception>
	/// <returns>The minimal element of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</returns>
	public TElement Dequeue()
	{
		if (Count == 0)
		{
			ThrowHelper.ThrowInvalidOperationException("Queue empty.");
		}

		var element = _nodes[0].Element;
		RemoveRootNode();
		return element;
	}

	/// <summary>
	///  Removes the minimal element from the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  and copies it to the <paramref name="element"/> parameter,
	///  and its associated priority to the <paramref name="priority"/> parameter.
	/// </summary>
	/// <param name="element">The removed element.</param>
	/// <param name="priority">The priority associated with the removed element.</param>
	/// <returns>
	///  <see langword="true"/> if the element is successfully removed;
	///  <see langword="false"/> if the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> is empty.
	/// </returns>
	public bool TryDequeue([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
	{
		if (Count != 0)
		{
			(element, priority) = _nodes[0];
			RemoveRootNode();
			return true;
		}

		element = default;
		priority = default;
		return false;
	}

	/// <summary>
	///  Returns a value that indicates whether there is a minimal element in the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  and if one is present, copies it to the <paramref name="element"/> parameter,
	///  and its associated priority to the <paramref name="priority"/> parameter.
	///  The element is not removed from the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	/// <param name="element">The minimal element in the queue.</param>
	/// <param name="priority">The priority associated with the minimal element.</param>
	/// <returns>
	///  <see langword="true"/> if there is a minimal element;
	///  <see langword="false"/> if the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> is empty.
	/// </returns>
	public bool TryPeek([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
	{
		if (Count != 0)
		{
			(element, priority) = _nodes[0];
			return true;
		}

		element = default;
		priority = default;
		return false;
	}

	/// <summary>
	///  Adds the specified element with associated priority to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  and immediately removes the minimal element, returning the result.
	/// </summary>
	/// <param name="element">The element to add to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</param>
	/// <param name="priority">The priority with which to associate the new element.</param>
	/// <returns>The minimal element removed after the enqueue operation.</returns>
	/// <remarks>
	///  Implements an insert-then-extract heap operation that is generally more efficient
	///  than sequencing Enqueue and Dequeue operations: in the worst case scenario only one
	///  sift-down operation is required.
	/// </remarks>
	public TElement EnqueueDequeue(TElement element, TPriority priority)
	{
		if (Count != 0)
		{
			var (rootElement, rootPriority) = _nodes[0];
			_ = _elementIndex.Remove(rootElement);

			if (_priorityComparer == null)
			{
				if (Comparer<TPriority>.Default.Compare(priority, rootPriority) > 0)
				{
					MoveDownDefaultComparer((element, priority), 0);
					_version++;
					return rootElement;
				}
			}
			else
			{
				if (_priorityComparer.Compare(priority, rootPriority) > 0)
				{
					MoveDownCustomComparer((element, priority), 0);
					_version++;
					return rootElement;
				}
			}
		}

		return element;
	}

	/// <summary>
	///  Enqueues a sequence of element/priority pairs to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities to add to the queue.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>Any existing elements will be unconditionally updated to the new priority.</remarks>
	public void EnqueueRange(IEnumerable<(TElement Element, TPriority Priority)> items)
	{
		Guard.IsNotNull(items);

		var count = 0;
		var collection = items as ICollection<(TElement Element, TPriority Priority)>;
		if (collection is not null && (count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			if (collection is not null)
			{
				collection.CopyTo(_nodes, 0);
				Count = count;
			}
			else
			{
				var i = 0;
				var nodes = _nodes;
				foreach (var (element, priority) in items)
				{
					if (nodes.Length == i)
					{
						Grow(i + 1);
						nodes = _nodes;
					}

					nodes[i++] = (element, priority);
				}

				Count = i;
			}

			_version++;

			if (Count > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach (var (element, priority) in items)
			{
				Enqueue(element, priority);
			}
		}
	}

	/// <summary>
	///  Enqueues a sequence of elements pairs to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  all associated with the specified priority.
	/// </summary>
	/// <param name="elements">The elements to add to the queue.</param>
	/// <param name="priority">The priority to associate with the new elements.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="elements"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>Any existing elements will be unconditionally updated to the new priority.</remarks>
	public void EnqueueRange(IEnumerable<TElement> elements, TPriority priority)
	{
		Guard.IsNotNull(elements);

		int count;
		if (elements is ICollection<(TElement Element, TPriority Priority)> collection &&
			(count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			var i = 0;
			var nodes = _nodes;
			foreach (var element in elements)
			{
				if (nodes.Length == i)
				{
					Grow(i + 1);
					nodes = _nodes;
				}

				nodes[i++] = (element, priority);
			}

			Count = i;
			_version++;

			if (i > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach (var element in elements)
			{
				Enqueue(element, priority);
			}
		}
	}

	/// <summary>
	///  Enqueues a sequence of element/priority pairs to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	/// <param name="items">The pairs of elements and priorities to add to the queue.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="items"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>Any existing elements will be updated to the new priority if and only if the new priority is lower than the existing priority.</remarks>
	public void EnqueueRangeMinimum(IEnumerable<(TElement Element, TPriority Priority)> items)
	{
		Guard.IsNotNull(items);

		var count = 0;
		var collection = items as ICollection<(TElement Element, TPriority Priority)>;
		if (collection is not null && (count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			if (collection is not null)
			{
				collection.CopyTo(_nodes, 0);
				Count = count;
			}
			else
			{
				var i = 0;
				var nodes = _nodes;
				foreach (var (element, priority) in items)
				{
					if (nodes.Length == i)
					{
						Grow(i + 1);
						nodes = _nodes;
					}

					nodes[i++] = (element, priority);
				}

				Count = i;
			}

			_version++;

			if (Count > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach (var (element, priority) in items)
			{
				EnqueueMinimum(element, priority);
			}
		}
	}

	/// <summary>
	///  Enqueues a sequence of elements pairs to the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  all associated with the specified priority.
	/// </summary>
	/// <param name="elements">The elements to add to the queue.</param>
	/// <param name="priority">The priority to associate with the new elements.</param>
	/// <exception cref="ArgumentNullException">
	///  The specified <paramref name="elements"/> argument was <see langword="null"/>.
	/// </exception>
	/// <remarks>Any existing elements will be updated to the new priority if and only if the new priority is lower than the existing priority.</remarks>
	public void EnqueueRangeMinimum(IEnumerable<TElement> elements, TPriority priority)
	{
		Guard.IsNotNull(elements);

		int count;
		if (elements is ICollection<(TElement Element, TPriority Priority)> collection &&
			(count = collection.Count) > _nodes.Length - Count)
		{
			Grow(Count + count);
		}

		if (Count == 0)
		{
			// build using Heapify() if the queue is empty.

			var i = 0;
			var nodes = _nodes;
			foreach (var element in elements)
			{
				if (nodes.Length == i)
				{
					Grow(i + 1);
					nodes = _nodes;
				}

				nodes[i++] = (element, priority);
			}

			Count = i;
			_version++;

			if (i > 1)
			{
				Heapify();
			}
		}
		else
		{
			foreach (var element in elements)
			{
				EnqueueMinimum(element, priority);
			}
		}
	}

	/// <summary>
	///  Removes all items from the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.
	/// </summary>
	public void Clear()
	{
		if (RuntimeHelpers.IsReferenceOrContainsReferences<(TElement, TPriority)>())
		{
			// Clear the elements so that the gc can reclaim the references
			Array.Clear(_nodes, 0, Count);
			_elementIndex.Clear();
		}
		Count = 0;
		_version++;
	}

	/// <summary>
	///  Ensures that the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/> can hold up to
	///  <paramref name="capacity"/> items without further expansion of its backing storage.
	/// </summary>
	/// <param name="capacity">The minimum capacity to be used.</param>
	/// <exception cref="ArgumentOutOfRangeException">
	///  The specified <paramref name="capacity"/> is negative.
	/// </exception>
	/// <returns>The current capacity of the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>.</returns>
	public int EnsureCapacity(int capacity)
	{
		Guard.IsGreaterThanOrEqualTo(capacity, 0);

		if (_nodes.Length < capacity)
		{
			Grow(capacity);
			_version++;
		}

		return _nodes.Length;
	}

	/// <summary>
	///  Sets the capacity to the actual number of items in the <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
	///  if that is less than 90 percent of current capacity.
	/// </summary>
	/// <remarks>
	///  This method can be used to minimize a collection's memory overhead
	///  if no new elements will be added to the collection.
	/// </remarks>
	public void TrimExcess()
	{
		var threshold = (int)(_nodes.Length * 0.9);
		if (Count < threshold)
		{
			Array.Resize(ref _nodes, Count);
			_version++;
		}
	}

	/// <summary>
	/// Grows the priority queue to match the specified min capacity.
	/// </summary>
	private void Grow(int minCapacity)
	{
		Debug.Assert(_nodes.Length < minCapacity);

		const int GrowFactor = 2;
		const int MinimumGrow = 4;

		var newcapacity = GrowFactor * _nodes.Length;

		// Allow the queue to grow to maximum possible capacity (~2G elements) before encountering overflow.
		// Note that this check works even when _nodes.Length overflowed thanks to the (uint) cast
		if ((uint)newcapacity > int.MaxValue) newcapacity = int.MaxValue;

		// Ensure minimum growth is respected.
		newcapacity = Math.Max(newcapacity, _nodes.Length + MinimumGrow);

		// If the computed capacity is still less than specified, set to the original argument.
		// Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
		if (newcapacity < minCapacity) newcapacity = minCapacity;

		Array.Resize(ref _nodes, newcapacity);
	}

	/// <summary>
	/// Removes the node from the root of the heap
	/// </summary>
	private void RemoveRootNode()
	{
		var lastNodeIndex = --Count;
		_version++;

		_ = _elementIndex.Remove(_nodes[0].Element);

		if (lastNodeIndex > 0)
		{
			var lastNode = _nodes[lastNodeIndex];
			if (_priorityComparer == null)
			{
				MoveDownDefaultComparer(lastNode, 0);
			}
			else
			{
				MoveDownCustomComparer(lastNode, 0);
			}
		}

		if (RuntimeHelpers.IsReferenceOrContainsReferences<(TElement, TPriority)>())
		{
			_nodes[lastNodeIndex] = default;
		}
	}

	/// <summary>
	/// Gets the index of an element's parent.
	/// </summary>
	private static int GetParentIndex(int index) => (index - 1) >> Log2Arity;

	/// <summary>
	/// Gets the index of the first child of an element.
	/// </summary>
	private static int GetFirstChildIndex(int index) => (index << Log2Arity) + 1;

	/// <summary>
	/// Converts an unordered list into a heap.
	/// </summary>
	private void Heapify()
	{
		var nodes = _nodes;

		_elementIndex.Clear();
		for (var index = 0; index < Count; index++)
		{
			if (_elementIndex.TryGetValue(nodes[index].Element, out var oldIndex))
			{
				var oldPriority = nodes[oldIndex].Priority;
				var newPriority = nodes[index].Priority;

				if ((_priorityComparer ?? Comparer<TPriority>.Default).Compare(oldPriority, newPriority) > 0)
					nodes[oldIndex].Priority = newPriority;

				nodes[index] = nodes[--Count];

				// so we process same index again next loop
				index--;
			}
			else
			{
				_elementIndex[nodes[index].Element] = index;
			}
		}

		// Leaves of the tree are in fact 1-element heaps, for which there
		// is no need to correct them. The heap property needs to be restored
		// only for higher nodes, starting from the first node that has children.
		// It is the parent of the very last element in the array.

		var lastParentWithChildren = GetParentIndex(Count - 1);

		if (_priorityComparer == null)
		{
			for (var index = lastParentWithChildren; index >= 0; --index)
			{
				MoveDownDefaultComparer(nodes[index], index);
			}
		}
		else
		{
			for (var index = lastParentWithChildren; index >= 0; --index)
			{
				MoveDownCustomComparer(nodes[index], index);
			}
		}
	}

	/// <summary>
	/// Moves a node up in the tree to restore heap order.
	/// </summary>
	private void MoveUpDefaultComparer((TElement Element, TPriority Priority) node, int nodeIndex)
	{
		// Instead of swapping items all the way to the root, we will perform
		// a similar optimization as in the insertion sort.

		Debug.Assert(_priorityComparer is null);
		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		var nodes = _nodes;

		while (nodeIndex > 0)
		{
			var parentIndex = GetParentIndex(nodeIndex);
			var parent = nodes[parentIndex];

			if (Comparer<TPriority>.Default.Compare(node.Priority, parent.Priority) < 0)
			{
				nodes[nodeIndex] = parent;
				_elementIndex[parent.Element] = nodeIndex;

				nodeIndex = parentIndex;
			}
			else
			{
				break;
			}
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}

	/// <summary>
	/// Moves a node up in the tree to restore heap order.
	/// </summary>
	private void MoveUpCustomComparer((TElement Element, TPriority Priority) node, int nodeIndex)
	{
		// Instead of swapping items all the way to the root, we will perform
		// a similar optimization as in the insertion sort.

		Debug.Assert(_priorityComparer is not null);
		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		var comparer = _priorityComparer;
		var nodes = _nodes;

		while (nodeIndex > 0)
		{
			var parentIndex = GetParentIndex(nodeIndex);
			var parent = nodes[parentIndex];

			if (comparer.Compare(node.Priority, parent.Priority) < 0)
			{
				nodes[nodeIndex] = parent;
				_elementIndex[parent.Element] = nodeIndex;

				nodeIndex = parentIndex;
			}
			else
			{
				break;
			}
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}

	/// <summary>
	/// Moves a node down in the tree to restore heap order.
	/// </summary>
	private void MoveDownDefaultComparer((TElement Element, TPriority Priority) node, int nodeIndex)
	{
		// The node to move down will not actually be swapped every time.
		// Rather, values on the affected path will be moved up, thus leaving a free spot
		// for this value to drop in. Similar optimization as in the insertion sort.

		Debug.Assert(_priorityComparer is null);
		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		var nodes = _nodes;
		var size = Count;

		int i;
		while ((i = GetFirstChildIndex(nodeIndex)) < size)
		{
			// Find the child node with the minimal priority
			var minChild = nodes[i];
			var minChildIndex = i;

			var childIndexUpperBound = Math.Min(i + Arity, size);
			while (++i < childIndexUpperBound)
			{
				var nextChild = nodes[i];
				if (Comparer<TPriority>.Default.Compare(nextChild.Priority, minChild.Priority) < 0)
				{
					minChild = nextChild;
					minChildIndex = i;
				}
			}

			// Heap property is satisfied; insert node in this location.
			if (Comparer<TPriority>.Default.Compare(node.Priority, minChild.Priority) <= 0)
			{
				break;
			}

			// Move the minimal child up by one node and
			// continue recursively from its location.
			nodes[nodeIndex] = minChild;
			_elementIndex[minChild.Element] = nodeIndex;

			nodeIndex = minChildIndex;
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}

	/// <summary>
	/// Moves a node down in the tree to restore heap order.
	/// </summary>
	private void MoveDownCustomComparer((TElement Element, TPriority Priority) node, int nodeIndex)
	{
		// The node to move down will not actually be swapped every time.
		// Rather, values on the affected path will be moved up, thus leaving a free spot
		// for this value to drop in. Similar optimization as in the insertion sort.

		Debug.Assert(_priorityComparer is not null);
		Debug.Assert(0 <= nodeIndex && nodeIndex < Count);

		var comparer = _priorityComparer;
		var nodes = _nodes;
		var size = Count;

		int i;
		while ((i = GetFirstChildIndex(nodeIndex)) < size)
		{
			// Find the child node with the minimal priority
			var minChild = nodes[i];
			var minChildIndex = i;

			var childIndexUpperBound = Math.Min(i + Arity, size);
			while (++i < childIndexUpperBound)
			{
				var nextChild = nodes[i];
				if (comparer.Compare(nextChild.Priority, minChild.Priority) < 0)
				{
					minChild = nextChild;
					minChildIndex = i;
				}
			}

			// Heap property is satisfied; insert node in this location.
			if (comparer.Compare(node.Priority, minChild.Priority) <= 0)
			{
				break;
			}

			// Move the minimal child up by one node and continue recursively from its location.
			nodes[nodeIndex] = minChild;
			_elementIndex[minChild.Element] = nodeIndex;

			nodeIndex = minChildIndex;
		}

		nodes[nodeIndex] = node;
		_elementIndex[node.Element] = nodeIndex;
	}

	/// <summary>
	/// Initializes the custom comparer to be used internally by the heap.
	/// </summary>
	private static IComparer<TPriority>? InitializeComparer(IComparer<TPriority>? comparer)
	{
		if (typeof(TPriority).IsValueType)
		{
			if (comparer == Comparer<TPriority>.Default)
			{
				// if the user manually specifies the default comparer,
				// revert to using the optimized path.
				return null;
			}

			return comparer;
		}
		else
		{
			// Currently the JIT doesn't optimize direct Comparer<T>.Default.Compare
			// calls for reference types, so we want to cache the comparer instance instead.
			return comparer ?? Comparer<TPriority>.Default;
		}
	}

	/// <summary>
	///  Enumerates the contents of a <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>, without any ordering guarantees.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(PriorityQueueDebugView<,>))]
	internal sealed class UnorderedItemsCollection : IReadOnlyCollection<(TElement Element, TPriority Priority)>, ICollection
	{
		internal readonly UpdatablePriorityQueue<TElement, TPriority> _queue;

		internal UnorderedItemsCollection(UpdatablePriorityQueue<TElement, TPriority> queue) => _queue = queue;

		/// <inheritdoc />
		public int Count => _queue.Count;
		object ICollection.SyncRoot => this;
		bool ICollection.IsSynchronized => false;

		void ICollection.CopyTo(Array array, int index)
		{
			Guard.IsNotNull(array);

			try
			{
				Array.Copy(_queue._nodes, 0, array, index, _queue.Count);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
		}

		/// <summary>
		///  Enumerates the element and priority pairs of a <see cref="UpdatablePriorityQueue{TElement, TPriority}"/>,
		///  without any ordering guarantees.
		/// </summary>
		public struct Enumerator : IEnumerator<(TElement Element, TPriority Priority)>
		{
			private readonly UpdatablePriorityQueue<TElement, TPriority> _queue;
			private readonly int _version;
			private int _index;
			private (TElement, TPriority) _current;

			internal Enumerator(UpdatablePriorityQueue<TElement, TPriority> queue)
			{
				_queue = queue;
				_index = 0;
				_version = queue._version;
				_current = default;
			}

			/// <summary>
			/// Releases all resources used by the <see cref="Enumerator"/>.
			/// </summary>
			public readonly void Dispose() { }

			/// <summary>
			/// Advances the enumerator to the next element of the <see cref="UnorderedItems"/>.
			/// </summary>
			/// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				var localQueue = _queue;

				if (_version == localQueue._version && ((uint)_index < (uint)localQueue.Count))
				{
					_current = localQueue._nodes[_index];
					_index++;
					return true;
				}

				return MoveNextRare();
			}

			private bool MoveNextRare()
			{
				if (_version != _queue._version)
				{
					ThrowHelper.ThrowInvalidOperationException(
						"Collection was modified; enumeration operation may not execute.");
				}

				_index = _queue.Count + 1;
				_current = default;
				return false;
			}

			/// <summary>
			/// Gets the element at the current position of the enumerator.
			/// </summary>
			public (TElement Element, TPriority Priority) Current => _current;
			object IEnumerator.Current => _current;

			void IEnumerator.Reset()
			{
				if (_version != _queue._version)
				{
					ThrowHelper.ThrowInvalidOperationException(
						"Collection was modified; enumeration operation may not execute.");
				}

				_index = 0;
				_current = default;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the <see cref="UnorderedItems"/>.
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> for the <see cref="UnorderedItems"/>.</returns>
		public Enumerator GetEnumerator() => new(_queue);

		IEnumerator<(TElement Element, TPriority Priority)> IEnumerable<(TElement Element, TPriority Priority)>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

internal sealed class PriorityQueueDebugView<TElement, TPriority>
{
	private readonly UpdatablePriorityQueue<TElement, TPriority> _queue;
	private readonly bool _sort;

	public PriorityQueueDebugView(UpdatablePriorityQueue<TElement, TPriority> queue)
	{
		Guard.IsNotNull(queue);

		_queue = queue;
		_sort = true;
	}

	public PriorityQueueDebugView(UpdatablePriorityQueue<TElement, TPriority>.UnorderedItemsCollection collection)
	{
		_queue = collection?._queue ?? throw new ArgumentNullException(nameof(collection));
	}

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public (TElement Element, TPriority Priority)[] Items
	{
		get
		{
			var list = new List<(TElement Element, TPriority Priority)>(_queue.UnorderedItems);
			if (_sort)
			{
				list.Sort((i1, i2) => _queue.Comparer.Compare(i1.Priority, i2.Priority));
			}
			return list.ToArray();
		}
	}
}
