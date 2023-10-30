using System.Diagnostics.CodeAnalysis;

namespace SuperLinq;

internal static class ThrowHelper
{
	[DoesNotReturn]
	public static void ThrowArgumentException(string param, string message) =>
		throw new ArgumentException(param, message);

	[DoesNotReturn]
	public static T ThrowArgumentException<T>(string param, string message) =>
		throw new ArgumentException(param, message);

	[DoesNotReturn]
	public static void ThrowArgumentOutOfRangeException(string param) =>
		throw new System.ArgumentOutOfRangeException(param);

	[DoesNotReturn]
	public static T ThrowArgumentOutOfRangeException<T>(string param) =>
		throw new System.ArgumentOutOfRangeException(param);

	[DoesNotReturn]
	public static void ThrowArgumentOutOfRangeException(string param, string message) =>
		throw new System.ArgumentOutOfRangeException(param, message);

	[DoesNotReturn]
	public static T ThrowArgumentOutOfRangeException<T>(string param, string message) =>
		throw new System.ArgumentOutOfRangeException(param, message);

	[DoesNotReturn]
	public static void ThrowInvalidOperationException(string message) =>
		throw new InvalidOperationException(message);

	[DoesNotReturn]
	public static T ThrowInvalidOperationException<T>(string message) =>
		throw new InvalidOperationException(message);

	[DoesNotReturn]
	public static void ThrowObjectDisposedException(string type) =>
		throw new ObjectDisposedException(type);
}
