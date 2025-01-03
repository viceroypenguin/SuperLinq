namespace SuperLinq.Tests;

/// <summary>
/// Functions which throw NotImplementedException if they're ever called.
/// </summary>
internal static class BreakingReadOnlySpanFunc
{
	internal static SuperEnumerable.ReadOnlySpanFunc<T, TResult> Of<T, TResult>() =>
		t => throw new TestException();
}
