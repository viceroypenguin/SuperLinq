#if !NET6_0_OR_GREATER
global using ArgumentNullException = SuperLinq.Exceptions.ArgumentNullException;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Exceptions;

#pragma warning disable RS0016
#pragma warning disable CA1711
#pragma warning disable CS1591

[Browsable(false)]
internal static class ArgumentNullException
{
	/// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
	/// <param name="argument">The reference type argument to validate as non-null.</param>
	/// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
	public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null)
		{
			Throw(paramName);
		}
	}

	[DoesNotReturn]
	private static void Throw(string? paramName) =>
		throw new System.ArgumentNullException(paramName);
}
#endif
