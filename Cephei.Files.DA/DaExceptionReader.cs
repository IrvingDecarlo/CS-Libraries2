using System.Collections.Generic;

namespace Cephei.Files.DA
{
  /// <summary>
  /// The DaExceptionReader reads the 2da's content to the file object, outputting exceptions when they occur.
  /// </summary>
  public class DaExceptionReader : DaReader, IFileExceptionReader<DaFile>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Reads the file's content to the object, outputting exceptions whenever they occur.
    /// </summary>
    /// <param name="file">File object to read.</param>
    /// <param name="excs">List of outputted exceptions.</param>
    public void Read(DaFile file, List<FileReadException> excs)
    {
      Exceptions = excs;
      Read(file);
    }

    /// <summary>
    /// Adds a Too Many Columns exception to the exceptions list.
    /// </summary>
    /// <param name="file">Reference to the file.</param>
    /// <param name="text">Text that was attempted to be inserted.</param>
    /// <param name="count">File column count.</param>
    /// <param name="col">Attempted column index.</param>
    /// <param name="line">File line.</param>
    protected override void TooManyColumns(DaFile file, string text, int count, int col, ushort line)
      => Exceptions.Add(new FileReadException<ushort>(file.FilePath, line, $"The file attempted to add column {col} ({text}) while the file's column capacity is {count}."));

    //
    // PUBLIC
    //

    // VARIABLES

    /// <summary>
    /// List of exceptions outputted. The list is filled even when the regular Read(DaFile) method is called.
    /// </summary>
    public List<FileReadException> Exceptions = new List<FileReadException>();
  }
}
