using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// ProtectedCollections without the second type parameter are identical to their twin, but default their collection type to ICollection{T}.
  /// </summary>
  /// <typeparam name="T">The collection's object type.</typeparam>
  public class ProtectedCollection<T> : ProtectedCollection<T, ICollection<T>>
  {
    /// <summary>
    /// Creates a ProtectedCollection around an already existing collection.
    /// </summary>
    /// <param name="col">Collection to envelop around.</param>
    public ProtectedCollection(ICollection<T> col) : base(col)
    { }
  }
  /// <summary>
  /// ProtectedCollections are collections that only their Getters are publicly accessible. The raw access to the collection is protected.
  /// </summary>
  /// <typeparam name="T">The collection's object type.</typeparam>
  /// <typeparam name="U">The collection's type.</typeparam>
  public class ProtectedCollection<T, U> : IReadOnlyCollection<T> where U : ICollection<T>
  {
    /// <summary>
    /// Creates a ProtectedCollection around an already existing collection.
    /// </summary>
    /// <param name="col">Collection to envelop around.</param>
    public ProtectedCollection(U col) => Collection = col;

    //
    // OVERRIDES
    //

    /// <summary>
    /// Number of objects under this collection.
    /// </summary>
    public int Count => Collection.Count;

    /// <summary>
    /// Gets this collection's enumerator.
    /// </summary>
    /// <returns>The collection's enumerator.</returns>
    public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    //
    // PUBLIC
    //

    // METHODS

    /// <summary>
    /// Checks if the collection contains an item.
    /// </summary>
    /// <param name="item">The item to look for.</param>
    /// <returns>True if the item is present in the collection.</returns>
    public bool Contains(T item) => Collection.Contains(item);

    /// <summary>
    /// Copies the collection's data to an array.
    /// </summary>
    /// <param name="array">Array to receive the collection's data.</param>
    /// <param name="index">Starting index.</param>
    public void CopyTo(T[] array, int index) => Collection.CopyTo(array, index);

    //
    // PROTECTED
    //

    /// <summary>
    /// Direct reference to this object's collection.
    /// </summary>
    protected U Collection;
  }
}
