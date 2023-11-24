using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections.DBDictionaries
{
  /// <summary>
  /// BasicReadOnlyDatabases contain very basic functionalities that don't directly involve database handling.
  /// </summary>
  /// <typeparam name="T">Type to be used as key.</typeparam>
  /// <typeparam name="U">Type to be used as value.</typeparam>
  public abstract class BasicReadOnlyDatabase<T, U> : IReadOnlyDBDictionary<T, U>
  {
    #region overrides

    /// <summary>
    /// Is this database readonly?
    /// </summary>
    public virtual bool IsReadOnly => true;

    /// <summary>
    /// Is this database of fixed size? Returns the same as IsReadOnly.
    /// </summary>
    public bool IsFixedSize => IsReadOnly;

    /// <summary>
    /// Checks if this database contains a specific item.
    /// </summary>
    /// <param name="item">KeyValuePair item to look for.</param>
    /// <returns>True if the item is present.</returns>
    /// <remarks>Only the key is checked, the value component is ignored.</remarks>
    public bool Contains(KeyValuePair<T, U> item) => ContainsKey(item.Key);
    /// <summary>
    /// Checks if this database contains a specific key object.
    /// </summary>
    /// <param name="key">Key object.</param>
    /// <returns>True if the object is of the correct key type and is present in this database.</returns>
    public bool Contains(object key) => key is T tkey && ContainsKey(tkey);

    /// <summary>
    /// Copies all of this database's items to an array.
    /// </summary>
    /// <param name="array">Array to copy the items to.</param>
    /// <param name="arrayIndex">The starting index.</param>
    public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
    {
      foreach (KeyValuePair<T, U> item in this)
      {
        array[arrayIndex] = item;
        arrayIndex++;
      }
    }

    /// <summary>
    /// Checks if a specific key is present in this database.
    /// </summary>
    /// <param name="key">Key to be looked for.</param>
    /// <returns>True if that specific key is present in the database.</returns>
    public abstract bool ContainsKey(T key);

    /// <summary>
    /// Tries to get a value out of this database.
    /// </summary>
    /// <param name="key">Key to be used.</param>
    /// <param name="value">Value collected from the database (if the key is present).</param>
    /// <returns>True if the key was found and the value was returned.</returns>
    public abstract bool TryGetValue(T key, out U value);

    /// <summary>
    /// Gets a value out of this database.
    /// </summary>
    /// <param name="key">Key used to locate the value.</param>
    /// <returns>The value.</returns>
    public abstract U this[T key] { get; }

    /// <summary>
    /// Gets all of this database's keys.
    /// </summary>
    public abstract IEnumerable<T> Keys { get; }

    /// <summary>
    /// Gets all of this database's values.
    /// </summary>
    public abstract IEnumerable<U> Values { get; }

    /// <summary>
    /// Gets the amount of items in this database.
    /// </summary>
    public abstract int Count { get; }

    /// <summary>
    /// Gets an enumerator that will iterate through all of this database's items.
    /// </summary>
    /// <returns>Enumerator to iterate through all of the database's items.</returns>
    public abstract IEnumerator<KeyValuePair<T, U>> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Disposes of this database's connection.
    /// </summary>
    public abstract void Dispose();

    #endregion
  }
}
