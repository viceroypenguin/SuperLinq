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
			ThrowHelper.ThrowNotSupportedException();

		public bool Remove(T item) =>
			ThrowHelper.ThrowNotSupportedException<bool>();

		public void Clear() =>
			ThrowHelper.ThrowNotSupportedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		protected abstract IEnumerable<T> GetEnumerable();

		public abstract int Count { get; }

		public virtual IEnumerator<T> GetEnumerator() =>
			GetEnumerable().GetEnumerator();

		public virtual bool Contains(T item) =>
			GetEnumerable().Contains(item);

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			ArgumentNullException.ThrowIfNull(array);
			ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);
			ArgumentOutOfRangeException.ThrowIfGreaterThan(arrayIndex, array.Length - Count);

			_ = SuperEnumerable.CopyTo(GetEnumerable(), array, arrayIndex);
		}
	}
}
