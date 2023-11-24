namespace Cephei.Files
{
  /// <summary>
  /// The IFileWriter interface offers the Write(T file) method which is used to write a file object's content to its file.
  /// </summary>
  public interface IFileWriter<T>
  {
    /// <summary>
    /// Writes the object's content to its file.
    /// </summary>
    /// <param name="obj">Object to be written to a file.</param>
    void Write(T obj);
  }
}
