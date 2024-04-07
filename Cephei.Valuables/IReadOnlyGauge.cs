namespace Cephei.Valuables
{
  /// <summary>
  /// The ReadOnlyGauge is a gauge which the values can never be set.
  /// </summary>
  /// <typeparam name="T">Gauge value type.</typeparam>
  /// <typeparam name="U">Percentage value type.</typeparam>
  /// 
  public interface IReadOnlyGauge<T, U> : IReadOnlyValuable<T>
  {
    /// <summary>
    /// Gets the gauge's max value.
    /// </summary>
    T MaxValue { get; }

    /// <summary>
    /// Gets the gauge's percentage.
    /// </summary>
    U Percentage { get; }
  }
}
