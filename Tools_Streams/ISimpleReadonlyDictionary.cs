using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// The ISimpleReadonlyDictionary combines the functionalities of a readonly dictionary and a read only collection, where the value object
  /// type dictates most of the dictionary's functionalities.
  /// </summary>
  /// <typeparam name="T">Key type used for the dictionary.</typeparam>
  /// <typeparam name="U">Value type used for the dictionary.</typeparam>
  public interface ISimpleReadonlyDictionary<T, U> : IReadOnlyDictionary<T, U>, IReadOnlyCollection<U>
  {
    /// <summary>
    /// Does this dictionary contain the defined object?
    /// </summary>
    /// <param name="obj">Object to check if exists within the dictionary.</param>
    /// <returns>True if it is present in the dictionary.</returns>
    bool Contains(U obj);

    /// <summary>
    /// Copies the dictionary's objects to an array.
    /// </summary>
    /// <param name="array">Array to copy the objects to.</param>
    /// <param name="arrayIndex">Starting index.</param>
    void CopyTo(U[] array, int arrayIndex);

    /// <summary>
    /// Is this dictionary read only?
    /// </summary>
    bool IsReadOnly { get; }
  }
}
