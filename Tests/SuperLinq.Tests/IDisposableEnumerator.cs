namespace SuperLinq.Tests;

/// <summary>
/// Used to identify enumerables that can and should be disposed, such as <see cref="TestingSequence{T}"/>.
/// </summary>
/// <typeparam name="T">Type of elements.</typeparam>
public interface IDisposableEnumerable<T> : IEnumerable<T>, IDisposable { }
