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
    public GaugeInt(int value = 0, int maxvalue = 0)
    {
      this.value = value;
      this.maxvalue = maxvalue;
    }

    #region overrides

    /// <summary>
    /// Gets or sets the gauge's max value.
    /// </summary>
    public virtual int MaxValue
    {
      set => maxvalue = value;
      get => maxvalue;
    }

    /// <summary>
    /// Gets or sets the gauge's percentage.
    /// </summary>
    /// <remarks>Caching the value is recommended since it is not stored locally within the gauge.</remarks>
    public virtual float Percentage
    {
      set => this.value = (int)(maxvalue * value);
      get => value / maxvalue;
    }

    /// <summary>
    /// Gets or sets the gauge's value.
    /// </summary>
    public virtual int Value
    {
      set => this.value = value;
      get => value;
    }

    #endregion

    #region private

    private int value, maxvalue;

    #endregion
  }
}