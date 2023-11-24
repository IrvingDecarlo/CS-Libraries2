using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// Protected Simple Dictionaries with two generic types default their collection type to IDictionary{T, U}.
  /// </summary>
  /// <typeparam name="T">Key type.</typeparam>
  /// <typeparam name="U">Value type.</typeparam>
  public abstract class ProtectedSimpleDictionary<T, U> : ProtectedSimpleDictionary<T, U, IDictionary<T, U>>
  {
    /// <summary>
    /// Creates a protected simple dictionary over an already existing dictionary.
    /// </summary>
    /// <param name="dict">Dictionary to add it under.</param>
    public ProtectedSimpleDictionary(IDictionary<T, U> dict) : base(dict) 
    { }
    /// <summary>
    /// Creates a protected simple dictionary with the basic dictionary object.
    /// </summary>
    public ProtectedSimpleDictionary() : base(new Dictionary<T, U>()) 
    { }

    #region overrides

    /// <summary>
    /// Is this dictionary read-only? Returns true by default.
    /// </summary>
    public sealed override bool IsReadOnly => true;

    #endregion
  }
  /// <summary>
  /// Protected simple dictionaries implement the Simple readonly version of dictionaries.
  /// </summary>
  /// <typeparam name="T">Key type.</typeparam>
  /// <typeparam name="U">Value type.</typeparam>
  /// <typeparam name="V">Collection type for the dictionary.</typeparam>
  public abstract class ProtectedSimpleDictionary<T, U, V> : ProtectedDictionary<T, U, V>, ISimpleReadonlyDictionary<T, U> where V : IDictionary<T, U>
  {
    /// <summary>
    /// Creates a protected simple dictionary over an already existing dictionary.
    /// </summary>
    /// <param name="dict">Dictionary to add it under.</param>
    public ProtectedSimpleDictionary(V dict) : base(dict) 
    { }

    #region overrides

    /// <summary>
    /// Checks if the dictionary contains an object.
    /// </summary>
    /// <param name="value">Object to check if it contains.</param>
    /// <returns>True if the object is present in the dictionary.</returns>
    public bool Contains(U value) => Collection.ContainsKey(GetKey(value));

    /// <summary>
    /// Copies the dictionary's objects to an array.
    /// </summary>
    /// <param name="array">Array to copy the objects to.</param>
    /// <param name="arrayIndex">Starting index.</param>
    public void CopyTo(U[] array, int arrayIndex) => Collection.Values.CopyTo(array, arrayIndex);

    /// <summary>
    /// Is this dictionary read-only?
    /// </summary>
    public abstract bool IsReadOnly { get; }

    IEnumerator<U> IEnumerable<U>.GetEnumerator() => Collection.Values.GetEnumerator();

    #endregion

    #region protected

    /// <summary>
    /// Method that will generate the value's key.
    /// </summary>
    /// <param name="value">Value to get the key from.</param>
    /// <returns>The value's key.</returns>
    protected abstract T GetKey(U value);

    #endregion
  }
}
