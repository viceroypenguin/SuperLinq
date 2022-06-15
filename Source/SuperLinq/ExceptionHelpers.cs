// © 2022 Amichai Mantinband MIT License

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace SuperLinq;

internal static class ExceptionHelpers
{
	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNull<TValue>(
			[NotNull, AllowNull] this TValue? value,
			[CallerArgumentExpression("value")] string? paramName = null)
			where TValue : notnull
	{
		if (value == null)
		{
			ThrowNull(paramName!);
		}
	}

	[DoesNotReturn]
	private static void ThrowNull(string paramName) =>
		throw new ArgumentNullException(paramName);
}
