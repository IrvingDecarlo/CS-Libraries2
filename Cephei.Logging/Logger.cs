using System;
using System.IO;

namespace Cephei.Logging
{
  /// <summary>
  /// The Logger class is an abstract implementation for the ILogger interface.
  /// </summary>
  public abstract class Logger : ILogger
  {
    #region overrides
    /// <summary>
    /// Outputs an exception to the log.
    /// </summary>
    /// <param name="e">Exception to be outputted to it.</param>
    /// <param name="level">LogLevel to use for the exception.</param>
    public void Log(Exception e, LogLevel level = LogLevel.Exception) => Log(e.Message, level);
    /// <summary>
    /// Writes a line to the log.
    /// </summary>
    /// <param name="message">Message to be written.</param>
    /// <param name="level">LogLevel to use for the exception.</param>
    public void Log(string message, LogLevel level)
    {
      TextWriter? writer = GetLogWriter();
      if (writer is null || level < GetLogLevel()) return;
      writer.WriteLine($"{DateTime.Now.ToString(GetDateFormat())} [{level.GetIdentifier()}]: {message}.");
    }

    #endregion

    #region protected

    /// <summary>
    /// Gets the TextWriter for outputting logs to.
    /// </summary>
    /// <returns>The TextWriter for outputting logs to.</returns>
    protected abstract TextWriter? GetLogWriter();

    /// <summary>
    /// Gets the logger's LogLevel.
    /// </summary>
    /// <returns>The logger's LogLevel.</returns>
    protected abstract LogLevel GetLogLevel();

    /// <summary>
    /// Gets the DateFormat for each outputted log.
    /// </summary>
    /// <returns>The DateFormat for each outputted log.</returns>
    protected virtual string GetDateFormat() => "yyyy/MM/dd-HH:mm:ss:fff";

    #endregion
  }
}
