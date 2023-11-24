using System;
using System.Collections.Generic;

namespace Cephei.Collections.DBDictionaries
{
  /// <summary>
  /// The IReadOnlyDBDictionary is a read-only dictionary that works with a database in disk.
  /// </summary>
  /// <typeparam name="T">Key object type.</typeparam>
  /// <typeparam name="U">Value object type.</typeparam>
  public interface IReadOnlyDBDictionary<T, U> : IReadOnlyDictionary<T, U>, IDisposable
  {
    /// <summary>
    /// Is the database read-only?
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Is this database of fixed size (aka read-only)?
    /// </summary>
    bool IsFixedSize { get; }

    /// <summary>
    /// Checks if the database contains a specific item.
    /// </summary>
    /// <param name="item">Item to look for.</param>
    /// <returns>True if the item is present.</returns>
    bool Contains(KeyValuePair<T, U> item);
    /// <summary>
    /// Checks if the database contains a specific object.
    /// </summary>
    /// <param name="obj">Object to look for.</param>
    /// <returns>True if the object is present in the database.</returns>
    bool Contains(object obj);

    /// <summary>
    /// Copies this database's content to an array.
    /// </summary>
    /// <param name="array">Target array.</param>
    /// <param name="arrayIndex">Starting index.</param>
    void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex);
  }
}
