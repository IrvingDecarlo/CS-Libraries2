using System;
using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// Delegate SQLiteTableCollections use delegates for adding tables.
  /// </summary>
  public sealed class SQLiteTableCollectionDelegate : SQLiteTableCollection
  {
    /// <summary>
    /// Creates a new delegate SQLiteTableCollection.
    /// </summary>
    /// <param name="conn">Connection to add to it.</param>
    /// <param name="add">Action to take for adding tables.</param>
    /// <param name="rdonly">Is the table collection read-only?</param>
    public SQLiteTableCollectionDelegate(SQLiteConnection conn, Action<SQLiteConnection, string> add, bool rdonly = false) 
    {
      this.add = add;
      connection = conn;
      this.rdonly = rdonly;
    }

    #region overrides

    /// <summary>
    /// Gets the database's connection.
    /// </summary>
    protected override SQLiteConnection Connection => connection;

    /// <summary>
    /// Adds a table using the AddTable delegate.
    /// </summary>
    /// <param name="table">Table to add.</param>
    protected override void DoAdd(string table) => add(connection, table);

    /// <summary>
    /// Is this table collection read only?
    /// </summary>
    public override bool IsReadOnly => rdonly;

    #endregion

    #region private

    private readonly Action<SQLiteConnection, string> add;
    private readonly SQLiteConnection connection;
    private readonly bool rdonly;

    #endregion
  }
}
