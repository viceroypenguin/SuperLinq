using System.Collections;

namespace Test.Async;

/// <summary>
/// Enumerable sequence which throws InvalidOperationException as soon as its
/// enumerator is requested. Used to check lazy evaluation.
/// </summary>
class AsyncBreakingSequence<T> : IAsyncEnumerable<T>
{
	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) 
		=> throw new NotSupportedException();
}
