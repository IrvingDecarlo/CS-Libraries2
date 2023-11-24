namespace Cephei.Files
{
  /// <summary>
  /// The IWriteableFile interface is used for file objects that can write their content back to their file.
  /// </summary>
  public interface IWriteableFile
  {
    /// <summary>
    /// Outputs the object's content to the file.
    /// </summary>
    void Write();
  }
}
