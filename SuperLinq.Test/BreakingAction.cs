namespace SuperLinq.Test;

/// <summary>
/// Actions which throw NotImplementedException if they're ever called.
/// </summary>
static class BreakingAction
{
	internal static Action WithoutArguments =>
		() => throw new NotImplementedException();

	internal static Action<T> Of<T>() =>
		t => throw new NotImplementedException();

	internal static Action<T1, T2> Of<T1, T2>() =>
		(t1, t2) => throw new NotImplementedException();

	internal static Action<T1, T2, T3> Of<T1, T2, T3>() =>
		(t1, t2, t3) => throw new NotImplementedException();

	internal static Action<T1, T2, T3, T4> Of<T1, T2, T3, T4>() =>
		(t1, t2, t3, t4) => throw new NotImplementedException();
}
