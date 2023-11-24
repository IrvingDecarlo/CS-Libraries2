using Cephei.Collections.SQLite;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Cephei.Collections.DBDictionaries.SQLite
{
  /// <summary>
  /// SQLiteTabledDatabases are abstract SQLite databases with support for multiple tables.
  /// </summary>
  /// <typeparam name="T">Type used for the database's IDs.</typeparam>
  /// <typeparam name="U">Type used for the database's values.</typeparam>
  public abstract class SQLiteTabledDatabase<T, U> : SQLiteDatabase<T, U>, ITabledObject
  {
    /// <summary>
    /// Creates a new SQLiteTabledDatabase object.
    /// </summary>
    /// <param name="dbpath">Path to the database.</param>
    public SQLiteTabledDatabase(string dbpath) : base(dbpath) 
    { }

    #region overrides

    /// <summary>
    /// Gets the currently managed table.
    /// </summary>
    /// <returns>The currently managed table.</returns>
    protected sealed override string GetTable() => Table;

    /// <summary>
    /// Gets or sets the database's currently managed table, creating new tables if they do not exist.
    /// May throw exceptions when creating tables while CanCreateTables is false.
    /// </summary>
    /// <exception cref="KeyNotFoundException"></exception>
    public string Table
    {
      set
      {
        if (value.Equals(table)) return;
        CreateTable(Connection, value);
        table = value;
      }
      get => table;
    }

    /// <summary>
    /// Gets this connection's table collection.
    /// </summary>
    /// <returns>The database's table collection.</returns>
    public ICollection<string> GetTables() => new SQLiteTableCollectionDelegate(Connection, CreateTable, !CanCreateTables);

    #endregion

    #region public

    /// <summary>
    /// Can this database create new tables?
    /// </summary>
    public abstract bool CanCreateTables { get; }

    #endregion

    #region protected

    /// <summary>
    /// Gets the SQLite type for the ID.
    /// </summary>
    /// <returns>The SQLite type for the ID.</returns>
    protected abstract string GetColumnIDType();

    /// <summary>
    /// Gets the SQLite type for the value.
    /// </summary>
    /// <returns>The SQLite type for the value.</returns>
    protected abstract string GetColumnValueType();

    /// <summary>
    /// Creates a new table. Should not throw error if the specified table already exists.
    /// </summary>
    /// <param name="conn">Connection to use for the operation.</param>
    /// <param name="name">The new table's name.</param>
    protected virtual void CreateTable(SQLiteConnection conn, string name)
    {
      using SQLiteCommand cmd = conn.CreateCommand();
      cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {name}("
       + $"{GetColumnID()} {GetColumnIDType()} PRIMARY KEY NOT NULL UNIQUE,"
       + $"{GetColumnValue()} {GetColumnValueType()},"
       + $"{GetColumnCache()} INTEGER"
       + ") WITHOUT ROWID, STRICT;";
      cmd.ExecuteNonQuery();
    }

    #endregion

    #region private

    private string table;

    #endregion
  }
}
