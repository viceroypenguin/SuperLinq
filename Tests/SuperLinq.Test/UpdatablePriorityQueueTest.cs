// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// copied from https://github.com/dotnet/runtime/tree/main/src/libraries/System.Collections/tests/Generic/PriorityQueue
// and further edited

using System.Reflection;
using SuperLinq.Collections;

namespace Test;

public class UpdatableUpdatablePriorityQueueTest
{
	public abstract class TestBase
	{
		public static IEnumerable<object[]> ValidCollectionSizes()
		{
			yield return new object[] { 0 };
			yield return new object[] { 1 };
			yield return new object[] { 75 };
		}

		public static IEnumerable<object[]> ValidPositiveCollectionSizes()
		{
			yield return new object[] { 1 };
			yield return new object[] { 75 };
		}
	}

	public abstract class UpdatablePriorityQueue_Generic_Tests<TElement, TPriority> : TestBase
	{

		#region Helpers
		protected abstract (TElement, TPriority) CreateT(int seed);

		protected virtual IComparer<TPriority>? GetPriorityComparer() => Comparer<TPriority>.Default;
		protected virtual IEqualityComparer<TElement>? GetElementComparer() => EqualityComparer<TElement>.Default;
		private IEqualityComparer<(TElement, TPriority)> GetNodeComparer() => ValueTupleEqualityComparer.Create(GetElementComparer(), EqualityComparer<TPriority>.Default);

		protected IEnumerable<(TElement, TPriority)> CreateItems(int count)
		{
			const int MagicValue = 34;
			var seed = count * MagicValue;
			for (var i = 0; i < count; i++)
			{
				yield return CreateT(seed++);
			}
		}

		protected UpdatablePriorityQueue<TElement, TPriority> CreateEmptyUpdatablePriorityQueue(int initialCapacity = 0)
			=> new(initialCapacity, GetPriorityComparer(), GetElementComparer());

		protected UpdatablePriorityQueue<TElement, TPriority> CreateUpdatablePriorityQueue(
			int initialCapacity, int countOfItemsToGenerate, out List<(TElement element, TPriority priority)> generatedItems)
		{
			generatedItems = CreateItems(countOfItemsToGenerate).ToList();
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(initialCapacity, GetPriorityComparer(), GetElementComparer());
			queue.EnqueueRange(generatedItems);
			return queue;
		}
		#endregion

		#region Constructors

		[Fact]
		public void UpdatablePriorityQueue_DefaultConstructor_ComparerEqualsDefaultComparer()
		{
			var queue = new UpdatablePriorityQueue<TElement, TPriority>();

			Assert.Equal(expected: 0, queue.Count);
			Assert.Empty(queue.UnorderedItems);
			Assert.Equal(queue.Comparer, Comparer<TPriority>.Default);
		}

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_EmptyCollection_UnorderedItemsIsEmpty(int initialCapacity)
		{
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(initialCapacity);
			Assert.Empty(queue.UnorderedItems);
		}

		[Fact]
		public void UpdatablePriorityQueue_ComparerConstructor_ComparerShouldEqualParameter()
		{
			var comparer = GetPriorityComparer();
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(comparer);
			Assert.Equal(comparer, queue.Comparer);
		}

		[Fact]
		public void UpdatablePriorityQueue_PriorityComparerConstructorNull_ComparerShouldEqualDefaultComparer()
		{
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(priorityComparer: null);
			Assert.Equal(0, queue.Count);
			Assert.Same(Comparer<TPriority>.Default, queue.Comparer);
		}

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_CapacityConstructor_ComparerShouldEqualDefaultComparer(int initialCapacity)
		{
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(initialCapacity);
			Assert.Empty(queue.UnorderedItems);
			Assert.Same(Comparer<TPriority>.Default, queue.Comparer);
		}

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_EnumerableConstructor_ShouldContainAllElements(int count)
		{
			var itemsToEnqueue = CreateItems(count).ToArray();
			var queue = new UpdatablePriorityQueue<TElement, TPriority>(itemsToEnqueue, GetPriorityComparer(), GetElementComparer());
			Assert.Equal(itemsToEnqueue.Length, queue.Count);
			queue.UnorderedItems.AssertCollectionEqual(itemsToEnqueue, comparer: GetNodeComparer());
		}

