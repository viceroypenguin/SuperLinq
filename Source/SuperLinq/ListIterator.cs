using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public partial class SuperEnumerable
{
	[ExcludeFromCodeCoverage]
	private abstract class ListIterator<T> : CollectionIterator<T>, IList<T>, IReadOnlyList<T>
	{
		public void Insert(int index, T item) =>
			throw new NotSupportedException();
		public void RemoveAt(int index) =>
			throw new NotSupportedException();

		public T this[int index]
		{
			get => ElementAt(index);
			set => throw new NotSupportedException();
		}

		protected abstract T ElementAt(int index);
		public virtual int IndexOf(T item) =>
			GetEnumerable().IndexOf(item);
	}
}
