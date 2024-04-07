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
    public GaugeFloat(float value = 0f, float maxvalue = 0f)
    {
      this.value = value;
      this.maxvalue = maxvalue;
    }

    #region overrides

    /// <summary>
    /// Gets or sets the gauge's max value.
    /// </summary>
    public virtual float MaxValue
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
      set => this.value = maxvalue * value;
      get => value / maxvalue;
    }

    /// <summary>
    /// Gets or sets the gauge's value.
    /// </summary>
    public virtual float Value
    {
      set => this.value = value;
      get => value;
    }

    #endregion

    #region private

    private float value, maxvalue;

    #endregion
  }
}