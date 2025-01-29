namespace Cephei.Valuables
{
  /// <summary>
  /// The Gauge is a basic implementation for IGauges.
  /// </summary>
  public class GaugeInt : IGauge<int, float>
  {/// <summary>
   /// Creates a new int Gauge.
   /// </summary>
   /// <param name="value">The gauge's initial value.</param>
   /// <param name="maxvalue">The gauge's max value.</param>
    public GaugeInt(int value, int maxvalue)
    {
      MaxValue = maxvalue; 
      Value = value;
    }
    /// <summary>
    /// Creates a new int Gauge without values.
    /// </summary>
    public GaugeInt()
    { }

    #region overrides

    /// <summary>
    /// Gets or sets the gauge's max value.
    /// </summary>
    public virtual int MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the gauge's percentage.
    /// </summary>
    /// <remarks>Caching the value is recommended since it is not stored locally within the gauge.</remarks>
    public virtual float Percentage
    {
      set => Value = (int)(MaxValue * value);
      get => Value / MaxValue;
    }

    /// <summary>
    /// Gets or sets the gauge's value.
    /// </summary>
    public virtual int Value { get; set; }

    #endregion
  }
}