		#endregion

		#region Enqueue, Dequeue, Peek, EnqueueDequeue

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_Enqueue_IEnumerable(int count)
		{
			var itemsToEnqueue = CreateItems(count).ToArray();
			var queue = CreateEmptyUpdatablePriorityQueue();

			foreach ((var element, var priority) in itemsToEnqueue)
			{
				queue.Enqueue(element, priority);
			}

			queue.UnorderedItems.AssertCollectionEqual(itemsToEnqueue);
		}

		[Theory]
		[MemberData(nameof(ValidPositiveCollectionSizes))]
		public void UpdatablePriorityQueue_Peek_ShouldReturnMinimalElement(int count)
		{
			var itemsToEnqueue = CreateItems(count).ToArray();
			var queue = CreateEmptyUpdatablePriorityQueue();
			(TElement Element, TPriority Priority) minItem = itemsToEnqueue[0];

			foreach (var (element, priority) in itemsToEnqueue)
			{
				if (queue.Comparer.Compare(priority, minItem.Priority) < 0)
				{
					minItem = (element, priority);
				}

				queue.Enqueue(element, priority);

				var actualPeekElement = queue.Peek();
				Assert.Equal(minItem.Element, actualPeekElement);

				var actualTryPeekSuccess = queue.TryPeek(out var actualTryPeekElement, out var actualTryPeekPriority);
				Assert.True(actualTryPeekSuccess);
				Assert.Equal(minItem.Element, actualTryPeekElement);
				Assert.Equal(minItem.Priority, actualTryPeekPriority);
			}
		}

		[Theory]
		[InlineData(0, 5)]
		[InlineData(1, 1)]
		[InlineData(3, 100)]
		public void UpdatablePriorityQueue_PeekAndDequeue(int initialCapacity, int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity, count, out var generatedItems);

			var expectedPeekPriorities = generatedItems
				.Select(x => x.priority)
				.OrderBy(SuperEnumerable.Identity, queue.Comparer)
				.ToArray();

			for (var i = 0; i < count; ++i)
			{
				var expectedPeekPriority = expectedPeekPriorities[i];

				var actualTryPeekSuccess = queue.TryPeek(out var actualTryPeekElement, out var actualTryPeekPriority);
				var actualTryDequeueSuccess = queue.TryDequeue(out var actualTryDequeueElement, out var actualTryDequeuePriority);

				Assert.True(actualTryPeekSuccess);
				Assert.True(actualTryDequeueSuccess);
				Assert.Equal(expectedPeekPriority, actualTryPeekPriority);
				Assert.Equal(expectedPeekPriority, actualTryDequeuePriority);
			}

			Assert.Equal(expected: 0, queue.Count);
			Assert.False(queue.TryPeek(out _, out _));
			Assert.False(queue.TryDequeue(out _, out _));
		}

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_EnqueueRange_IEnumerable(int count)
		{
			var itemsToEnqueue = CreateItems(count).ToArray();
			var queue = CreateEmptyUpdatablePriorityQueue();

			queue.EnqueueRange(itemsToEnqueue);

			queue.UnorderedItems.AssertCollectionEqual(itemsToEnqueue, comparer: GetNodeComparer());
		}

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_EnqueueDequeue(int count)
		{
			(TElement Element, TPriority Priority)[] itemsToEnqueue = CreateItems(2 * count).ToArray();
			var queue = CreateEmptyUpdatablePriorityQueue();
			queue.EnqueueRange(itemsToEnqueue.Take(count));

			foreach ((var element, var priority) in itemsToEnqueue.Skip(count))
			{
				_ = queue.EnqueueDequeue(element, priority);
			}

			var expectedItems = itemsToEnqueue.OrderByDescending(x => x.Priority, queue.Comparer).Take(count);
			queue.UnorderedItems.AssertCollectionEqual(expectedItems, comparer: GetNodeComparer());
		}

		#endregion

		#region Clear

		[Theory]
		[MemberData(nameof(ValidCollectionSizes))]
		public void UpdatablePriorityQueue_Clear(int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity: 0, count, out _);
			Assert.Equal(count, queue.Count);

			queue.Clear();

