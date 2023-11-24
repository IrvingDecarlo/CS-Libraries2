namespace Cephei.Objects
{
  /// <summary>
  /// The IExecutable interface refers to an object that can execute a process.
  /// </summary>
  public interface IExecutable
  {
    /// <summary>
    /// Executes the process.
    /// </summary>
    void Execute();
  }
  /// <summary>
  /// The generic IExecutable executes a process with a parameter.
  /// </summary>
  /// <typeparam name="T">Type of the parameter.</typeparam>
  public interface IExecutable<T>
  {
    /// <summary>
    /// Executes the process with a parameter.
    /// </summary>
    /// <param name="param">The parameter for the process.</param>
    void Execute(T param);
  }
}
