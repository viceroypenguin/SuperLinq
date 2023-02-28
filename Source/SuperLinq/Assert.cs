using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

[ExcludeFromCodeCoverage(Justification = "These cover situations that are theoretically impossible, and so cannot be code-covered.")]
internal static class Assert
{
	public static void NotNull<T>([NotNull, AllowNull] T? obj, [CallerArgumentExpression(nameof(obj))] string? parameter = null)
	{
		if (obj == null)
			ThrowHelper.ThrowInvalidOperationException($"`SuperLinq` bug: '{parameter}' should not be null.");
	}

	public static void True([DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression(nameof(condition))] string? expression = null)
	{
		if (!condition)
			ThrowHelper.ThrowInvalidOperationException($"`SuperLinq` bug: '{expression}' should be `true`.");
	}
}
