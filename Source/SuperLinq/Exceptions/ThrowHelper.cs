using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SuperLinq.Exceptions;

internal static class ThrowHelper
{
	[DoesNotReturn]
	public static void ThrowArgumentException(string param, string message) =>
		throw new ArgumentException(message, param);

	[DoesNotReturn]
	public static T ThrowArgumentException<T>(string param, string message) =>
		throw new ArgumentException(message, param);

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
	public static void ThrowNotSupportedException() =>
		throw new NotSupportedException();

	[DoesNotReturn]
	public static T ThrowNotSupportedException<T>() =>
		throw new NotSupportedException();

	[DoesNotReturn]
	public static void ThrowObjectDisposedException(string type) =>
		throw new ObjectDisposedException(type);

	[DoesNotReturn]
	public static void ThrowUnreachableException() =>
		throw new UnreachableException();

	[DoesNotReturn]
	public static void ThrowTimeoutException(string message, Exception? ex = default) =>
		throw new TimeoutException(message, ex);
}
