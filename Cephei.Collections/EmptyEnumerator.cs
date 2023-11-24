using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// The generic EmptyEnumerator class behaves similarly to its non-generic counterpart, albeit implementing IEnumerator{T}.
  /// </summary>
  /// <typeparam name="T">The enumerator's object type.</typeparam>
  public readonly struct EmptyEnumerator<T> : IEnumerator<T>
  {
    /// <summary>
    /// Creates a new EmptyEnumerator, assigning a default object for it to return.
    /// </summary>
    /// <param name="def">The default object to return.</param>
    public EmptyEnumerator(T def)
      => Default = def;

    //
    // OVERRIDES
    //

    /// <summary>
    /// The current object in this enumerator. It always returns the default object.
    /// </summary>
    public T Current => Default;
    object? IEnumerator.Current => null;

    /// <summary>
    /// MoveNext does nothing, returning false by default.
    /// </summary>
    /// <returns>False.</returns>
    public bool MoveNext() => false;

    /// <summary>
    /// Reset does nothing.
    /// </summary>
    public void Reset() { }

    /// <summary>
    /// "Disposes" this enumerator.
    /// </summary>
    public void Dispose()
    { }

    /// <summary>
    /// The default object that was assigned to this enumerator.
    /// </summary>
    public readonly T Default;
  }
  /// <summary>
  /// The EmptyEnumerator class implements IEnumerator albeit without any function, meaning that the Current object will always be null and MoveNext will never be possible.
  /// </summary>
  public readonly struct EmptyEnumerator : IEnumerator
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// MoveNext does nothing, returning false by default.
    /// </summary>
    /// <returns>False.</returns>
    public bool MoveNext() => false;

    /// <summary>
    /// Reset does nothing.
    /// </summary>
    public void Reset() { }

    /// <summary>
    /// Returns a null object by default.
    /// </summary>
    public object? Current => null;

    //
    // STATIC
    //

    static EmptyEnumerator() => Default = new EmptyEnumerator();

    // VARIABLES

    /// <summary>
    /// Reference to an EmptyEnumerator. It can be used to avoid the creation of additional EmptyEnumerators.
    /// </summary>
    public static readonly EmptyEnumerator Default;
  }
}
