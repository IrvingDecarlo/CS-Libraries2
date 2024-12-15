namespace Cephei.Valuables
{
  /// <summary>
  /// The IGauge interface offers the base for gauges, implementing IValuable alongside MaxValue and percentage.
  /// </summary>
  /// <typeparam name="T">The gauge's value type.</typeparam>
  /// <typeparam name="U">The gauge's percentage value type.</typeparam>
  public interface IGauge<T, U> : IReadOnlyGauge<T, U>
  {
    /// <summary>
    /// This is the gauge's max value.
    /// </summary>
    new T MaxValue { set; get; }

    /// <summary>
    /// This is the gauge's percentage fill.
    /// </summary>
    new U Percentage { set; get; }
  }
}
