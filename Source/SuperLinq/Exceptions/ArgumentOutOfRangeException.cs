#if !NET8_0_OR_GREATER
global using ArgumentOutOfRangeException = SuperLinq.Exceptions.ArgumentOutOfRangeException;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Exceptions;

#pragma warning disable RS0016
#pragma warning disable CA1711
#pragma warning disable CS1591

[Browsable(false)]
public static class ArgumentOutOfRangeException
{
	[DoesNotReturn]
	private static void ThrowZero(long value, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be not be zero.");

	[DoesNotReturn]
	private static void ThrowNegative(long value, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be a non-negative value.");

	[DoesNotReturn]
	private static void ThrowNegativeOrZero(long value, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be a non-negative and non-zero value.");

	[DoesNotReturn]
	private static void ThrowGreater(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be less than or equal to '{other}'.");

	[DoesNotReturn]
	private static void ThrowGreaterEqual(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be less than '{other}'.");

	[DoesNotReturn]
	private static void ThrowLess(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be greater than or equal to '{other}'.");

	[DoesNotReturn]
	private static void ThrowLessEqual(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be greater than '{other}'.");

	[DoesNotReturn]
	private static void ThrowEqual(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must not be equal to '{other}'.");

	[DoesNotReturn]
	private static void ThrowNotEqual(long value, int other, string? paramName) =>
		throw new System.ArgumentOutOfRangeException(paramName, value, $"{paramName} ('{value}') must be equal to '{other}'.");

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.</summary>
	/// <param name="value">The argument to validate as non-zero.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value == 0)
			ThrowZero(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
	/// <param name="value">The argument to validate as non-negative.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNegative(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value < 0)
			ThrowNegative(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
	/// <param name="value">The argument to validate as non-zero or non-negative.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNegativeOrZero(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value <= 0)
			ThrowNegativeOrZero(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfEqual(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value == other)
			ThrowEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as equal to <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNotEqual(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value != other)
			ThrowNotEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as less or equal than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfGreaterThan(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value > other)
			ThrowGreater(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as less than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfGreaterThanOrEqual(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value >= other)
			ThrowGreaterEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as greatar than or equal than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfLessThan(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value < other)
			ThrowLess(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as greatar than than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfLessThanOrEqual(int value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value <= other)
			ThrowLessEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.</summary>
	/// <param name="value">The argument to validate as non-zero.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfZero(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value == 0)
			ThrowZero(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.</summary>
	/// <param name="value">The argument to validate as non-negative.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNegative(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value < 0)
			ThrowNegative(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero.</summary>
	/// <param name="value">The argument to validate as non-zero or non-negative.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNegativeOrZero(long value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value <= 0)
			ThrowNegativeOrZero(value, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfEqual(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value == other)
			ThrowEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as equal to <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfNotEqual(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value != other)
			ThrowNotEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as less or equal than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfGreaterThan(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value > other)
			ThrowGreater(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as less than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfGreaterThanOrEqual(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value >= other)
			ThrowGreaterEqual(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as greatar than or equal than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfLessThan(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value < other)
			ThrowLess(value, other, paramName);
	}

	/// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal <paramref name="other"/>.</summary>
	/// <param name="value">The argument to validate as greatar than than <paramref name="other"/>.</param>
	/// <param name="other">The value to compare with <paramref name="value"/>.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
	public static void ThrowIfLessThanOrEqual(long value, int other, [CallerArgumentExpression(nameof(value))] string? paramName = null)
	{
		if (value <= other)
			ThrowLessEqual(value, other, paramName);
	}
}
#endif
