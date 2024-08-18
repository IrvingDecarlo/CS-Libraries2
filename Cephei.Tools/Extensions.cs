using System;

namespace Cephei.Tools
{
  /// <summary>
  /// Tools.Extensions serve only to extend .NET classes.
  /// </summary>
  public static class Extensions
  {
    //
    // PUBLIC
    //

    /// <summary>
    /// Gets the number of non-decimal digits a double has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    public static double GetNumberOfDigits(this double val) => val == 0 ? 1 : Math.Abs(Math.Floor(Math.Log10(val) + 1));
    /// <summary>
    /// Gets the number of digits a convertible an int has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    /// <remarks>This is double.GetNumberOfDigits but with the appropriate casts. Use the double's version if the value was already casted or if
    /// a double with the same value exists.</remarks>
    public static int GetNumberOfDigits(this int val) => (int)((double)val).GetNumberOfDigits();

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
