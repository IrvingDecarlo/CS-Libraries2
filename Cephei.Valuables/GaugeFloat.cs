namespace Cephei.Valuables
{
  /// <summary>
  /// The Gauge is a basic implementation for IGauges.
  /// </summary>
  public class GaugeFloat : IGauge<float, float>
  {
    /// <summary>
    /// Creates a new float Gauge.
    /// </summary>
    /// <param name="value">The gauge's initial value.</param>
    /// <param name="maxvalue">The gauge's max value.</param>
    public GaugeFloat(float value, float maxvalue)
    {
      MaxValue = maxvalue;
      Value = value;
    }
    /// <summary>
    /// Creates a new float Gauge without values.
    /// </summary>
    public GaugeFloat()
    { }

    #region overrides

    /// <summary>
    /// Gets or sets the gauge's max value.
    /// </summary>
    public virtual float MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the gauge's percentage.
    /// </summary>
    /// <remarks>Caching the value is recommended since it is not stored locally within the gauge.</remarks>
    public virtual float Percentage
    {
      set => Value = MaxValue * value;
      get => Value / MaxValue;
    }

    /// <summary>
    /// Gets or sets the gauge's value.
    /// </summary>
    public virtual float Value { get; set; }

    #endregion
  }
}