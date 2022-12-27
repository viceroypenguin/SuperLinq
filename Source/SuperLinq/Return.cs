using System.Collections;

namespace SuperLinq;

public partial class SuperEnumerable
{
	/// <summary>
	/// Returns a single-element sequence containing the item provided.
	/// </summary>
	/// <typeparam name="T">The type of the item.</typeparam>
	/// <param name="item">The item to return in a sequence.</param>
	/// <returns>A sequence containing only <paramref name="item"/>.</returns>
	public static IEnumerable<T> Return<T>(T item) =>
		new SingleElementList<T>(item);

	private sealed class SingleElementList<T> : IList<T>, IReadOnlyList<T>
	{
		private readonly T _item;

		public SingleElementList(T item) => _item = item;

		public int Count => 1;
		public bool IsReadOnly => true;

		public T this[int index]
		{
			get => index == 0 ? _item : ThrowHelper.ThrowArgumentOutOfRangeException<T>(nameof(index));
			set => throw ReadOnlyException();
		}

		public IEnumerator<T> GetEnumerator() { yield return _item; }
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(T item) => Contains(item) ? 0 : -1;
		public bool Contains(T item) => EqualityComparer<T>.Default.Equals(_item, item);

		public void CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = _item;

		// Following methods are unsupported as this is a read-only list.

		public void Add(T item) => throw ReadOnlyException();
		public void Clear() => throw ReadOnlyException();
		public bool Remove(T item) => throw ReadOnlyException();
		public void Insert(int index, T item) => throw ReadOnlyException();
		public void RemoveAt(int index) => throw ReadOnlyException();

		private static NotSupportedException ReadOnlyException() =>
			new("Single element list is immutable.");
	}
}
