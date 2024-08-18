using System.Threading.Tasks;

namespace Cephei.Objects
{
  /// <summary>
  /// The IExecutableAsync denotes an object that executes an asynchronous method.
  /// </summary>
  public interface IExecutableAsync
  {
    /// <summary>
    /// Executes the operation.
    /// </summary>
    /// <returns>The task for executing the operation.</returns>
    Task ExecuteAsync();
  }
  /// <summary>
  /// The IExecutableAsync denotes an object that executes an asynchronous method.
  /// </summary>
  /// <typeparam name="T">Object type that is used as an argument.</typeparam>
  public interface IExecutableAsync<T>
  {
    /// <summary>
    /// Executes the operation with a parameter.
    /// </summary>
    /// <param name="parms">Parameter to use for executing the operation.</param>
    /// <returns>The task for executing the operation.</returns>
    Task ExecuteAsync(T parms);
  }
}
