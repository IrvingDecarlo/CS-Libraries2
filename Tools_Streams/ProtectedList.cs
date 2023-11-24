using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// ProtectedLists without the second type parameter are identical to their twin, but default their collection type to IList{T}.
  /// </summary>
  /// <typeparam name="T">The collection's object type.</typeparam>
  public class ProtectedList<T> : ProtectedList<T, IList<T>>
  {
    /// <summary>
    /// Creates a ProtectedList around an already existing collection.
    /// </summary>
    /// <param name="col">Collection to envelop around.</param>
    public ProtectedList(IList<T> col) : base(col)
    { }
    /// <summary>
    /// Creates a ProtectedList with a basic List.
    /// </summary>
    public ProtectedList() : base(new List<T>())
    { }
  }
  /// <summary>
  /// ProtectedLists behave similarly to regular lists but their setters are protected.
  /// </summary>
  /// <typeparam name="T">The list's object type.</typeparam>
  /// <typeparam name="U">The list type.</typeparam>
  public class ProtectedList<T, U> : ProtectedCollection<T, U>, IReadOnlyList<T> where U : IList<T>
  {
    /// <summary>
    /// Creates a ProtectedList around an already existing list.
    /// </summary>
    /// <param name="list">The list to envelop.</param>
    public ProtectedList(U list) : base(list)
    { }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Gets an object at a specific index.
    /// </summary>
    /// <param name="index">The index to look for.</param>
    /// <returns>The object in that specific index.</returns>
    public T this[int index] => Collection[index];

    //
    // PUBLIC
    //

    // METHODS

    /// <summary>
    /// Gets the index of a value in the list.
    /// </summary>
    /// <param name="value">The value to look for.</param>
    /// <returns>The value's index (-1 if not found).</returns>
    public int IndexOf(T value) => Collection.IndexOf(value);
  }
}
