using System.Collections.Generic;

namespace Cephei.Collections
{
    /// <summary>
    /// ProtectedDictionaries without the third type parameter are identical to their twin, but default their collection type to IDictionary{T, U}.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    public class ProtectedDictionary<T, U> : ProtectedDictionary<T, U, IDictionary<T, U>>
    {
        /// <summary>
        /// Creates a ProtectedDictionary around an already existing collection.
        /// </summary>
        /// <param name="col">Collection to envelop around.</param>
        public ProtectedDictionary(IDictionary<T, U> col) : base(col)
        { }
        /// <summary>
        /// Creates a ProtectedDictionary with a basic Dictionary of the defined type.
        /// </summary>
        public ProtectedDictionary() : base(new Dictionary<T, U>())
        { }
    }
    /// <summary>
    /// ProtectedDictionaries behave similarly to regular dictionaries, but their setters are protected.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <typeparam name="V">The dictionary's type.</typeparam>
    public class ProtectedDictionary<T, U, V> : ProtectedCollection<KeyValuePair<T, U>, V>, IReadOnlyDictionary<T, U> where V : IDictionary<T, U>
    {
        /// <summary>
        /// Creates a ProtectedDictionary around an already existing dictionary.
        /// </summary>
        /// <param name="dict">The dictionary to be enveloped.</param>
        public ProtectedDictionary(V dict) : base(dict)
        { }

        //
        // OVERRIDES
        //

        /// <summary>
        /// Does this dictionary contain a specific key?
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <returns>True if it contains such key.</returns>
        public bool ContainsKey(T key) => Collection.ContainsKey(key);

        /// <summary>
        /// Tries to get a value out of this dictionary.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <param name="value">The value (if found).</param>
        /// <returns>True if the key was found.</returns>
        public bool TryGetValue(T key, out U value) => Collection.TryGetValue(key, out value);

        /// <summary>
        /// Tries to get a value through a key.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <returns>The key's value.</returns>
        public U this[T key] => Collection[key];

        /// <summary>
        /// The collection of this dictionary's keys.
        /// </summary>
        public IEnumerable<T> Keys => Collection.Keys;

        /// <summary>
        /// The collection of this dictionary's values.
        /// </summary>
        public IEnumerable<U> Values => Collection.Values;
    }
}
