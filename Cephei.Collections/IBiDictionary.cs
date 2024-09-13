using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// Bi-Dictionaries are cross-referenced dictionaries, where the keys and values are bi-directionally referenced.
  /// </summary>
  /// <typeparam name="T">Key object type.</typeparam>
  /// <typeparam name="U">Value object type.</typeparam>
  public interface IBiDictionary<T, U> : IReadOnlyBiDictionary<T, U>, IDictionary<T, U>, IDictionary
  {


    /// <summary>
    /// Gets the BiDictionary's mirror.
    /// </summary>
    /// <returns>The BiDictionary's mirror.</returns>
    new IBiDictionary<U, T> GetMirror();
  }
}
