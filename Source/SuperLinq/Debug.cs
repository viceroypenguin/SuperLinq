using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SuperLinq;

internal static class Debug
{
	[Conditional("DEBUG")]
	public static void Assert(
		[DoesNotReturnIf(false)] bool condition,
		[CallerArgumentExpression(nameof(condition))] string? message = null) =>
		System.Diagnostics.Debug.Assert(condition, message);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T AssertNotNull<T>(T? obj)
	{
		Assert(obj is not null);
		return obj;
	}
}
