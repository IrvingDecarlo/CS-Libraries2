using Cephei.Collections;
using System.Collections.Generic;

namespace Cephei.Objects.Effects.Stats
{
  /// <summary>
  /// The ModifierCollection is the collection of modifiers that can be placed under a stat. These collections are always sorted.
  /// </summary>
  /// <typeparam name="T">The IIdentifier type.</typeparam>
  /// <typeparam name="U">The object type.</typeparam>
  /// <typeparam name="V">The IValuable type.</typeparam>
  public sealed class ModifierCollection<T, U, V> : ProtectedDictionary<T, U> where U : IModifier<T, V>
  {
    internal ModifierCollection() : base(new SortedDictionary<T, U>()) 
    { }
    internal ModifierCollection(IComparer<T> comparer) : base(new SortedDictionary<T, U>(comparer)) 
    { }

    internal bool Remove(U value) => Collection.Remove(value);

    internal void Add(U value) => Collection.Add(value);

    internal void Clear() => Collection.Clear();
  }
}
