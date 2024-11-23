using System;

namespace Cephei.Logging
{
  /// <summary>
  /// The ILogger interface denotes an object that is capable of logging data.
  /// </summary>
  public interface ILogger : IDisposable
  {
    /// <summary>
    /// Outputs an exception to the log.
    /// </summary>
    /// <param name="e">Exception to be outputted.</param>
    /// <param name="level">LogLevel to use for the exception.</param>
    void Log(Exception e, LogLevel level = LogLevel.Exception);
    /// <summary>
    /// Writes a line to the log, using the current DateTime.
    /// </summary>
    /// <param name="message">Message to be written.</param>
    /// <param name="level">LogLevel to use for outputting the message.</param>
    void Log(string message, LogLevel level);
  }
}
