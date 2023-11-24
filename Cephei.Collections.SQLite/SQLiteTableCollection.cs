using System;
using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// The SQLiteTableCollection represents the collection of tables in a connection.
  /// </summary>
  public abstract class SQLiteTableCollection : SQLiteReadOnlyTableCollection
  {
    #region overrides

    /// <summary>
    /// Adds a new table to the database.
    /// </summary>
    /// <param name="table">The new table's name.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public override void Add(string table)
    {
      ValidateReadOnly();
      DoAdd(table);
    }

    /// <summary>
    /// Drops all of this connection's tables.
    /// </summary>
    public override void Clear()
    {
      ValidateReadOnly();
      foreach (string table in this) Remove(table);
    }

    /// <summary>
    /// Removes a table.
    /// </summary>
    /// <param name="table">Table to be removed.</param>
    /// <returns>Always true, regardless if the table was found or not.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override bool Remove(string table)
    {
      ValidateReadOnly();
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = "drop table " + table;
      cmd.ExecuteNonQuery();
      return true;
    }

    #endregion

    #region protected

    /// <summary>
    /// Adds a new table to the database.
    /// </summary>
    /// <param name="table">The new table's name.</param>
    protected abstract void DoAdd(string table);

    #endregion

    #region private

    private void ValidateReadOnly()
    {
      if (IsReadOnly) ThrowNotSupported();
    }

    #endregion
  }
}
