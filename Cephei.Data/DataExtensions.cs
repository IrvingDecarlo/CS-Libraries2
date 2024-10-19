using System;
using System.Data;
using System.Data.Common;

namespace Cephei.Data
{
  /// <summary>
  /// The DataExtensions class contains extension methods for data operations.
  /// </summary>
  public static class DataExtensions
  {
    /// <summary>
    /// Wraps a Command Enumerable around a DbCommand.
    /// </summary>
    /// <typeparam name="T">Object type to be extracted by the command's reader.</typeparam>
    /// <param name="command">Command to wrap the enumerable around.</param>
    /// <param name="func">Function to extract the object out of the reader.</param>
    /// <param name="behavior">The command's behavior.</param>
    /// <returns>The enumerable wrapping the DbCommand.</returns>
    public static CommandEnumerableDelegate<T> ToEnumerable<T>(this DbCommand command, Func<DbDataReader, T> func, CommandBehavior behavior = CommandBehavior.Default)
      => new CommandEnumerableDelegate<T>(command, func, behavior);
    /// <summary>
    /// Wraps a Data Reader Enumerable around a DbDataReader.
    /// </summary>
    /// <typeparam name="T">Object type to be extracted by the reader.</typeparam>
    /// <param name="reader">Data Reader to wrap the enumerable around.</param>
    /// <param name="func">Function to extract the object out of the reader.</param>
    /// <returns>The enumerable wrapping the DbDataReader.</returns>
    public static DataReaderEnumerableDelegate<T> ToEnumerable<T>(this DbDataReader reader, Func<DbDataReader, T> func)
      => new DataReaderEnumerableDelegate<T>(reader, func);
  }
}
