namespace Cephei.Logging
{
  /// <summary>
  /// The FileLogger implements the Logger class by outputting data to an appended file.
  /// </summary>
  public class FileLogger : FileLoggerAbstract
  {
    /// <summary>
    /// Creates a FileLogger object, creating a new (or appending to) a file.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <param name="level">The logger's initial log level.</param>
    /// <remarks>Does not create the directories that lead to the file.</remarks>
    public FileLogger(string path, LogLevel level = LogLevel.Information) : base(path) 
      => LogLevel = level;

    #region override

    /// <summary>
    /// Gets the logger's log level.
    /// </summary>
    /// <returns>The logger's log level.</returns>
    protected override LogLevel GetLogLevel() => LogLevel;

    #endregion

    #region public

    /// <summary>
    /// Defines the logger's log level.
    /// </summary>
    public LogLevel LogLevel;

    #endregion
  }
}
