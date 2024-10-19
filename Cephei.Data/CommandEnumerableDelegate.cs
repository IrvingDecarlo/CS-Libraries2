using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Cephei.Data
{
  /// <summary>
  /// The CommandEnumerableDelegate is an enumerable based on a database command that returns a DataReaderEnumeratorDelegate as its enumerator.
  /// </summary>
  /// <typeparam name="T">Object type that is returned by the enumerable.</typeparam>
  public class CommandEnumerableDelegate<T> : CommandEnumerable<T>
  {
    /// <summary>
    /// Creates a new enumerable for a command.
    /// </summary>
    /// <param name="command">Command to enumerate for.</param>
    /// <param name="func">Function to extract the object out of the command's reader.</param>
    /// <param name="behavior">The command's behavior.</param>
    public CommandEnumerableDelegate(DbCommand command, Func<DbDataReader, T> func, CommandBehavior behavior = CommandBehavior.Default) : base(command)
    { 
      Function = func;
      Behavior = behavior;
    }

    #region overrides

    /// <summary>
    /// Gets the reader to iterate through the command's results asynchronously.
    /// </summary>
    /// <param name="token">CancellationToken for the operation.</param>
    /// <returns>The reader to iterate through the command's results asynchronously.</returns>
    public override IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token) => GetReader(token);

    /// <summary>
    /// Gets the reader to iterate through the command's results.
    /// </summary>
    /// <returns>The reader to iterate through the command's results.</returns>
    public override IEnumerator<T> GetEnumerator() => GetReader();

    #endregion

    #region public

    /// <summary>
    /// Function to extract the object out of the data reader.
    /// </summary>
    public Func<DbDataReader, T> Function;

    /// <summary>
    /// The command execution's behavior.
    /// </summary>
    public CommandBehavior Behavior;

    #endregion

    #region private

    private DataReaderEnumeratorDelegate<T> GetReader(CancellationToken token = default) 
      => new DataReaderEnumeratorDelegate<T>(Command.ExecuteReader(Behavior), Function, token);

    #endregion
  }
}
