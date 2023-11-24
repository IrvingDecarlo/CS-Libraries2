namespace Cephei.Files
{
  /// <summary>
  /// The IFileReader has the Read(T file) method which is used to read the file object's file content into it.
  /// </summary>
  /// <typeparam name="T">The object type that is to be read.</typeparam>
  public interface IFileReader<T>
  {
    /// <summary>
    /// Reads the object's file content into it.
    /// </summary>
    /// <param name="obj">The object to be populated with its file's content.</param>
    void Read(T obj);
  }
}
