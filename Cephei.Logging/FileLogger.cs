using System;
using System.IO;

namespace Cephei.Logging
{
  /// <summary>
  /// The FileLogger implements the Logger class by outputting data to an appended file.
  /// </summary>
  public abstract class FileLogger : Logger, IDisposable
  {
    /// <summary>
    /// Creates a FileLogger object, creating a new (or appending to) a file.
    /// </summary>
    /// <param name="path">Path to the file.</param>
    /// <remarks>Does not create the directories that lead to the file.</remarks>
    public FileLogger(string path) => Writer = new StreamWriter(path, true);

    /// <summary>
    /// Disposes of the FileLogger.
    /// </summary>
    ~FileLogger() => Dispose();

    #region override

    /// <summary>
    /// Gets the writer used by the logger.
    /// </summary>
    /// <returns>The writer used by the logger.</returns>
    protected override TextWriter? GetLogWriter() => Writer;

    /// <summary>
    /// Disposes of the FileLogger, disposing its underlying stream.
    /// </summary>
    public void Dispose()
    {
      OnDispose();
      Writer?.Dispose();
    }

    /// <summary>
    /// Additional actions to take when the logger is disposed.
    /// </summary>
    protected abstract void OnDispose();

    #endregion

    #region protected

    /// <summary>
    /// Reference to the writer used by the logger.
    /// </summary>
    protected readonly TextWriter Writer;

    #endregion
  }
}
