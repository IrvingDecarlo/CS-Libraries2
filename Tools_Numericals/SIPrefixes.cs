using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cephei.Numericals
{
  /// <summary>
  /// The SIPrefixes class is used to format numbers to use the International System of Units prefixes (k, M, G...).
  /// </summary>
  public static class SIPrefixes
  {
    static SIPrefixes() => Prefixes = new Dictionary<string, sbyte>()
        {
            { "Y", 24 },
            { "Z", 21 },
            { "E", 18 },
            { "P", 15 },
            { "T", 12 },
            { "G", 9 },
            { "M", 6 },
            { "k", 3 },
            { "h", 2 },
            { "de", 1 },
            { "", 0 },
            { "d", -1 },
            { "c", -2 },
            { "m", -3 },
            { "u", -6 },
            { "n", -9 },
            { "p", -12 },
            { "f", -15 },
            { "a", -18 },
            { "z", -21 },
            { "y", -24 }
        };

    //
    // PUBLIC
    //

    // VARIABLES

    /// <summary>
    /// The dictionary of prefixes that is used. Keys are the prefixes and values are its base 10. (By default, micro is represented as an "u").
    /// An empty string will equal to no prefix (thus, no numeric adjustments will be made).
    /// </summary>
    public static readonly Dictionary<string, sbyte> Prefixes;

    // METHODS

    /// <summary>
    /// Returns a number to string, using a prefix and using the culture's number format provider.
    /// </summary>
    /// <param name="d">The number.</param>
    /// <param name="pref">The prefix to be used.</param>
    /// <returns>The number to string.</returns>
    public static string ToString(double d, string pref) => ToString(d, pref, "G", CultureInfo.CurrentCulture.NumberFormat);
    /// <summary>
    /// Returns a number to a string using a prefix and specific formatting.
    /// </summary>
    /// <param name="d">The number.</param>
    /// <param name="pref">The prefix to use.</param>
    /// <param name="format">The number's format.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The number to string.</returns>
    public static string ToString(double d, string pref, string format, IFormatProvider provider)
        => UsePrefix(d, pref).ToString(format, provider) + pref;

    /// <summary>
    /// Uses a prefix on a number.
    /// </summary>
    /// <param name="d">The number.</param>
    /// <param name="pref">The prefix to use.</param>
    /// <returns>The number adjusted to the prefix.</returns>
    public static double UsePrefix(double d, string pref) => d / Math.Pow(10, Prefixes[pref]);
  }
}
