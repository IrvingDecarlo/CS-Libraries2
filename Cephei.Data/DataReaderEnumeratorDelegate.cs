using System;
using System.Data.Common;
using System.Threading;

namespace Cephei.Data
{
  /// <summary>
  /// The DataReaderEnumeratorDelegate enumerates a DbDataReader into an object using a delegate function to perform the operation.
  /// </summary>
  /// <typeparam name="T">Object type that is extracted by the enumerator.</typeparam>
  public class DataReaderEnumeratorDelegate<T> : DataReaderEnumerator<T>
  {
    /// <summary>
    /// Creates a new delegate enumerator for a reader.
    /// </summary>
    /// <param name="reader">Reader to enumerate through.</param>
    /// <param name="func">Function that extracts the object from the reader.</param>
    /// <param name="token">The reader's cancellation token.</param>
    public DataReaderEnumeratorDelegate(DbDataReader reader, Func<DbDataReader, T> func, CancellationToken token = default) : base(reader, token)
      => Function = func;

    #region overrides

    /// <summary>
    /// Gets the current object out of the reader using the delegate function.
    /// </summary>
    public override T Current => Function(Reader);

    #endregion

    #region public

    /// <summary>
    /// Function that extracts the object from the reader.
    /// </summary>
    public Func<DbDataReader, T> Function;

    #endregion
  }
}
