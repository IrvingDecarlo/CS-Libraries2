using System.Collections.Generic;
using System.Collections;

namespace Cephei.Collections
{
  /// <summary>
  /// MonoEnumerators are enumerators but with a single object as reference. Their Current object will always be the one reference when it was instantiated
  /// and they can never MoveNext, unless manually changed.
  /// </summary>
  /// <typeparam name="T">The object type that is being enumerated.</typeparam>
  public struct MonoEnumerator<T> : IEnumerator<T>
  {
    /// <summary>
    /// Creates a new MonoEnumerator for an object.
    /// </summary>
    /// <param name="obj">Object to be enumerated.</param>
    public MonoEnumerator(T obj)
    {
      Current = obj;
      moved = false;
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Reference to the enumerator's sole object.
    /// </summary>
    public T Current { set; get; }
    object? IEnumerator.Current => Current;

    /// <summary>
    /// MonoEnumerators can only move next once. After that, false by default, since there is just a single object referenced.
    /// </summary>
    /// <returns>True for the first time, false afterwards.</returns>
    public bool MoveNext()
    {
      if (moved) return false;
      moved = true;
      return true;
    }

    /// <summary>
    /// Reset simply resets MoveNext's rule.
    /// </summary>
    public void Reset() => moved = false;

    /// <summary>
    /// Dispose does nothing.
    /// </summary>
    public void Dispose()
    { }

    //
    // PRIVATE
    //

    // VARIABLES

    private bool moved;
  }
}
