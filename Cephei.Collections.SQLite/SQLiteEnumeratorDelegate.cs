using System;
using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// SQLiteEnumeratorDelegates have a delegate function to return the current element in the reader.
  /// </summary>
  /// <typeparam name="T">Object type to be returned by the enumerator.</typeparam>
  public class SQLiteEnumeratorDelegate<T> : SQLiteEnumerator<T>
  {
    /// <summary>
    /// Creates a new SQLiteEnumeratorDelegate.
    /// </summary>
    /// <param name="reader">Reader for it to iterate through.</param>
    /// <param name="func">Function to return the current element in the reader.</param>
    public SQLiteEnumeratorDelegate(SQLiteDataReader reader, Func<SQLiteDataReader, T> func) : base(reader)
      => GetCurrent = func;

    #region overrides

    /// <summary>
    /// Returns the current element.
    /// </summary>
    public override T Current => GetCurrent(Reader);

    #endregion

    #region public

    /// <summary>
    /// Function to return the current element.
    /// </summary>
    public Func<SQLiteDataReader, T> GetCurrent;

    #endregion
  }
}
