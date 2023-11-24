using Cephei.Collections;

namespace Cephei.Objects.Managed
{
  /// <summary>
  /// The ManagedCollection is the collection of managed objects.
  /// </summary>
  public sealed class ManagedCollection : SimpleDictionary<int, object>, IReadOnlyPositionable<int>
  {
    internal ManagedCollection() : base() { }

    #region overrides

    /// <summary>
    /// Gets the object's key via its HashCode.
    /// </summary>
    /// <param name="value">The object to get the key from.</param>
    /// <returns>The object's key.</returns>
    protected override int GetKey(object value) => value.GetHashCode();

    /// <summary>
    /// The collection's position aka the last object's ID.
    /// </summary>
    public int Position { get; private set; } = 0;

    #endregion
  }
}
