namespace Cephei.Valuables
{
    /// <summary>
    /// The IGauge interface offers the base for gauges, implementing IValuable alongside MaxValue and percentage.
    /// </summary>
    /// <typeparam name="T">Any type, but a numeric type is highly advised.</typeparam>
    public interface IGauge<T> : IValuable<T>
    {
        /// <summary>
        /// This is the gauge's max value.
        /// </summary>
        T MaxValue { set; get; }

        /// <summary>
        /// This is the gauge's percentage fill.
        /// </summary>
        T Percentage { set; get; }
    }
}
