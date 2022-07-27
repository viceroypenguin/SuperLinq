namespace Test;

internal static class EqualityComparer
{
	public static IEqualityComparer<T> Create<T>(Func<T, T, bool> comparer) =>
		new DelegatingComparer<T>(comparer);

	public static IEqualityComparer<T> Create<T>(Func<T, T, bool> comparer, Func<T, int> hasher) =>
		new DelegatingComparer<T>(comparer, hasher);

	private sealed class DelegatingComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _comparer;
		private readonly Func<T, int> _hasher;

		public DelegatingComparer(Func<T, T, bool> comparer)
			: this(comparer, x => x == null ? 0 : x.GetHashCode()) { }

		public DelegatingComparer(Func<T, T, bool> comparer, Func<T, int> hasher)
		{
			_comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
			_hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
		}

		public bool Equals(T? x, T? y) => _comparer(x!, y!);
		public int GetHashCode(T obj) => _hasher(obj);
	}
}
