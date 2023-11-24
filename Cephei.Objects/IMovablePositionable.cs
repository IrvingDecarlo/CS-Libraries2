namespace Cephei.Objects
{
  /// <summary>
  /// An IMovablePositionable object can only have its position changed via the Move methods.
  /// </summary>
  /// <typeparam name="T">Value type to be used as the position.</typeparam>
  public interface IMovablePositionable<T> : IReadOnlyPositionable<T>
  {
    /// <summary>
    /// Moves the position to the next.
    /// </summary>
    /// <param name="amount">Amount to move forward.</param>
    /// <returns>The new position.</returns>
    T MoveNext(T amount);

    /// <summary>
    /// Moves the position to the previous.
    /// </summary>
    /// <param name="amount">Amount to move behind.</param>
    /// <returns>The new position.</returns>
    T MovePrevious(T amount);
  }
}
