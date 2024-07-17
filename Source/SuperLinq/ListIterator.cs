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

#if !NO_INDEX
		public virtual int IndexOf(T item) =>
			GetEnumerable().IndexOf(item);
#else
		public virtual int IndexOf(T item)
		{
			var index = 0;
			foreach (var i in this)
			{
				if (EqualityComparer<T>.Default.Equals(item, i))
					return index;
				index++;
			}

			return -1;
		}
#endif
	}
}
