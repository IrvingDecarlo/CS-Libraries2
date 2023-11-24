namespace Cephei.Objects
{
  /// <summary>
  /// The IPositionable interface denotes an object that can have its position manipulated in any way.
  /// </summary>
  /// <typeparam name="T">Value type to be used as the object's position.</typeparam>
  public interface IPositionable<T> : IMovablePositionable<T>
  {
    /// <summary>
    /// Gets or sets the position.
    /// </summary>
    new T Position { get; set; }
  }
}
