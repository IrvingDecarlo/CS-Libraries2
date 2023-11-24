namespace Cephei.Objects
{
  /// <summary>
  /// The IReadOnlyPositionable interface denotes an object which can have a position within it manipulated.
  /// </summary>
  /// <typeparam name="T">Value type to be used as position.</typeparam>
  public interface IReadOnlyPositionable<T>
  {
    /// <summary>
    /// Gets the object's current position.
    /// </summary>
    T Position { get; }
  }
}
