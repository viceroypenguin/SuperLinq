using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

public partial class SuperEnumerable
{
	[ExcludeFromCodeCoverage]
	private abstract class IteratorCollection<TSource, TResult> : ICollection<TResult>, IReadOnlyCollection<TResult>
	{
		public bool IsReadOnly => true;
		public void Add(TResult item) =>
			throw new NotSupportedException();
		public bool Remove(TResult item) =>
			throw new NotSupportedException();
		public void Clear() =>
			throw new NotSupportedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		protected abstract IEnumerable<TResult> GetEnumerable();

		public abstract int Count { get; }

		public virtual IEnumerator<TResult> GetEnumerator() =>
			GetEnumerable().GetEnumerator();

		public virtual bool Contains(TResult item) =>
			GetEnumerable().Contains(item);

		public virtual void CopyTo(TResult[] array, int arrayIndex) =>
			GetEnumerable().CopyTo(array, arrayIndex);
	}
}