			Assert.Equal(expected: 0, queue.Count);
			Assert.False(queue.TryPeek(out _, out _));
		}

		#endregion

		#region Enumeration

		[Theory]
		[MemberData(nameof(ValidPositiveCollectionSizes))]
		public void UpdatablePriorityQueue_Enumeration_OrderingIsConsistent(int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity: 0, count, out _);

			(TElement, TPriority)[] firstEnumeration = queue.UnorderedItems.ToArray();
			(TElement, TPriority)[] secondEnumeration = queue.UnorderedItems.ToArray();

			Assert.Equal(firstEnumeration.Length, count);
			Assert.True(firstEnumeration.SequenceEqual(secondEnumeration));
		}

		#endregion
	}

	public class UpdatablePriorityQueue_Generic_Tests_string_string : UpdatablePriorityQueue_Generic_Tests<string, string>
	{
		protected override (string, string) CreateT(int seed)
		{
			var random = new Random(seed);
			return (CreateString(random), CreateString(random));

			static string CreateString(Random random)
			{
				var stringLength = random.Next(5, 15);
				var bytes = new byte[stringLength];
				random.NextBytes(bytes);
				return Convert.ToBase64String(bytes);
			}
		}
	}

	public class UpdatablePriorityQueue_Generic_Tests_int_int : UpdatablePriorityQueue_Generic_Tests<int, int>
	{
		protected override (int, int) CreateT(int seed)
		{
			var random = new Random(seed);
			return (random.Next(), random.Next());
		}
	}

	public class PriorityQueue_Generic_Tests_string_string_CustomPriorityComparer : UpdatablePriorityQueue_Generic_Tests_string_string
	{
		protected override IComparer<string> GetPriorityComparer() => StringComparer.InvariantCultureIgnoreCase;
	}

	public class PriorityQueue_Generic_Tests_string_string_CustomElementComparer : UpdatablePriorityQueue_Generic_Tests_string_string
	{
		protected override IEqualityComparer<string>? GetElementComparer() => StringComparer.InvariantCultureIgnoreCase;
	}

	public class PriorityQueue_Generic_Tests_int_int_CustomComparer : UpdatablePriorityQueue_Generic_Tests_int_int
	{
		protected override IComparer<int> GetPriorityComparer() => Comparer<int>.Create((x, y) => -x.CompareTo(y));
	}

	public class UpdatablePriorityQueue_NonGeneric_Tests : TestBase
	{
		protected static UpdatablePriorityQueue<string, int> CreateSmallUpdatablePriorityQueue(out HashSet<(string, int)> items)
		{
			items = new HashSet<(string, int)>
			{
				("one", 1),
				("two", 2),
				("three", 3),
			};
			var queue = new UpdatablePriorityQueue<string, int>(items);

			return queue;
		}

		protected static UpdatablePriorityQueue<int, int> CreateUpdatablePriorityQueue(int initialCapacity, int count)
		{
			var pq = new UpdatablePriorityQueue<int, int>(initialCapacity);
			for (var i = 0; i < count; i++)
			{
				pq.Enqueue(i, i);
			}

			return pq;
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueDequeue_Empty()
		{
			var queue = new UpdatablePriorityQueue<string, int>();

			Assert.Equal("hello", queue.EnqueueDequeue("hello", 42));
			Assert.Equal(0, queue.Count);
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueDequeue_SmallerThanMin()
		{
			var queue = CreateSmallUpdatablePriorityQueue(out var enqueuedItems);

			var actualElement = queue.EnqueueDequeue("zero", 0);

			Assert.Equal("zero", actualElement);
			Assert.True(enqueuedItems.SetEquals(queue.UnorderedItems));
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueDequeue_LargerThanMin()
		{
			var queue = CreateSmallUpdatablePriorityQueue(out var enqueuedItems);

			var actualElement = queue.EnqueueDequeue("four", 4);

			Assert.Equal("one", actualElement);
			Assert.Equal("two", queue.Dequeue());
			Assert.Equal("three", queue.Dequeue());
			Assert.Equal("four", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueDequeue_EqualToMin()
		{
			var queue = CreateSmallUpdatablePriorityQueue(out var enqueuedItems);

			var actualElement = queue.EnqueueDequeue("one-not-to-enqueue", 1);

			Assert.Equal("one-not-to-enqueue", actualElement);
			Assert.True(enqueuedItems.SetEquals(queue.UnorderedItems));
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_Constructor_IEnumerable_Null()
		{
			var itemsToEnqueue = new (string?, int)[] { (null, 0), ("one", 1) };
			var queue = new UpdatablePriorityQueue<string?, int>(itemsToEnqueue);
			Assert.Null(queue.Dequeue());
			Assert.Equal("one", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_Enqueue_Null()
		{
			var queue = new UpdatablePriorityQueue<string?, int>();

			queue.Enqueue(element: null, 1);
			queue.Enqueue(element: "zero", 0);
			queue.Enqueue(element: "two", 2);

			Assert.Equal("zero", queue.Dequeue());
			Assert.Null(queue.Dequeue());
			Assert.Equal("two", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueRange_Null()
		{
			var queue = new UpdatablePriorityQueue<string?, int>();

			queue.EnqueueRange(new string?[] { null, null, null }, 0);
			queue.EnqueueRange(new string?[] { "not null" }, 1);
			queue.EnqueueRange(new string?[] { null, null, null }, 0);

			Assert.Null(queue.Dequeue());
			Assert.Equal("not null", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_Enqueue_ChangesPriority()
		{
			var queue = new UpdatablePriorityQueue<string, int>();

			queue.EnqueueRange(new[] { "alpha", "bravo", "charlie", "delta", "echo", }, 30);

			queue.Enqueue("alpha", 50);
			queue.Enqueue("bravo", 40);
			queue.Enqueue("charlie", 30);
			queue.Enqueue("delta", 20);
			queue.Enqueue("echo", 10);

			queue.UnorderedItems.AssertCollectionEqual(
				("alpha", 50),
				("bravo", 40),
				("charlie", 30),
				("delta", 20),
				("echo", 10));

			Assert.Equal("echo", queue.Dequeue());
			Assert.Equal("delta", queue.Dequeue());
			Assert.Equal("charlie", queue.Dequeue());
			Assert.Equal("bravo", queue.Dequeue());
			Assert.Equal("alpha", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueRange_ChangesPriority()
		{
			var queue = new UpdatablePriorityQueue<string, int>();

			queue.EnqueueRange(new[] { "alpha", "bravo", "charlie", "delta", "echo", }, 30);

			queue.EnqueueRange(
				new[]
				{
					("alpha", 50),
					("bravo", 40),
					("charlie", 30),
					("delta", 20),
					("echo", 10),
				});

			queue.UnorderedItems.AssertCollectionEqual(
				("alpha", 50),
				("bravo", 40),
				("charlie", 30),
				("delta", 20),
				("echo", 10));

			Assert.Equal("echo", queue.Dequeue());
			Assert.Equal("delta", queue.Dequeue());
			Assert.Equal("charlie", queue.Dequeue());
			Assert.Equal("bravo", queue.Dequeue());
			Assert.Equal("alpha", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueMinimum_UsesLowestPriority()
		{
			var queue = new UpdatablePriorityQueue<string, int>();

			queue.EnqueueRange(new[] { "alpha", "bravo", "charlie", "delta", "echo", }, 30);

			queue.EnqueueMinimum("alpha", 50);
			queue.EnqueueMinimum("bravo", 40);
			queue.EnqueueMinimum("charlie", 30);
			queue.EnqueueMinimum("delta", 20);
			queue.EnqueueMinimum("echo", 10);

			queue.UnorderedItems.AssertCollectionEqual(
				("alpha", 30),
				("bravo", 30),
				("charlie", 30),
				("delta", 20),
				("echo", 10));

			Assert.Equal("echo", queue.Dequeue());
			Assert.Equal("delta", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Generic_EnqueueRangeMinimum_UsesLowestPriority()
		{
			var queue = new UpdatablePriorityQueue<string, int>();

			queue.EnqueueRange(new[] { "alpha", "bravo", "charlie", "delta", "echo", }, 30);

			queue.EnqueueRangeMinimum(
				new[]
				{
					("alpha", 50),
					("bravo", 40),
					("charlie", 30),
					("delta", 20),
					("echo", 10),
				});

			queue.UnorderedItems.AssertCollectionEqual(
				("alpha", 30),
				("bravo", 30),
				("charlie", 30),
				("delta", 20),
				("echo", 10));

			Assert.Equal("echo", queue.Dequeue());
			Assert.Equal("delta", queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_Constructor_int_Negative_ThrowsArgumentOutOfRangeException()
		{
			_ = Assert.Throws<ArgumentOutOfRangeException>("initialCapacity", () => new UpdatablePriorityQueue<int, int>(-1));
			_ = Assert.Throws<ArgumentOutOfRangeException>("initialCapacity", () => new UpdatablePriorityQueue<int, int>(int.MinValue));
		}

		[Fact]
		public void UpdatablePriorityQueue_Constructor_Enumerable_null_ThrowsArgumentNullException()
		{
			_ = Assert.Throws<ArgumentNullException>("items", () => new UpdatablePriorityQueue<int, int>(items: null!));
			_ = Assert.Throws<ArgumentNullException>("items", () => new UpdatablePriorityQueue<int, int>(items: null!, priorityComparer: Comparer<int>.Default, elementComparer: EqualityComparer<int>.Default));
		}

		[Fact]
		public void UpdatablePriorityQueue_EnqueueRange_null_ThrowsArgumentNullException()
		{
			var queue = new UpdatablePriorityQueue<int, int>();
			_ = Assert.Throws<ArgumentNullException>("items", () => queue.EnqueueRange(null!));
		}

		[Fact]
		public void UpdatablePriorityQueue_EmptyCollection_Dequeue_ShouldThrowException()
		{
			var queue = new UpdatablePriorityQueue<int, int>();

			Assert.Equal(0, queue.Count);
			Assert.False(queue.TryDequeue(out _, out _));
			_ = Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
		}

		[Fact]
		public void UpdatablePriorityQueue_EmptyCollection_Peek_ShouldReturnFalse()
		{
			var queue = new UpdatablePriorityQueue<int, int>();

			Assert.False(queue.TryPeek(out _, out _));
			_ = Assert.Throws<InvalidOperationException>(() => queue.Peek());
		}

		#region EnsureCapacity, TrimExcess

		[Fact]
		public void UpdatablePriorityQueue_EnsureCapacity_Negative_ShouldThrowException()
		{
			var queue = new UpdatablePriorityQueue<int, int>();
			_ = Assert.Throws<ArgumentOutOfRangeException>("capacity", () => queue.EnsureCapacity(-1));
			_ = Assert.Throws<ArgumentOutOfRangeException>("capacity", () => queue.EnsureCapacity(int.MinValue));
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 5)]
		[InlineData(1, 1)]
		[InlineData(3, 100)]
		public void UpdatablePriorityQueue_TrimExcess_ShouldNotChangeCount(int initialCapacity, int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity, count);

			Assert.Equal(count, queue.Count);
			queue.TrimExcess();
			Assert.Equal(count, queue.Count);
		}

		[Theory]
		[MemberData(nameof(ValidPositiveCollectionSizes))]
		public void UpdatablePriorityQueue_TrimExcess_Repeatedly_ShouldNotChangeCount(int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity: count, count);

			Assert.Equal(count, queue.Count);
			queue.TrimExcess();
			queue.TrimExcess();
			queue.TrimExcess();
			Assert.Equal(count, queue.Count);
		}

		[Theory]
		[MemberData(nameof(ValidPositiveCollectionSizes))]
		public void UpdatablePriorityQueue_Generic_EnsureCapacityAndTrimExcess(int count)
		{
			IReadOnlyCollection<(int, int)> itemsToEnqueue = Enumerable.Range(1, count).Select(i => (i, i)).ToArray();
			var queue = new UpdatablePriorityQueue<int, int>();
			var expectedCount = 0;
			var random = new Random(Seed: 34);
			int GetNextEnsureCapacity() => random.Next(0, count * 2);
			void TrimAndEnsureCapacity()
			{
				queue.TrimExcess();

				var capacityAfterEnsureCapacity = queue.EnsureCapacity(GetNextEnsureCapacity());
				Assert.Equal(capacityAfterEnsureCapacity, GetUnderlyingBufferCapacity(queue));

				var capacityAfterTrimExcess = (queue.Count < (int)(capacityAfterEnsureCapacity * 0.9)) ? queue.Count : capacityAfterEnsureCapacity;
				queue.TrimExcess();
				Assert.Equal(capacityAfterTrimExcess, GetUnderlyingBufferCapacity(queue));
			}

			foreach ((var element, var priority) in itemsToEnqueue)
			{
				TrimAndEnsureCapacity();
				queue.Enqueue(element, priority);
				expectedCount++;
				Assert.Equal(expectedCount, queue.Count);
			}

			while (expectedCount > 0)
			{
				_ = queue.Dequeue();
				TrimAndEnsureCapacity();
				expectedCount--;
				Assert.Equal(expectedCount, queue.Count);
			}

			TrimAndEnsureCapacity();
			Assert.Equal(0, queue.Count);
		}

		private static int GetUnderlyingBufferCapacity<TPriority, TElement>(UpdatablePriorityQueue<TPriority, TElement> queue)
		{
			var nodesField = queue.GetType().GetField("_nodes", BindingFlags.NonPublic | BindingFlags.Instance)!;
			Assert.NotNull(nodesField);
			var nodes = ((TElement Element, TPriority Priority)[])nodesField.GetValue(queue)!;
			return nodes.Length;
		}

		#endregion

		#region Enumeration

		[Theory]
		[MemberData(nameof(GetNonModifyingOperations))]
		public void UpdatablePriorityQueue_Enumeration_ValidOnNonModifyingOperation(Action<UpdatablePriorityQueue<int, int>> nonModifyingOperation, int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity: count, count: count);
			using var enumerator = queue.UnorderedItems.GetEnumerator();
			nonModifyingOperation(queue);
			_ = enumerator.MoveNext();
		}

		[Theory]
		[MemberData(nameof(GetModifyingOperations))]
		public void UpdatablePriorityQueue_Enumeration_InvalidationOnModifyingOperation(Action<UpdatablePriorityQueue<int, int>> modifyingOperation, int count)
		{
			var queue = CreateUpdatablePriorityQueue(initialCapacity: count, count: count);
			using var enumerator = queue.UnorderedItems.GetEnumerator();
			modifyingOperation(queue);
			_ = Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
		}

		public static IEnumerable<object[]> GetModifyingOperations()
		{
			yield return WrapArg(queue => queue.Enqueue(42, 0), 0);
			yield return WrapArg(queue => queue.Dequeue(), 5);
			yield return WrapArg(queue => queue.TryDequeue(out _, out _), 5);
			yield return WrapArg(queue => queue.EnqueueDequeue(5, priority: int.MaxValue), 5);
			yield return WrapArg(queue => queue.EnqueueDequeue(5, priority: int.MaxValue), 5);
			yield return WrapArg(queue => queue.EnqueueRange(new[] { (1, 2) }), 0);
			yield return WrapArg(queue => queue.EnqueueRange(new[] { (1, 2) }), 10);
			yield return WrapArg(queue => queue.EnqueueRange(new[] { 1, 2 }, 42), 0);
			yield return WrapArg(queue => queue.EnqueueRange(new[] { 1, 2 }, 42), 10);
			yield return WrapArg(queue => queue.EnsureCapacity(2 * queue.Count), 4);
			yield return WrapArg(queue => queue.Clear(), 5);
			yield return WrapArg(queue => queue.Clear(), 0);

			static object[] WrapArg(Action<UpdatablePriorityQueue<int, int>> arg, int queueCount) => new object[] { arg, queueCount };
		}

		public static IEnumerable<object[]> GetNonModifyingOperations()
		{
			yield return WrapArg(queue => queue.Peek(), 1);
			yield return WrapArg(queue => queue.TryPeek(out _, out _), 1);
			yield return WrapArg(queue => queue.TryDequeue(out _, out _), 0);
			yield return WrapArg(queue => queue.EnqueueDequeue(5, priority: int.MinValue), 1);
			yield return WrapArg(queue => queue.EnqueueDequeue(5, priority: int.MaxValue), 0);
			yield return WrapArg(queue => queue.EnqueueRange(Array.Empty<(int, int)>()), 5);
			yield return WrapArg(queue => queue.EnqueueRange(Array.Empty<int>(), 42), 5);
			yield return WrapArg(queue => queue.EnsureCapacity(5), 5);

			static object[] WrapArg(Action<UpdatablePriorityQueue<int, int>> arg, int queueCount) => new object[] { arg, queueCount };
		}

		#endregion
	}
}
