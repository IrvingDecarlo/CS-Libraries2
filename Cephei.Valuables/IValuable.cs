namespace Cephei.Valuables
{
  /// <summary>
  /// IValuable is the base for any object that intends to have a value assigned to it.
  /// </summary>
  /// <typeparam name="T">Any object.</typeparam>
  public interface IValuable<T> : IReadOnlyValuable<T>
  {
    /// <summary>
    /// This is the value assigned to this IValuable.
    /// </summary>
    new T Value { set; get; }
  }
}
