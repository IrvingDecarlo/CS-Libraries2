using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// Read-Only Bi-Dictionaries are cross-referenced dictionaries, where the keys and values are bi-directionally referenced.
  /// </summary>
  /// <typeparam name="T">Key object type.</typeparam>
  /// <typeparam name="U">Value object type.</typeparam>
  public interface IReadOnlyBiDictionary<T, U> : IReadOnlyDictionary<T, U>
  {
    /// <summary>
    /// Gets the BiDictionary's mirror.
    /// </summary>
    /// <returns>The BiDictionary's mirror.</returns>
    IReadOnlyDictionary<U, T> GetMirror();
  }
}
