using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace Cephei.Data
{
  /// <summary>
  /// The DataReaderEnumerableDelegate wraps around a DbDataReader to extract a defined type of object from it.
  /// </summary>
  /// <typeparam name="T">Object type that is extracted by the enumerator.</typeparam>
  /// <remarks>The DataReader enumerable is single-use. It will be disposed after a foreach iteration.</remarks>
  public readonly struct DataReaderEnumerableDelegate<T> : IEnumerable<T>, IAsyncEnumerable<T>
  {
    /// <summary>
    /// Wraps a DbDataReader around a delegate enumerator.
    /// </summary>
    /// <param name="reader">Data Reader to wrap around.</param>
    /// <param name="func">Function to extract the object out of the reader.</param>
    public DataReaderEnumerableDelegate(DbDataReader reader, Func<DbDataReader, T> func)
    {
      Reader = reader;
      Function = func;
    }

    #region overrides

    /// <summary>
    /// Gets the enumerable's enumerator.
    /// </summary>
    /// <returns>The enumerable's enumerator.</returns>
    public IEnumerator<T> GetEnumerator() => GetDataEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Gets the enumerable's asynchronous enumerator.
    /// </summary>
    /// <param name="token">CancellationToken for the operation.</param>
    /// <returns>The enumerable's asynchronous enumerator.</returns>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token) => GetDataEnumerator(token);

    #endregion

    #region public

    /// <summary>
    /// Reader that is wrapped around by the enumerable.
    /// </summary>
    public readonly DbDataReader Reader;

    /// <summary>
    /// Function to extract the object out of the reader.
    /// </summary>
    public readonly Func<DbDataReader, T> Function;

    #endregion

    #region private

    private DataReaderEnumeratorDelegate<T> GetDataEnumerator(CancellationToken token = default) 
      => new DataReaderEnumeratorDelegate<T>(Reader, Function, token);

    #endregion
  }
}
