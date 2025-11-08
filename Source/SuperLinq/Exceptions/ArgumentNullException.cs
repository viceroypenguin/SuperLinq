#if !NET6_0_OR_GREATER
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SuperLinq.Exceptions;

[Browsable(false)]
[ExcludeFromCodeCoverage]
internal static class ArgumentNullExceptionExtension
{
	extension(ArgumentNullException)
	{
		/// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
		/// <param name="argument">The reference type argument to validate as non-null.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
		public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
		{
			if (argument is null)
				Throw(paramName);
		}
	}

	[DoesNotReturn]
	private static void Throw(string? paramName) =>
		throw new ArgumentNullException(paramName);
}
#endif
