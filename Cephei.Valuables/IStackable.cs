namespace Cephei.Valuables
{
  /// <summary>
  /// The IReadOnlyStackable interface denotes an object that has a stack size attached to it and the stack size can be manipulated.
  /// </summary>
  /// <typeparam name="T">Stackable type.</typeparam>
  public interface IStackable<T> : IReadOnlyStackable<T>
  {
    /// <summary>
    /// Gets or sets the stack's size.
    /// </summary>
    new T Stack { get; set; }

    /// <summary>
    /// Increases the stack size by a certain amount.
    /// </summary>
    /// <param name="amount">Amount to increase the stack size.</param>
    void Increase(T amount);

    /// <summary>
    /// Decreases the stack size by a certain amount.
    /// </summary>
    /// <param name="amount">Amount to decrease from the stack size.</param>
    void Decrease(T amount);
  }
}
