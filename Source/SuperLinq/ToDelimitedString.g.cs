using System.Text;

namespace SuperLinq;

public static partial class SuperEnumerable
{
    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<bool> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Boolean);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, bool, StringBuilder> Boolean = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<byte> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Byte);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, byte, StringBuilder> Byte = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<char> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Char);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, char, StringBuilder> Char = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<decimal> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Decimal);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, decimal, StringBuilder> Decimal = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<double> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Double);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, double, StringBuilder> Double = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<float> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Single);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, float, StringBuilder> Single = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<int> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int32);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, int, StringBuilder> Int32 = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<long> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int64);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, long, StringBuilder> Int64 = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<sbyte> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.SByte);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, sbyte, StringBuilder> SByte = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<short> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.Int16);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, short, StringBuilder> Int16 = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<string> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.String);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, string, StringBuilder> String = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<uint> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt32);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, uint, StringBuilder> UInt32 = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<ulong> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt64);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, ulong, StringBuilder> UInt64 = (sb, e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name="source">The sequence of items to delimit. Each is converted to a string using the
    /// simple ToString() conversion.</param>
    /// <param name="delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name="source"/>
    /// delimited by <paramref name="delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="delimiter"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static string ToDelimitedString(this IEnumerable<ushort> source, string delimiter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
        return ToDelimitedStringImpl(source, delimiter, StringBuilderAppenders.UInt16);
    }

    static partial class StringBuilderAppenders
    {
        public static readonly Func<StringBuilder, ushort, StringBuilder> UInt16 = (sb, e) => sb.Append(e);
    }


    static partial class StringBuilderAppenders {}
}
