using System;

namespace Cephei.Networking
{
  /// <summary>
  /// ConnectionExceptions are thrown whenever a connection/action fails to be performed.
  /// </summary>
  public class ConnectionException : Exception
  {
    /// <summary>
    /// Creates a new ConnectionException.
    /// </summary>
    /// <param name="message">Base message to show.</param>
    /// <param name="attempts">Number of attempts taken until failure.</param>
    public ConnectionException(string message, int attempts) : base(message + $" (Attempts: {attempts})")
      => Attempts = attempts;

    #region public

    /// <summary>
    /// Number of attempts performed until failure.
    /// </summary>
    public readonly int Attempts;

    #endregion
  }
}
