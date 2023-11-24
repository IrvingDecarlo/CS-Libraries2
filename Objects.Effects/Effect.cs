using System.Collections;
using System.Collections.Generic;

namespace Cephei.Objects.Effects
{
  /// <summary>
  /// Effects are temporary objects that have other objects underneath them. Once they're deleted, their sub objects will be deleted as well.
  /// </summary>
  /// <typeparam name="T">Object type for the object's ID.</typeparam>
  public abstract class Effect<T> : EffectObject<T>, IReadOnlyDictionary<T, EffectObject<T>>
  {
    /// <summary>
    /// Creates a new Effect.
    /// </summary>
    /// <param name="id">The object's ID. It cannot be altered afterwards.</param>
    /// <param name="eff">Effect to assign the object under.</param>
    public Effect(T id, Effect<T>? eff = null) : base(id, eff)
    {
      dictionary = new Dictionary<T, EffectObject<T>>();
      OnDeleted += OnDelete;
    }

    #region overrides

    /// <summary>
    /// Returns a string defining this effect.
    /// </summary>
    /// <returns>A string defining the effect.</returns>
    public override string ToString() => $"Effect ID='{ID}' Objects: {Count}";

    /// <summary>
    /// Checks if an object of specific ID exists within this effect.
    /// </summary>
    /// <param name="key">The object's ID.</param>
    /// <returns>True if such object is present in this effect.</returns>
    public bool ContainsKey(T key) => dictionary.ContainsKey(key);

    /// <summary>
    /// Tries to get an object out of this effect.
    /// </summary>
    /// <param name="key">The object's ID.</param>
    /// <param name="value">The object (if found).</param>
    /// <returns>True if the object is present in this effect.</returns>
    public bool TryGetValue(T key, out EffectObject<T> value) => dictionary.TryGetValue(key, out value);

    /// <summary>
    /// Gets an object through its ID.
    /// </summary>
    /// <param name="key">The object's ID.</param>
    /// <returns>The object.</returns>
    public EffectObject<T> this[T key] => dictionary[key];

    /// <summary>
    /// Gets the collection of object IDs in this effect.
    /// </summary>
    public IEnumerable<T> Keys => dictionary.Keys;

    /// <summary>
    /// Gets the collection of objects in this effect.
    /// </summary>
    public IEnumerable<EffectObject<T>> Values => dictionary.Values;

    /// <summary>
    /// Gets the number of objects under this effect.
    /// </summary>
    public int Count => dictionary.Count;

    /// <summary>
    /// Gets this effect's enumerator for its objects.
    /// </summary>
    /// <returns>The effect's enumerator.</returns>
    public IEnumerator<KeyValuePair<T, EffectObject<T>>> GetEnumerator() => dictionary.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region internal

    internal bool Remove(EffectObject<T> obj) => dictionary.Remove(obj);

    internal void Add(EffectObject<T> obj) => dictionary.Add(obj);

    #endregion

    #region private

    // VARIABLES

    private readonly Dictionary<T, EffectObject<T>> dictionary;

    // METHODS

    private void OnDelete()
    {
      dictionary.Values.DeleteAll();
      dictionary.Clear();
    }

    #endregion
  }
}
