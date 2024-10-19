using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Data
{
  /// <summary>
  /// The DataReaderEnumerator enumerates a DbDataReader into an object.
  /// </summary>
  /// <typeparam name="T">Object type that is extracted by the enumerator.</typeparam>
  public abstract class DataReaderEnumerator<T> : IEnumerator<T>, IAsyncEnumerator<T>
  {
    /// <summary>
    /// Creates a new enumerator for a reader.
    /// </summary>
    /// <param name="reader">Reader to enumerate through.</param>
    /// <param name="token">The reader's cancellation token.</param>
    public DataReaderEnumerator(DbDataReader reader, CancellationToken token = default)
    {
      Reader = reader;
      Token = token;
    }

    #region overrides

    /// <summary>
    /// Gets the current object in the reader.
    /// </summary>
    public abstract T Current { get; }
    object? IEnumerator.Current => Current;

    /// <summary>
    /// Moves the reader to the next result.
    /// </summary>
    /// <returns>False if the reader reached the end, true otherwise.</returns>
    public bool MoveNext() => Reader.Read();

    /// <summary>
    /// Resets the reader. Throws an exception since it is not supported.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public void Reset() => throw new NotSupportedException();

    /// <summary>
    /// Disposes of the reader.
    /// </summary>
    public void Dispose() => Reader.Dispose();

    /// <summary>
    /// Moves the reader to the next result asynchronously.
    /// </summary>
    public async ValueTask<bool> MoveNextAsync() => await Reader.ReadAsync(Token);

    /// <summary>
    /// Disposes of the reader asynchronously.
    /// </summary>
    public ValueTask DisposeAsync() => Reader.DisposeAsync();

    #endregion

    #region public

    /// <summary>
    /// Reader that is being enumerated.
    /// </summary>
    public readonly DbDataReader Reader;

    /// <summary>
    /// The reader's cancellation token.
    /// </summary>
    public CancellationToken Token;

    #endregion
  }
}
