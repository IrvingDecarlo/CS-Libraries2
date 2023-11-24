using System;
using System.Collections.Generic;
using System.Collections;

namespace Cephei.Collections
{
  /// <summary>
  /// The EnumerableDelegate class contains a function that'll return the Enumerable's enumerator.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class EnumerableDelegate<T> : IEnumerable<T>
  {
    /// <summary>
    /// Creates a new EnumerableDelegate.
    /// </summary>
    /// <param name="func">Function to use to get the enumerable's enumerator.</param>
    public EnumerableDelegate(Func<IEnumerator<T>> func) => GetEnum = func;

    #region overrides

    /// <summary>
    /// Gets the enumerable's enumerator.
    /// </summary>
    /// <returns>The enumerable's enumerator.</returns>
    public IEnumerator<T> GetEnumerator() => GetEnum();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region public

    /// <summary>
    /// Function that returns the enumerable's Enumerator.
    /// </summary>
    public Func<IEnumerator<T>> GetEnum;

    #endregion
  }
}
