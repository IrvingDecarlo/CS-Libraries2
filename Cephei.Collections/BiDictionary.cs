using Cephei.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// The BiDictionary is a cross-referenced dictionary with bi-directional references, where the key and the value are mutually indexed.
  /// </summary>
  /// <typeparam name="T">Primary object type.</typeparam>
  /// <typeparam name="U">Secondary object type.</typeparam>
  /// <remarks>To get the object with the interface implementations for the secondary dictionary, use GetMirror().</remarks>
  public class BiDictionary<T, U> : IBiDictionary<T, U>
  {
    /// <summary>
    /// Instantiates a new bi-dictionary.
    /// </summary>
    public BiDictionary() : this(new Dictionary<T, U>(), new Dictionary<U, T>())
    { }
    /// <summary>
    /// Instantiates a new bi-dictionary based on a preexisting dictionary. The secondary dictionary will mirror the primary.
    /// </summary>
    /// <param name="dictionary">Dictionary to base the bi-directional dictionary on.</param>
    /// <remarks>May throw exceptions if the original dictionary has repeated value objects. The original dictionary should not be changed since the secondary
    /// dictionary will not be updated automatically.</remarks>
    public BiDictionary(Dictionary<T, U> dictionary) : this(dictionary, dictionary.ToMirror())
    { }
    private BiDictionary(Dictionary<T, U> primary, Dictionary<U, T> secondary) : this(primary, secondary, primary.GetSyncRoot())
    { }
    private BiDictionary(Dictionary<T, U> primary, Dictionary<U, T> secondary, object lck)
    {
      this.primary = primary;
      this.secondary = secondary;
      SyncRoot = lck;
    }

    #region overrides

    /// <summary>
    /// Gets the mirrored version of this BiDictionary.
    /// </summary>
    /// <returns>The mirrored version of this BiDictionary.</returns>
    public IBiDictionary<U, T> GetMirror() => new BiDictionary<U, T>(secondary, primary, SyncRoot);
    IReadOnlyDictionary<U, T> IReadOnlyBiDictionary<T, U>.GetMirror() => GetMirror();

    /// <summary>
    /// Adds a key and a value to the dictionary.
    /// </summary>
    /// <param name="key">Key to add.</param>
    /// <param name="value">Value to add.</param>
    public void Add(T key, U value)
    {
      primary.Add(key, value);
      try { secondary.Add(value, key); }
      catch
      {
        primary.Remove(key);
        throw;
      }
    }
    /// <summary>
    /// Adds a key and a value to the dictionary.
    /// </summary>
    /// <param name="item">Key-Value pair to add.</param>
    public void Add(KeyValuePair<T, U> item) => Add(item.Key, item.Value);
    void IDictionary.Add(object key, object value) => Add((T)key, (U)value);

    /// <summary>
    /// Checks if a key is present in the dictionary.
    /// </summary>
    /// <param name="key">Key to check.</param>
    /// <returns>True if the key is present in the dictionary.</returns>
    public bool ContainsKey(T key) => primary.ContainsKey(key);

    /// <summary>
    /// Removes a key from the dictionary.
    /// </summary>
    /// <param name="key">Key to remove.</param>
    /// <returns>True if the key was found and removed.</returns>
    public bool Remove(T key)
    {
      if (!primary.TryGetValue(key, out U value)) return false;
      return RemoveFromDictionaries(key, value);
    }
    /// <summary>
    /// Removes a key-value pair from the dictionary.
    /// </summary>
    /// <param name="item">Key-value pair to remove.</param>
    /// <returns>True if the key was found with a value equal to the KeyValuePair's and removed.</returns>
    public bool Remove(KeyValuePair<T, U> item)
    {
      T key = item.Key;
      if (!primary.TryGetValue(key, out U value) || !value.SafeEquals(item.Value)) return false;
      return RemoveFromDictionaries(key, value);
    }
    void IDictionary.Remove(object key) => Remove((T)key);

    /// <summary>
    /// Tries to get a value from the dictionary.
    /// </summary>
    /// <param name="key">Key to the value.</param>
    /// <param name="value">Value (if found).</param>
    /// <returns>True if the value was found and returned.</returns>
    public bool TryGetValue(T key, out U value) => primary.TryGetValue(key, out value);

    /// <summary>
    /// Gets or sets a value from the dictionary.
    /// </summary>
    /// <param name="key">Key to the value.</param>
    /// <returns>The key's value.</returns>
    public U this[T key]
    {
      set
      {
        secondary.Add(value, key);
        secondary.Remove(primary[key]);
        primary[key] = value;
      }
      get => primary[key];
    }
    object? IDictionary.this[object key]
    {
      set => this[(T)key] = (U)value ?? throw new InvalidCastException();
      get => primary[(T)key];
    }

    /// <summary>
    /// Gets the dictionary's keys.
    /// </summary>
    public ICollection<T> Keys => primary.Keys;
    IEnumerable<T> IReadOnlyDictionary<T, U>.Keys => primary.Keys;
    ICollection IDictionary.Keys => primary.Keys;

    /// <summary>
    /// Gets the dictionary's values.
    /// </summary>
    public ICollection<U> Values => primary.Values;
    IEnumerable<U> IReadOnlyDictionary<T, U>.Values => primary.Values;
    ICollection IDictionary.Values => primary.Values;

    /// <summary>
    /// Gets the number of elements in the dictionary.
    /// </summary>
    public int Count => primary.Count;

    /// <summary>
    /// Clears the dictionary.
    /// </summary>
    public void Clear()
    {
      primary.Clear(); 
      secondary.Clear();
    }

    /// <summary>
    /// Checks if a specific key-value pair is present in the dictionary.
    /// </summary>
    /// <param name="item">KeyValuePair to find.</param>
    /// <returns>True if the specified key-value pair is present in the dictionary.</returns>
    public bool Contains(KeyValuePair<T, U> item) => primary.Contains(item);
    bool IDictionary.Contains(object key) => primary.ContainsKey((T)key);

    /// <summary>
    /// Copies the dictionary's content to an array.
    /// </summary>
    /// <param name="array">Array to copy the content to.</param>
    /// <param name="arrayIndex">The array's starting index.</param>
    public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex) => primary.DoCopyTo(array, arrayIndex);
    void ICollection.CopyTo(Array array, int index) => primary.DoCopyTo(array, index);

    /// <summary>
    /// Is this dictionary read-only? Returns false.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets the dictionary's enumerator.
    /// </summary>
    /// <returns>The dictionary's enumerator.</returns>
    public IEnumerator<KeyValuePair<T, U>> GetEnumerator() => primary.GetEnumerator();
    IDictionaryEnumerator IDictionary.GetEnumerator() => primary.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => primary.GetEnumerator();

    /// <summary>
    /// Is the dictionary of fixed size? Returns false.
    /// </summary>
    public bool IsFixedSize => false;

    /// <summary>
    /// Is this dictionary synchronized (thread-safe)? Returns false by default.
    /// </summary>
    public bool IsSynchronized => false;

    /// <summary>
    /// Reference to the root synchronization object.
    /// </summary>
    /// <remarks>The same object will be shared with the mirrored version of this dictionary to ensure a safe lock.</remarks>
    public object SyncRoot { get; }

    #endregion

    #region private

    // VARIABLES

    private readonly Dictionary<T, U> primary;
    private readonly Dictionary<U, T> secondary;

    // METHODS

    private bool RemoveFromDictionaries(T key, U value)
    {
      secondary.Remove(value);
      return primary.Remove(key);
    }

    #endregion
  }
}
