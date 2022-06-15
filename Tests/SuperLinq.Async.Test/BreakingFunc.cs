namespace Test.Async;

/// <summary>
/// Functions which throw NotImplementedException if they're ever called.
/// </summary>
static class BreakingAsyncFunc
{
	internal static Func<Task<TResult>> Of<TResult>() =>
		() => throw new NotSupportedException();

	internal static Func<T, Task<TResult>> Of<T, TResult>() =>
		t => throw new NotSupportedException();

	internal static Func<T1, T2, Task<TResult>> Of<T1, T2, TResult>() =>
		(t1, t2) => throw new NotSupportedException();

	internal static Func<T1, T2, T3, Task<TResult>> Of<T1, T2, T3, TResult>() =>
		(t1, t2, t3) => throw new NotSupportedException();

	internal static Func<T1, T2, T3, T4, Task<TResult>> Of<T1, T2, T3, T4, TResult>() =>
		(t1, t2, t3, t4) => throw new NotSupportedException();
}
