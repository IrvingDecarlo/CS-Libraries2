using System;

namespace Cephei.Objects
{
  /// <summary>
  /// SystemNotModifiableExceptions are similar to ObjectNotModifiableExceptions but without an object reference.
  /// </summary>
  public class SystemNotModifiableException : Exception
  {
    /// <summary>
    /// Creates a new SystemNotModifiableException with a standard message.
    /// </summary>
    public SystemNotModifiableException() : base("The system is not modifiable")
    { }
  }
}
