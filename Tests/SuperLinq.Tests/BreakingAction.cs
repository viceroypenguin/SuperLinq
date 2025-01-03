namespace SuperLinq.Tests;

/// <summary>
/// Functions which throw NotImplementedException if they're ever called.
/// </summary>
internal static class BreakingAction
{
	internal static Action Of() =>
		() => throw new TestException();

	internal static Action<T> Of<T>() =>
		t => throw new TestException();

	internal static Action<T1, T2> Of<T1, T2>() =>
		(t1, t2) => throw new TestException();

	internal static Action<T1, T2, T3> Of<T1, T2, T3>() =>
		(t1, t2, t3) => throw new TestException();

	internal static Action<T1, T2, T3, T4> Of<T1, T2, T3, T4>() =>
		(t1, t2, t3, t4) => throw new TestException();
}
