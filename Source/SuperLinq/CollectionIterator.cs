using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public partial class SuperEnumerable
{
	[ExcludeFromCodeCoverage]
	private abstract class CollectionIterator<T> : ICollection<T>, IReadOnlyCollection<T>
	{
		public bool IsReadOnly => true;
		public void Add(T item) =>
			throw new NotSupportedException();
		public bool Remove(T item) =>
			throw new NotSupportedException();
		public void Clear() =>
			throw new NotSupportedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		protected abstract IEnumerable<T> GetEnumerable();

		public abstract int Count { get; }

		public virtual IEnumerator<T> GetEnumerator() =>
			GetEnumerable().GetEnumerator();

		public virtual bool Contains(T item) =>
			GetEnumerable().Contains(item);

		public virtual void CopyTo(T[] array, int arrayIndex) =>
			GetEnumerable().CopyTo(array, arrayIndex);
	}
}
