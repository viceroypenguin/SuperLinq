namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
{
    /// <summary>
    /// Returns the result of applying a function to a sequence of 1 element.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 1 element.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(1 + 1).ToList();
        if (elements.Count != 1)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 1, Actual: {elements.Count})");
        return folder(elements[0]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 2 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 2 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(2 + 1).ToList();
        if (elements.Count != 2)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 2, Actual: {elements.Count})");
        return folder(elements[0], elements[1]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 3 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 3 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(3 + 1).ToList();
        if (elements.Count != 3)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 3, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 4 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 4 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(4 + 1).ToList();
        if (elements.Count != 4)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 4, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 5 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 5 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(5 + 1).ToList();
        if (elements.Count != 5)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 5, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 6 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 6 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(6 + 1).ToList();
        if (elements.Count != 6)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 6, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 7 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 7 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(7 + 1).ToList();
        if (elements.Count != 7)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 7, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 8 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 8 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(8 + 1).ToList();
        if (elements.Count != 8)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 8, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 9 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 9 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(9 + 1).ToList();
        if (elements.Count != 9)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 9, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 10 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 10 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(10 + 1).ToList();
        if (elements.Count != 10)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 10, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 11 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 11 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(11 + 1).ToList();
        if (elements.Count != 11)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 11, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 12 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 12 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(12 + 1).ToList();
        if (elements.Count != 12)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 12, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 13 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 13 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(13 + 1).ToList();
        if (elements.Count != 13)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 13, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 14 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 14 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(14 + 1).ToList();
        if (elements.Count != 14)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 14, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 15 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 15 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(15 + 1).ToList();
        if (elements.Count != 15)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 15, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14]);
    }

    /// <summary>
    /// Returns the result of applying a function to a sequence of 16 elements.
    /// </summary>
    /// <remarks>
    /// This operator uses immediate execution and buffers as many items of the source sequence as necessary.
    /// </remarks>
    /// <typeparam name = "T">Type of element in the source sequence</typeparam>
    /// <typeparam name = "TResult">Type of the result</typeparam>
    /// <param name = "source">The sequence of items to fold.</param>
    /// <param name = "folder">Function to apply to the elements in the sequence.</param>
    /// <returns>The folded value returned by <paramref name = "folder"/>.</returns>
    /// <exception cref = "global::System.ArgumentNullException"><paramref name = "source"/> or <paramref name = "folder"/> is null.</exception>
    /// <exception cref = "global::System.InvalidOperationException">
    /// <paramref name = "source"/> does not contain exactly 16 elements.
    /// </exception>
    public static TResult Fold<T, TResult>(this global::System.Collections.Generic.IEnumerable<T> source, global::System.Func<T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, T, TResult> folder)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(folder);
        var elements = source.Take(16 + 1).ToList();
        if (elements.Count != 16)
            global::CommunityToolkit.Diagnostics.ThrowHelper.ThrowInvalidOperationException($"Sequence contained an incorrect number of elements. (Expected: 16, Actual: {elements.Count})");
        return folder(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5], elements[6], elements[7], elements[8], elements[9], elements[10], elements[11], elements[12], elements[13], elements[14], elements[15]);
    }
}