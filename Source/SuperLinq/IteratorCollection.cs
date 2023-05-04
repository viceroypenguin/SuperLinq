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

		public abstract int Count { get; }
		public abstract IEnumerator<TResult> GetEnumerator();
		public abstract bool Contains(TResult item);
		public abstract void CopyTo(TResult[] array, int arrayIndex);
	}
}
