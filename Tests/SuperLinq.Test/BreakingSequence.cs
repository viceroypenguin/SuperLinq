﻿using System.Collections;

namespace Test;

/// <summary>
/// Enumerable sequence which throws InvalidOperationException as soon as its
/// enumerator is requested. Used to check lazy evaluation.
/// </summary>
internal class BreakingSequence<T> : IEnumerable<T>
{
	public IEnumerator<T> GetEnumerator() => throw new TestException();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
