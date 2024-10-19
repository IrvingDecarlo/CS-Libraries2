using System;
using System.Collections.Generic;

namespace Cephei.Data.DBDictionaries
{
  /// <summary>
  /// Hybrid DBDictionaries work in a hybrid manner: part of it is in memory and the remainder is in the database.
  /// </summary>
  /// <typeparam name="T">Key type.</typeparam>
  /// <typeparam name="U">Value type.</typeparam>
  /// <typeparam name="V">Dictionary type to be used.</typeparam>
  /// <typeparam name="W">DBDictionary type to be used.</typeparam>
  public class HybridDBDictionary<T, U, V, W> : ReadOnlyHybridDBDictionary<T, U, V, W>, IDBDictionary<T, U>
    where V : IDictionary<T, U>, IReadOnlyDictionary<T, U>
    where W : IDBDictionary<T, U>
  {
    /// <summary>
    /// Creates a new HybridDBDictionary.
    /// </summary>
    /// <param name="dictionary">Dictionary to be used in memory.</param>
    /// <param name="database">Disk database to be used.</param>
    public HybridDBDictionary(V dictionary, W database) : base(dictionary, database) { }

    #region overrides

    ICollection<T> IDictionary<T, U>.Keys => throw new NotSupportedException();
    
    ICollection<U> IDictionary<T, U>.Values => throw new NotSupportedException();

    /// <summary>
    /// Gets or sets a value for the key.
    /// </summary>
    /// <param name="key">Key used to identify the value.</param>
    /// <returns>The value (if present).</returns>
    public new U this[T key]
    {
      set
      {
        if (((IDictionary<T, U>)Dictionary).ContainsKey(key)) ((IDictionary<T, U>)Dictionary)[key] = value;
        ((IDictionary<T, U>)Database)[key] = value;
      }
      get => base[key];
    }

    /// <summary>
    /// Adds a value to the dictionary and to the database.
    /// </summary>
    /// <param name="key">Key used to identify the value.</param>
    /// <param name="value">Value to be added.</param>
    public void Add(T key, U value) => Add(new KeyValuePair<T, U>(key, value));
    /// <summary>
    /// Adds a new KeyValuePair to the dictionary and to the database.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    public void Add(KeyValuePair<T, U> item)
    {
      Dictionary.Add(item);
      Database.Add(item);
    }

    /// <summary>
    /// Removes a value from the dictionary and database.
    /// </summary>
    /// <param name="key">Key used to identify the value.</param>
    /// <returns>True if it was found and removed from the database.</returns>
    public bool Remove(T key)
    {
      Dictionary.Remove(key);
      return Database.Remove(key);
    }
    bool ICollection<KeyValuePair<T, U>>.Remove(KeyValuePair<T, U> item) => Remove(item.Key);

    /// <summary>
    /// Clears the dictionary and the database.
    /// </summary>
    public void Clear()
    { 
      Dictionary.Clear();
      ((IDictionary<T, U>)Database).Clear();
    }

    /// <summary>
    /// Is this dictionary read only?
    /// </summary>
    public override bool IsReadOnly => false;

    #endregion
  }
}
