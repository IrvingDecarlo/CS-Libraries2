using System.Collections.Generic;

namespace Cephei.Files
{
  /// <summary>
  /// The IFileExceptionReader is a file reader but that outputs FileReadExceptions to a list when they occur.
  /// </summary>
  /// <typeparam name="T">The file type that the reader is to use.</typeparam>
  public interface IFileExceptionReader<T>
  {
    /// <summary>
    /// Reads the file, outputting any FileReadExceptions that may have occurred.
    /// </summary>
    /// <param name="obj">The file to read.</param>
    /// <param name="excs">Exceptions that have occurred.</param>
    void Read(T obj, List<FileReadException> excs);
  }
}
