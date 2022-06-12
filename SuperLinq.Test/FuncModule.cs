namespace SuperLinq.Test;

using System;

// This type is designed to be imported statically.
//
// Its members enable replacing explicit instantiations of `Func<...>`,
// as in:
//
//     new Func<string, object, string>((a, b) => a + b)
//
// with the shorter version:
//
//     Func((string a, object b) => a + b)
//
// The `new` is no longer required and the return type can be omitted
// as it can be inferred through the type of the lambda expression.

static class FuncModule
{
	public static Func<T, TResult> Func<T, TResult>(Func<T, TResult> f) => f;
	public static Func<T1, T2, TResult> Func<T1, T2, TResult>(Func<T1, T2, TResult> f) => f;
	public static Func<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f) => f;
	public static Func<T1, T2, T3, T4, TResult> Func<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> f) => f;
}
