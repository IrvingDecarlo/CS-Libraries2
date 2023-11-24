namespace Cephei.Files
{
  /// <summary>
  /// IReadableFile is used for objects which can have their content generated from reading a file.
  /// </summary>
  public interface IReadableFile
  {
    /// <summary>
    /// Reads the file's content, importing it to the object.
    /// </summary>
    void Read();
  }
}
