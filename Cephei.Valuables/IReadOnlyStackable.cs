namespace Cephei.Valuables
{
  /// <summary>
  /// The IReadOnlyStackable interface denotes an object that has a stack size attached to it.
  /// </summary>
  /// <typeparam name="T">Stackable type.</typeparam>
  public interface IReadOnlyStackable<T>
  {
    /// <summary>
    /// Gets the object's stack size.
    /// </summary>
    T Stack { get; }

    /// <summary>
    /// Can the stack size be decreased by a certain amount?
    /// </summary>
    /// <param name="amount">Amount to decrease it by.</param>
    /// <param name="new">The stack's new size after it is decreased.</param>
    /// <returns>True if it can be decreased by said amount.</returns>
    bool CanDecrease(T amount, out T @new);
  }
}
