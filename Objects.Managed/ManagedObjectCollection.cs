using Cephei.Collections;

namespace Cephei.Objects.Managed
{
  /// <summary>
  /// The ManagedObjectCollection is the collection of ManagedObjects.
  /// </summary>
  public sealed class ManagedObjectCollection : ProtectedSimpleDictionary<int, object>, IReadOnlyPositionable<int>
  {
    internal ManagedObjectCollection() : base()
    { }

    #region overrides

    /// <summary>
    /// Gets the object's key.
    /// </summary>
    /// <param name="value">The object to get the key from.</param>
    /// <returns>The object's key.</returns>
    protected override int GetKey(object value) => value.GetHashCode();

    /// <summary>
    /// The collection's position aka the HashCode of the last created object.
    /// </summary>
    public int Position { get; private set; }

    #endregion

    #region internal

    internal bool Remove(object value) => Collection.Remove(GetKey(value));

    internal void Add(object value)
    {
      Collection.Add(GetKey(value), value);
      Position++;
    }

    internal void Clear() => Collection.Clear();

    #endregion
  }
}
