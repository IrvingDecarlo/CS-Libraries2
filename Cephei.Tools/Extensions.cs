using System;

namespace Cephei.Tools
{
  /// <summary>
  /// Tools.Extensions serve only to extend .NET classes.
  /// </summary>
  public static class Extensions
  {
    // PUBLIC

    /// <summary>
    /// Extracts a collection of booleans out of an integer.
    /// </summary>
    /// <param name="compressed">Integer to extract the booleans out of.</param>
    /// <param name="n">Number of booleans to extract.</param>
    public static bool[] ToBools(this int compressed, byte n)
    {
      bool[] bools = new bool[n];
      compressed.ToBools(bools);
      return bools;
    }
    /// <summary>
    /// Extracts a collection of booleans out of an integer.
    /// </summary>
    /// <param name="compressed">Integer to extract the booleans out of.</param>
    /// <param name="bools">Collection of booleans.</param>
    public static void ToBools(this int compressed, Span<bool> bools)
    {
      for (byte i = 0; i < bools.Length; i++) bools[i] = compressed.ToBool(i);
    }
    /// <summary>
    /// Converts a span of objects into a span of booleans, using a model object as "true".
    /// </summary>
    /// <typeparam name="T">Object type to convert.</typeparam>
    /// <param name="span">Span of objects to convert.</param>
    /// <param name="tru">Model object to be used as "true".</param>
    public static bool[] ToBools<T>(this ReadOnlySpan<T> span, T tru)
    {
      bool[] bools = new bool[span.Length];
      span.ToBools(bools, tru);
      return bools;
    }
    /// <summary>
    /// Converts a span of objects into a span of booleans, using a model object as "true".
    /// </summary>
    /// <typeparam name="T">Object type to convert.</typeparam>
    /// <param name="span">Span of objects to convert.</param>
    /// <param name="bools">Span of booleans to receive the new value.</param>
    /// <param name="tru">Model object to be used as "true".</param>
    public static void ToBools<T>(this ReadOnlySpan<T> span, Span<bool> bools, T tru)
    {
      for (int i = 0; i < span.Length; i++) bools[i] = span[i].SafeEquals(tru);
    }

    /// <summary>
    /// Gets the number of years in a time span.
    /// </summary>
    /// <param name="time">Time span to get the number of years from.</param>
    /// <returns>The number of years in a time span.</returns>
    /// <remarks>Is not accurate since leap years are not taken into account. Use <see cref="GetYears(DateTime, DateTime)"/> instead for more precision.</remarks>
    public static int GetYears(this TimeSpan time) => time.Days / 365;
    /// <summary>
    /// Gets the number of years between two dates.
    /// </summary>
    /// <param name="date">Reference date. Should always be greater than the comparison date.</param>
    /// <param name="other">Date to compare with.</param>
    /// <returns>The number of years between the two dates.</returns>
    public static int GetYears(this DateTime date, DateTime other)
    {
      int y = date.Year - other.Year;
      if (date.Month < other.Month || date.Month == other.Month && date.Day < other.Day) y--;
      return y;
    }

    /// <summary>
    /// Compresses a span of booleans into a single int. The booleans are compressed to fit 32 of them within an Int32 - use binary
    /// operators to extract the boolean out of the integer.
    /// </summary>
    /// <param name="span">Span of booleans to convert.</param>
    public static int ToCompressedBool(this ReadOnlySpan<bool> span) => span.ToCompressedBool(true);
    /// <summary>
    /// Converts a span of objects into a span of booleans, using a model object as "true". The booleans are compressed to fit 32 of them within an Int32 - use binary
    /// operators to extract the boolean out of the integer.
    /// </summary>
    /// <typeparam name="T">Object type to convert.</typeparam>
    /// <param name="span">Span of objects to convert.</param>
    /// <param name="tru">Model object to be used as "true".</param>
    public static int ToCompressedBool<T>(this ReadOnlySpan<T> span, T tru)
    {
      int b = 0;
      for (int i = 0; i < span.Length; i++)
      {
        if (span[i].SafeEquals(tru)) b += 1 << i;
      }
      return b;
    }

    /// <summary>
    /// A safe version of Equals where both objects are checked if they are null.
    /// </summary>
    /// <param name="this">Object that will equate to the other.</param>
    /// <param name="other">Object that will be equated by the first.</param>
    /// <returns>True if both are null or if both are equals.</returns>
    public static bool SafeEquals(this object? @this, object? other)
    {
      if (@this is null && other is null) return true;
      if (@this is null || other is null) return false;
      return @this.Equals(other);
    }

    /// <summary>
    /// Extracts a boolean out of an integer, using a binary operator.
    /// </summary>
    /// <param name="nt">Integer to get the boolean.</param>
    /// <param name="index">Index to get the bool out of.</param>
    /// <returns>True if the bit in the specified int's index is 1.</returns>
    public static bool ToBool(this int nt, byte index) => (nt & (1 << index)) != 0;
  }
}
