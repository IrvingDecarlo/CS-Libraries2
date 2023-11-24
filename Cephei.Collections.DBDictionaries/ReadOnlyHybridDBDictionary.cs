using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cephei.Collections.DBDictionaries
{
  /// <summary>
  /// Hybrid DBDictionaries work in a hybrid manner: part of it is in memory and the remainder is in the database.
  /// </summary>
  /// <typeparam name="T">Key type.</typeparam>
  /// <typeparam name="U">Value type.</typeparam>
  /// <typeparam name="V">Dictionary type to be used.</typeparam>
  /// <typeparam name="W">DBDictionary type to be used.</typeparam>
  public class ReadOnlyHybridDBDictionary<T, U, V, W> : IReadOnlyDBDictionary<T, U>
    where V : IReadOnlyDictionary<T, U>
    where W : IReadOnlyDBDictionary<T, U>
  {
    /// <summary>
    /// Creates a new hybrid read-only DBDictionary.
    /// </summary>
    /// <param name="dictionary">Dictionary to be used.</param>
    /// <param name="database">DBDictionary to be used.</param>
    public ReadOnlyHybridDBDictionary(V dictionary, W database)
    {
      Dictionary = dictionary;
      Database = database;
    }

    #region overrides

    /// <summary>
    /// Checks if this dictionary has a key present.
    /// </summary>
    /// <param name="key">Key to look for.</param>
    /// <returns>True if it is present.</returns>
    public bool ContainsKey(T key)
      => Dictionary.ContainsKey(key) || Database.ContainsKey(key);

    /// <summary>
    /// Checks if the dictionary contains a specific item.
    /// </summary>
    /// <param name="item">Item to look for.</param>
    /// <returns>True if the item is present in the dictionary.</returns>
    public bool Contains(KeyValuePair<T, U> item) => ContainsKey(item.Key);
    /// <summary>
    /// Checks if the dictionary contains a specific object.
    /// </summary>
    /// <param name="obj">Object to look for.</param>
    /// <returns>True if the object is the key object type and it is present in the dictionary.</returns>
    public bool Contains(object obj) => obj is T key && ContainsKey(key);

    /// <summary>
    /// Tries to get a value from this dictionary.
    /// </summary>
    /// <param name="key">Key to be used to find the value.</param>
    /// <param name="value">The value (if found).</param>
    /// <returns>True if the value was found.</returns>
    public virtual bool TryGetValue(T key, out U value)
      => Dictionary.TryGetValue(key, out value) || Database.TryGetValue(key, out value);

    /// <summary>
    /// Gets a value from this dictionary.
    /// </summary>
    /// <param name="key">Key used to get the value.</param>
    /// <returns>The value.</returns>
    public virtual U this[T key]
    {
      get
      {
        if (Dictionary.TryGetValue(key, out U value)) return value;
        return Database[key];
      }
    }

    /// <summary>
    /// Gets all the keys present in the dictionary.
    /// </summary>
    public IEnumerable<T> Keys => Dictionary.Keys.Concat(Database.Keys);

    /// <summary>
    /// Gets all values present in the dictionary.
    /// </summary>
    public IEnumerable<U> Values => Dictionary.Values.Concat(Database.Values);

    /// <summary>
    /// Copies the database's data to an array.
    /// </summary>
    /// <param name="array">Array containing the key-value pairs.</param>
    /// <param name="arrayIndex">Starting index.</param>
    public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex = 0) => Database.CopyTo(array, arrayIndex);

    /// <summary>
    /// Gets the amount of items in the database.
    /// </summary>
    public int Count => Database.Count;

    /// <summary>
    /// Is this dictionary read-only? Returns true by default.
    /// </summary>
    public virtual bool IsReadOnly => true;

    /// <summary>
    /// Is this database of fixed size?
    /// </summary>
    public bool IsFixedSize => IsReadOnly;

    /// <summary>
    /// Gets an enumerator for the dictionary and the database.
    /// </summary>
    /// <returns>An enumerator for the dictionary and the database.</returns>
    public IEnumerator<KeyValuePair<T, U>> GetEnumerator() => new MultiEnumerator<KeyValuePair<T, U>>(Dictionary, Database);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    /// <summary>
    /// Disposes of the database and the dictionary (if disposable).
    /// </summary>
    public void Dispose()
    {
      if (Dictionary is IDisposable disposable) disposable.Dispose();
      Database.Dispose();
    }

    #endregion

    #region protected

    /// <summary>
    /// Reference to the hybrid DBDictionary's dictionary in memory.
    /// </summary>
    protected readonly V Dictionary;

    /// <summary>
    /// Reference to the hybrid DBDictionary's database.
    /// </summary>
    protected readonly W Database;

    #endregion
  }
}
