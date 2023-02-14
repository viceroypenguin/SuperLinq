namespace SuperLinq;
#nullable enable
public static partial class SuperEnumerable
{
    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Boolean> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Boolean e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Byte> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Byte e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Char> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Char e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Decimal> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Decimal e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Double> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Double e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Int16> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Int16 e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Int32> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Int32 e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Int64> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Int64 e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.SByte> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.SByte e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.Single> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.Single e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.String> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.String e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.UInt16> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.UInt16 e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.UInt32> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.UInt32 e) => sb.Append(e);
    }

    /// <summary>
    /// Creates a delimited string from a sequence of values and
    /// a given delimiter.
    /// </summary>
    /// <param name = "source">The sequence of items to delimit. Each is converted to a string using the
    /// simple <c>ToString()</c> conversion.</param>
    /// <param name = "delimiter">The delimiter to inject between elements.</param>
    /// <returns>
    /// A string that consists of the elements in <paramref name = "source"/>
    /// delimited by <paramref name = "delimiter"/>. If the source sequence
    /// is empty, the method returns an empty string.
    /// </returns>
    /// <exception cref = "global::System.ArgumentNullException">
    /// <paramref name = "source"/> or <paramref name = "delimiter"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// This operator uses immediate execution and effectively buffers the sequence.
    /// </remarks>
    public static global::System.String ToDelimitedString(this global::System.Collections.Generic.IEnumerable<global::System.UInt64> source, global::System.String delimiter)
    {
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(source);
        global::CommunityToolkit.Diagnostics.Guard.IsNotNull(delimiter);
        return ToDelimitedStringImpl(source, delimiter, Builder);
        static global::System.Text.StringBuilder Builder(global::System.Text.StringBuilder sb, global::System.UInt64 e) => sb.Append(e);
    }
}