using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// SQLiteEnumerators are used for enumerating SQLite data readers.
  /// </summary>
  /// <typeparam name="T">Object type to be returned by the enumerator.</typeparam>
  public abstract class SQLiteEnumerator<T> : IEnumerator<T>
  {
    /// <summary>
    /// Creates a new enumerator for the SQL data reader.
    /// </summary>
    /// <param name="reader">Reader to be used for the enumerator.</param>
    public SQLiteEnumerator(SQLiteDataReader reader) => Reader = reader;

    #region overrides

    /// <summary>
    /// Gets the current object in the enumerator.
    /// </summary>
    public abstract T Current { get; }
    object? IEnumerator.Current => Current;

    /// <summary>
    /// Disposes of the reader.
    /// </summary>
    public void Dispose() => Reader.Dispose();

    /// <summary>
    /// Moves the reader to the next row.
    /// </summary>
    /// <returns>True if the reader moved to the next row, false if the result list has ended.</returns>
    public bool MoveNext() => Reader.Read();

    /// <summary>
    /// Attempts to reset the reader but it is not supported by SQLite readers.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public void Reset() => throw new NotSupportedException();

    #endregion

    #region public

    /// <summary>
    /// Reference to the reader enumerating the SQL's results.
    /// </summary>
    public readonly SQLiteDataReader Reader;

    #endregion
  }
}
