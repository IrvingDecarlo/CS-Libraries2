namespace Cephei.Objects
{
  /// <summary>
  /// The IReadOnlyModifiable interface denotes an object that its modifiability cannot be changed.
  /// </summary>
  public interface IReadOnlyModifiable
  {
    /// <summary>
    /// Is the object modifiable?
    /// </summary>
    bool Modifiable { get; }
  }
}
