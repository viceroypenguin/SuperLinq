﻿namespace Test;

/// <summary>
/// Functions which throw NotImplementedException if they're ever called.
/// </summary>
internal static class BreakingFunc
{
	#region Func Delegates

	internal static Func<TResult> Of<TResult>() =>
		() => throw new TestException();

	internal static Func<T, TResult> Of<T, TResult>() =>
		t => throw new TestException();

	internal static Func<T1, T2, TResult> Of<T1, T2, TResult>() =>
		(t1, t2) => throw new TestException();

	internal static Func<T1, T2, T3, TResult> Of<T1, T2, T3, TResult>() =>
		(t1, t2, t3) => throw new TestException();

	internal static Func<T1, T2, T3, T4, TResult> Of<T1, T2, T3, T4, TResult>() =>
		(t1, t2, t3, t4) => throw new TestException();
	#endregion

	#region Span Delegates
	internal static SuperEnumerable.ReadOnlySpanFunc<T, TResult> OfSpan<T, TResult>() =>
		t => throw new TestException();
	#endregion
}
