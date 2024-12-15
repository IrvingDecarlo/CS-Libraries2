namespace Cephei.Valuables
{
  /// <summary>
  /// IReadOnlyValuables are valuable objects with a read-only Value property.
  /// </summary>
  /// <typeparam name="T">The value type.</typeparam>
  public interface IReadOnlyValuable<T>
  {
    /// <summary>
    /// Gets the object's value.
    /// </summary>
    T Value { get; }
  }
}
