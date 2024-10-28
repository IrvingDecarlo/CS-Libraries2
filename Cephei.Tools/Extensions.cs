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
    /// Gets the number of non-decimal digits a double has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    public static double GetNumberOfDigits(this double val) => val == 0 ? 1 : Math.Abs(Math.Floor(Math.Log10(val) + 1));

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
  }
}
