namespace Cephei.Files.DA
{
  /// <summary>
  /// The IDaObject offers a common base for all objects related to the 2da file.
  /// </summary>
  public interface IDaObject
  {
    /// <summary>
    /// Reference to the object's file. Can be null if it is an orphan object.
    /// </summary>
    DaFile? File { get; }
  }
}
