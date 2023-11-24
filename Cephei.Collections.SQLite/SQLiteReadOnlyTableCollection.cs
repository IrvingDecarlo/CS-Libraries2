using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// The SQLiteReadOnlyTableCollection contains read-only access to the connection's table operations.
  /// </summary>
  public abstract class SQLiteReadOnlyTableCollection : ICollection<string>, IReadOnlyCollection<string>
  {
    #region overrides

    /// <summary>
    /// Tries to add a table. Throws NotSupportedException.
    /// </summary>
    /// <param name="table">Table to add.</param>
    /// <exception cref="NotSupportedException"></exception>
    public virtual void Add(string table) => ThrowNotSupported();

    /// <summary>
    /// Tries to drop all tables. Throws NotSupportedException.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public virtual void Clear() => ThrowNotSupported();

    /// <summary>
    /// Tries to drop a table. Throws NotSupportedException.
    /// </summary>
    /// <param name="table">Table to drop.</param>
    /// <returns>Throws NotSupportedException.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public virtual bool Remove(string table) => ThrowNotSupported();

    /// <summary>
    /// Checks if a specified table exists.
    /// </summary>
    /// <param name="table">The table's name.</param>
    /// <returns>True if the table exists.</returns>
    public bool Contains(string table)
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = "select exists(select 1 from sqlite_schema where type='table' and name=:name)";
      cmd.Parameters.AddWithValue("name", table);
      using SQLiteDataReader reader = cmd.ExecuteReader();
      reader.Read();
      return reader.GetBoolean(0);
    }

    /// <summary>
    /// Copies this connection's tables to an array.
    /// </summary>
    /// <param name="array">Target array.</param>
    /// <param name="i">Starting index.</param>
    public void CopyTo(string[] array, int i) => this.DoCopyTo(array, i);

    /// <summary>
    /// Gets the amount of tables in this database.
    /// </summary>
    public int Count
    {
      get
      {
        using SQLiteCommand cmd = Connection.CreateCommand();
        cmd.CommandText = "select count(1) from sqlite_schema where type='table'";
        using SQLiteDataReader reader = cmd.ExecuteReader();
        reader.Read();
        return reader.GetInt32(0);
      }
    }

    /// <summary>
    /// Gets an enumerator for this database's tables.
    /// </summary>
    /// <returns>An enumerator for this database's tables.</returns>
    public IEnumerator<string> GetEnumerator()
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = "select name from sqlite_schema where type='table'";
      return new SQLiteEnumeratorDelegate<string>(cmd.ExecuteReader(), (x) => x.GetString(0));
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Is this table collection read-only?
    /// </summary>
    public virtual bool IsReadOnly => true;

    #endregion

    #region protected

    // PROPERTIES

    /// <summary>
    /// Is the reference to the SQLite connection.
    /// </summary>
    protected abstract SQLiteConnection Connection { get; }

    // METHODS

    /// <summary>
    /// Throws a standard NotSupportedException.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    protected bool ThrowNotSupported() => throw new NotSupportedException("Tables cannot be added nor removed from this database.");

    #endregion
  }
}
