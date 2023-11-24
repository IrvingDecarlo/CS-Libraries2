using Cephei.Collections.SQLite;
using System.Collections.Generic;

namespace Cephei.Collections.DBDictionaries.SQLite
{
  /// <summary>
  /// SQLiteTabledReadOnlyDatabases are read-only SQLite databases with support for multiple tables.
  /// </summary>
  /// <typeparam name="T">Type used for the database's ID.</typeparam>
  /// <typeparam name="U">Type used for the database's values.</typeparam>
  public abstract class SQLiteTabledReadOnlyDatabase<T, U> : SQLiteReadOnlyDatabase<T, U>, ITabledObject
  {
    /// <summary>
    /// Creates a new SQLiteTabledReadOnlyDatabase object.
    /// </summary>
    /// <param name="dbpath">Path to the database.</param>
    public SQLiteTabledReadOnlyDatabase(string dbpath) : base(dbpath) 
    { }

    #region overrides

    /// <summary>
    /// Gets the currently managed table.
    /// </summary>
    /// <returns>The currently managed table.</returns>
    protected sealed override string GetTable() => Table;

    /// <summary>
    /// Reference to the database's current table.
    /// </summary>
    public string Table { get; set; }

    /// <summary>
    /// Gets the collection of tables in this database.
    /// </summary>
    /// <returns>The collection of tables in this database.</returns>
    public ICollection<string> GetTables() => new SQLiteConnectedReadOnlyTableCollection(Connection);

    #endregion
  }
}
