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

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfLessThan<TValue>(
			[NotNull, DisallowNull] this TValue value,
			TValue other,
			[CallerArgumentExpression("value")] string? paramName = null)
			where TValue : notnull, IComparable<TValue>
	{
		if (value.CompareTo(other) < 0)
		{
			ThrowOutOfRange(paramName!, $"Value should not be less than {other}.");
		}
	}

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfGreaterThan<TValue>(
			[NotNull, DisallowNull] this TValue value,
			TValue other,
			[CallerArgumentExpression("value")] string? paramName = null)
			where TValue : notnull, IComparable<TValue>
	{
		if (value.CompareTo(other) > 0)
		{
			ThrowOutOfRange(paramName!, $"Value should not be greater than {other}.");
		}
	}

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ThrowIfNotInRange<TValue>(
			[NotNull, DisallowNull] this TValue value,
			TValue min,
			TValue max,
			[CallerArgumentExpression("value")] string? paramName = null)
			where TValue : notnull, IComparable<TValue>
	{
		if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
		{
			ThrowOutOfRange(paramName!, $"Value should be between {min} and {max}.");
		}
	}

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining), DoesNotReturn]
	public static void ThrowOutOfRange<TValue>(
			[NotNull, DisallowNull] this TValue value,
			string? message = null,
			[CallerArgumentExpression("value")] string? paramName = null)
			where TValue : notnull
	{
		ThrowOutOfRange(paramName!, message);
	}

	[DoesNotReturn]
	private static void ThrowOutOfRange(string paramName, string? message) =>
		throw new ArgumentOutOfRangeException(paramName, message);
}
