﻿using System;
using System.Globalization;

namespace Cephei.Valuables
{
  /// <summary>
  /// This class contains extension methods related to valuables.
  /// </summary>
  public static class ValuableExtensions
  {
    /// <summary>
    /// Decreases the stack size by a defined amount, regardless of the stack size.
    /// </summary>
    /// <typeparam name="T">IStackable type.</typeparam>
    /// <param name="stack">Stackable object.</param>
    /// <param name="amount">Amount to decrease by.</param>
    /// <returns>The stackable's new stack size.</returns>
    public static T DoDecrease<T>(this IStackable<T> stack, T amount)
    {
      stack.CanDecrease(amount, out T val);
      stack.Stack = val;
      return val;
    }

    /// <summary>
    /// Returns a string with the gauge's values.
    /// </summary>
    /// <typeparam name="T">The gauge's value type.</typeparam>
    /// <typeparam name="U">The gauge's percentage type.</typeparam>
    /// <param name="gauge">The gauge.</param>
    /// <returns>A string with the gauge's values.</returns>
    public static string ToGaugeString<T, U>(this IGauge<T, U> gauge)
        => "Value='" + gauge.Value?.ToString() + "' MaxValue='" + gauge.MaxValue?.ToString() + "' Percent='" + gauge.Percentage?.ToString() + "'";
    /// <summary>
    /// Returns a string with the gauge's values, using specific formatters.
    /// </summary>
    /// <typeparam name="T">The gauge's value type.</typeparam>
    /// <typeparam name="U">The gauge's percentage type.</typeparam>
    /// <param name="gauge">The gauge.</param>
    /// <param name="pformat">Format for the percentage.</param>
    /// <param name="nformat">Format for the other values.</param>
    /// <returns>A formatted string with the gauge's values.</returns>
    public static string ToGaugeString<T, U>(this IGauge<T, U> gauge, string pformat, string nformat = "G") 
      where T : IFormattable
      where U : IFormattable
        => gauge.ToGaugeString(pformat, nformat, CultureInfo.CurrentCulture.NumberFormat);
    /// <summary>
    /// Returns a string with the gauge's values, using specific formatters.
    /// </summary>
    /// <typeparam name="T">The gauge's value type.</typeparam>
    /// <typeparam name="U">The gauge's percentage type.</typeparam>
    /// <param name="gauge">The gauge.</param>
    /// <param name="pformat">Format for the percentage.</param>
    /// <param name="nformat">Format for the other values.</param>
    /// <param name="formatter">The format provider.</param>
    /// <returns>A formatted string with the gauge's values.</returns>
    public static string ToGaugeString<T, U>(this IGauge<T, U> gauge, string pformat, string nformat, IFormatProvider formatter) 
      where T : IFormattable
      where U : IFormattable
        => "Value='" + gauge.Value.ToString(nformat, formatter) + "' MaxValue='" + gauge.MaxValue.ToString(nformat, formatter)
        + "' Percent='" + gauge.Percentage.ToString(pformat, formatter) + "'";
  }
}
