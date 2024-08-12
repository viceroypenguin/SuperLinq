// Credit: Mark Cilia Vincenti, 2024
// Taken from: https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock
// NuGet package: https://www.nuget.org/packages/Backport.System.Threading.Lock

using System.Runtime.CompilerServices;
#if NET9_0_OR_GREATER
#pragma warning disable IDE0001
[assembly: TypeForwardedTo(typeof(System.Threading.Lock))]
#pragma warning restore IDE0001
#else
namespace System.Threading;
#pragma warning disable RS0016
#pragma warning disable CA2002
#pragma warning disable CA1034
#pragma warning disable MA0154
/// <summary>
/// A backport of .NET 9.0+'s System.Threading.Lock.
/// </summary>
public sealed class Lock
{
#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
	/// <summary>
	/// <inheritdoc cref="Monitor.Enter(object)"/>
	/// </summary>
	/// <exception cref="ArgumentNullException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Enter() => Monitor.Enter(this);

	/// <summary>
	/// <inheritdoc cref="Monitor.TryEnter(object)"/>
	/// </summary>
	/// <returns>
	/// <inheritdoc cref="Monitor.TryEnter(object)"/>
	/// </returns>
	/// <exception cref="ArgumentNullException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryEnter() => Monitor.TryEnter(this);

	/// <summary>
	/// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
	/// </summary>
	/// <returns>
	/// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
	/// </returns>
	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="ArgumentOutOfRangeException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

	/// <summary>
	/// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
	/// </summary>
	/// <returns>
	/// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
	/// </returns>
	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="ArgumentOutOfRangeException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryEnter(int millisecondsTimeout) => Monitor.TryEnter(this, millisecondsTimeout);

	/// <summary>
	/// <inheritdoc cref="Monitor.Exit(object)"/>
	/// </summary>
	/// <exception cref="ArgumentNullException"/>
	/// <exception cref="SynchronizationLockException"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Exit() => Monitor.Exit(this);

	/// <summary>
	/// <inheritdoc cref="Monitor.IsEntered(object)"/>
	/// </summary>
	/// <returns>
	/// <inheritdoc cref="Monitor.IsEntered(object)"/>
	/// </returns>
	/// <exception cref="ArgumentNullException"/>
	public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

	/// <summary>
	/// Enters the lock and returns a <see cref="Scope"/> that may be disposed to exit the lock. Once the method returns,
	/// the calling thread would be the only thread that holds the lock. This method is intended to be used along with a
	/// language construct that would automatically dispose the <see cref="Scope"/>, such as with the C# <code>using</code>
	/// statement.
	/// </summary>
	/// <returns>
	/// A <see cref="Scope"/> that may be disposed to exit the lock.
	/// </returns>
	/// <remarks>
	/// If the lock cannot be entered immediately, the calling thread waits for the lock to be exited. If the lock is
	/// already held by the calling thread, the lock is entered again. The calling thread should exit the lock, such as by
	/// disposing the returned <see cref="Scope"/>, as many times as it had entered the lock to fully exit the lock and
	/// allow other threads to enter the lock.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Scope EnterScope()
	{
		Enter();
		return new Scope(this);
	}

	/// <summary>
	/// A disposable structure that is returned by <see cref="EnterScope()"/>, which when disposed, exits the lock.
	/// </summary>
	public ref struct Scope(Lock @lock)
	{
		/// <summary>
		/// Exits the lock.
		/// </summary>
		/// <exception cref="SynchronizationLockException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void Dispose() => @lock.Exit();
	}
}
#pragma warning restore MA0154
#pragma warning restore CA1034
#pragma warning restore CA2002
#pragma warning restore RS0016
#endif
