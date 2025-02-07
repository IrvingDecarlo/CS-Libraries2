using System;

namespace Cephei.Numericals
{
  /// <summary>
  /// The NumericalExtensions class contains extension methods for numerical functions.
  /// </summary>
  public static class NumericalExtensions
  {
    /// <summary>
    /// Gets the number of non-decimal digits a double has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    public static double GetNumberOfDigits(this double val) => val == 0 ? 1 : Math.Abs(Math.Floor(Math.Log10(val) + 1));
  }
}
