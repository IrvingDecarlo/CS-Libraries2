using System;

namespace Cephei.Files
{
  /// <summary>
  /// The IPersistentFile interface provides a base for all classes that are directly dependant upon a file.
  /// </summary>
  public interface IPersistentFile : IEquatable<IPersistentFile>, IReadableFile, IWriteableFile
  {
    /// <summary>
    /// The file's path.
    /// </summary>
    string FilePath { get; }
  }
}
