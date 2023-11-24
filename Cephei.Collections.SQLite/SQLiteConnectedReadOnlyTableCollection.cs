using System.Data.SQLite;

namespace Cephei.Collections.SQLite
{
  /// <summary>
  /// Connected read-only SQLite table collections represent the collection of tables in a SQLite connection.
  /// </summary>
  public sealed class SQLiteConnectedReadOnlyTableCollection : SQLiteReadOnlyTableCollection
  {
    /// <summary>
    /// Creates a new connected read-only SQLite table collection.
    /// </summary>
    /// <param name="conn">Connection to use.</param>
    public SQLiteConnectedReadOnlyTableCollection(SQLiteConnection conn) => connection = conn;

    #region overrides

    /// <summary>
    /// Gets the connection.
    /// </summary>
    protected override SQLiteConnection Connection => connection;

    #endregion

    #region private

    private readonly SQLiteConnection connection;

    #endregion
  }
}
