using Cephei.Collections;
using System;
using System.Collections.Generic;

namespace Cephei.Commands.Consoles
{
  /// <summary>
  /// The ExceptionCollection contains all exceptions that were thrown in the console.
  /// </summary>
  public sealed class ExceptionCollection : ProtectedList<Exception>
  {
    internal ExceptionCollection() : base()
    { }

    internal IList<Exception> GetCollection() => Collection;

    /// <summary>
    /// Clears the exception collection.
    /// </summary>
    public void Clear() => Collection.Clear();
  }
}
