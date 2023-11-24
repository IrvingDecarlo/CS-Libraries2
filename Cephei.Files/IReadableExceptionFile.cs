using System.Collections.Generic;

namespace Cephei.Files
{
  /// <summary>
  /// The IReadableExceptionFile interface enables a file object to output exceptions upon reading.
  /// </summary>
  public interface IReadableExceptionFile
  {
    /// <summary>
    /// Reads the file.
    /// </summary>
    /// <param name="exceptions">List of exceptions.</param>
    void Read(List<FileReadException> exceptions);
  }
}
