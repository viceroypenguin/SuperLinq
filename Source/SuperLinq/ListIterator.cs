using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public partial class SuperEnumerable
{
	[ExcludeFromCodeCoverage]
	private abstract class ListIterator<T> : CollectionIterator<T>, IList<T>, IReadOnlyList<T>
	{
		public void Insert(int index, T item) =>
			ThrowHelper.ThrowNotSupportedException();

		public void RemoveAt(int index) =>
			ThrowHelper.ThrowNotSupportedException();

		public T this[int index]
		{
			get => ElementAt(index);
			set => ThrowHelper.ThrowNotSupportedException();
		}

		protected abstract T ElementAt(int index);

		public virtual int IndexOf(T item) =>
			GetEnumerable().IndexOf(item);
	}
}
