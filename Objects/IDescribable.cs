namespace Cephei.Objects
{
  /// <summary>
  /// The IDescribable interface refers to an object that can be described.
  /// </summary>
  /// <typeparam name="T">The object type for the description.</typeparam>
  public interface IDescribable<T>
  {
    /// <summary>
    /// Gets the object's description.
    /// </summary>
    /// <returns>The object's description.</returns>
    T GetDescription();
  }
}
