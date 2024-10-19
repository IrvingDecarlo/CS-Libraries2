using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Data
{
  /// <summary>
  /// The CommandEnumerable is an enumerable based on a database command that returns a DataReaderEnumerator as its enumerator.
  /// </summary>
  /// <typeparam name="T">Object type that is returned by the enumerable.</typeparam>
  public abstract class CommandEnumerable<T> : IEnumerable<T>, IAsyncEnumerable<T>, IDisposable, IAsyncDisposable
  {
    /// <summary>
    /// Creates a new enumerable for a command.
    /// </summary>
    /// <param name="command">Command to enumerate for.</param>
    public CommandEnumerable(DbCommand command) => Command = command;

    #region overrides

    /// <summary>
    /// Gets the enumerator for the command.
    /// </summary>
    /// <returns>The enumerator for the command.</returns>
    public abstract IEnumerator<T> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Gets the async enumerator for the command.
    /// </summary>
    /// <param name="token">CancellationToken for the operation.</param>
    /// <returns>The async enumerator for the command.</returns>
    public abstract IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token);

    /// <summary>
    /// Disposes of the enumerable's command.
    /// </summary>
    public void Dispose() => Command.Dispose();

    /// <summary>
    /// Disposes of the enumerable's command asynchronously.
    /// </summary>
    public ValueTask DisposeAsync() => Command.DisposeAsync();

    #endregion

    #region public

    // VARIABLES

    /// <summary>
    /// Command that will be used by the enumerable to create the enumerator.
    /// </summary>
    public DbCommand Command;

    #endregion
  }
}
