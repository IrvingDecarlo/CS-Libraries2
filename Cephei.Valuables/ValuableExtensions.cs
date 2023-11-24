using System;
using System.Globalization;

namespace Cephei.Valuables
{
    /// <summary>
    /// This class contains extension methods related to valuables.
    /// </summary>
    public static class ValuableExtensions
    {
        /// <summary>
        /// Returns a string with the gauge's values.
        /// </summary>
        /// <typeparam name="T">Any type.</typeparam>
        /// <param name="gauge">The gauge.</param>
        /// <returns>A string with the gauge's values.</returns>
        public static string ToGaugeString<T>(this IGauge<T> gauge) 
            => "Value='" + gauge.Value?.ToString() + "' MaxValue='" + gauge.MaxValue?.ToString() + "' Percent='" + gauge.Percentage?.ToString() + "'";
        /// <summary>
        /// Returns a string with the gauge's values, using specific formatters.
        /// </summary>
        /// <typeparam name="T">Any IFormattable object.</typeparam>
        /// <param name="gauge">The gauge.</param>
        /// <param name="pformat">Format for the percentage.</param>
        /// <param name="nformat">Format for the other values.</param>
        /// <returns>A formatted string with the gauge's values.</returns>
        public static string ToGaugeString<T>(this IGauge<T> gauge, string pformat, string nformat = "G") where T : IFormattable
            => gauge.ToGaugeString(pformat, nformat, CultureInfo.CurrentCulture.NumberFormat);
        /// <summary>
        /// Returns a string with the gauge's values, using specific formatters.
        /// </summary>
        /// <typeparam name="T">Any IFormattable object.</typeparam>
        /// <param name="gauge">The gauge.</param>
        /// <param name="pformat">Format for the percentage.</param>
        /// <param name="nformat">Format for the other values.</param>
        /// <param name="formatter">The format provider.</param>
        /// <returns>A formatted string with the gauge's values.</returns>
        public static string ToGaugeString<T>(this IGauge<T> gauge, string pformat, string nformat, IFormatProvider formatter) where T : IFormattable
            => "Value='" + gauge.Value.ToString(nformat, formatter) + "' MaxValue='" + gauge.MaxValue.ToString(nformat, formatter) 
            + "' Percent='" + gauge.Percentage.ToString(pformat, formatter) + "'";
    }
}
