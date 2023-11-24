using Cephei.Collections.SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Cephei.Collections.DBDictionaries.SQLite
{
  /// <summary>
  /// The SQLiteReadOnlyDatabase is an abstract SQLite implementation for read-only SQLite databases.
  /// </summary>
  /// <typeparam name="T">Type to be used for the database's keys.</typeparam>
  /// <typeparam name="U">Type used for the database's values.</typeparam>
  public abstract class SQLiteReadOnlyDatabase<T, U> : BasicReadOnlyDatabase<T, U>
  {
    /// <summary>
    /// Creates a new SQLite value database connection.
    /// </summary>
    /// <param name="dbpath">Path (with extension) to the database file.</param>
    public SQLiteReadOnlyDatabase(string dbpath)
    {
      Connection = new SQLiteConnection("URI=" + dbpath);
      Connection.Open();
    }

    #region overrides

    /// <summary>
    /// Disposes of this SQLite connection.
    /// </summary>
    public override void Dispose() => Connection.Dispose();

    /// <summary>
    /// Returns true if a value with the specified ID exists in the database.
    /// </summary>
    /// <param name="key">ID to look for.</param>
    /// <returns>True if a value with the specified ID exists in the database.</returns>
    public override bool ContainsKey(T key)
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"select exists(select 1 from {GetTable()} where {GetColumnID()} = :id)";
      cmd.Parameters.AddWithValue("id", key);
      using SQLiteDataReader reader = cmd.ExecuteReader();
      reader.Read();
      return reader.GetBoolean(0).Equals(true);
    }

    /// <summary>
    /// Gets the amount of items in the database.
    /// </summary>
    public override int Count
    {
      get
      {
        using SQLiteCommand cmd = Connection.CreateCommand();
        cmd.CommandText = $"select count(1) from {GetTable()}";
        using SQLiteDataReader reader = cmd.ExecuteReader();
        reader.Read();
        return reader.GetInt32(0);
      }
    }

    /// <summary>
    /// Tries to get a value out of this database.
    /// </summary>
    /// <param name="key">Key to be used.</param>
    /// <param name="value">Value (default if not present).</param>
    /// <returns>True if the value was found and returned.</returns>
    public override bool TryGetValue(T key, out U value)
    {
      IDictionary<T, U> dict = GetDictionary();
      if (dict.TryGetValue(key, out value)) return true;
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"select {GetColumnValue()}, {GetColumnCache()} from {GetTable()} where {GetColumnID()} = :id";
      cmd.Parameters.AddWithValue("id", key);
      using SQLiteDataReader reader = cmd.ExecuteReader();
      if (!reader.Read())
      {
        value = default;
        return false;
      }
      value = GetValue(reader, 0);
      if (!reader.IsDBNull(1) && reader.GetBoolean(1)) dict.TryAdd(key, value);
      return true;
    }

    /// <summary>
    /// Returns an enumerator for all of the database's values and their IDs.
    /// </summary>
    /// <returns>Enumerator for the database's values and IDs.</returns>
    public override IEnumerator<KeyValuePair<T, U>> GetEnumerator()
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"select {GetColumnID()}, {GetColumnValue()} from {GetTable()}";
      SQLiteDataReader reader = cmd.ExecuteReader();
      return new SQLiteEnumeratorDelegate<KeyValuePair<T, U>>(reader, (x) => new KeyValuePair<T, U>(GetID(reader, 0), GetValue(reader, 1)));
    }

    /// <summary>
    /// Gets all of the values' IDs.
    /// </summary>
    public override IEnumerable<T> Keys => GetEnum(GetColumnID(), (x) => GetID(x, 0));

    /// <summary>
    /// Gets all of the values in the database.
    /// </summary>
    public override IEnumerable<U> Values => GetEnum(GetColumnValue(), (x) => GetValue(x, 0));

    /// <summary>
    /// Gets the value defined by the specified ID.
    /// </summary>
    /// <param name="key">The value's ID.</param>
    /// <returns>The value defined by the specified ID, "" if such value does not exist.</returns>
    public override U this[T key] => TryGetValue(key, out U value) ? value : default;

    #endregion

    #region public

    // PROPERTIES

    /// <summary>
    /// Gets the database's file name (without path or file extension).
    /// </summary>
    public string DatabaseName => Connection.DataSource;

    // METHODS

    /// <summary>
    /// Caches all cacheable values.
    /// </summary>
    public void CacheAll()
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"select {GetColumnID()}, {GetColumnValue()} from {GetTable()} where {GetColumnCache()} = :cache";
      cmd.Parameters.AddWithValue("cache", true);
      IDictionary<T, U> dict = GetDictionary();
      using SQLiteDataReader reader = cmd.ExecuteReader();
      while (reader.Read()) dict.TryAdd(GetID(reader, 0), GetValue(reader, 1));
    }

    #endregion

    #region protected

    // PROPERTIES

    /// <summary>
    /// Reference to this dictionary's connection to its database.
    /// </summary>
    protected SQLiteConnection Connection { private set; get; }

    // METHODS

    /// <summary>
    /// Gets the dictionary for caching values.
    /// </summary>
    /// <returns>The dictionary for caching values.</returns>
    protected abstract IDictionary<T, U> GetDictionary();

    /// <summary>
    /// Method to use when getting the item's ID from a reader.
    /// </summary>
    /// <param name="reader">Reader to get the ID from.</param>
    /// <param name="column">Column index among the parameters.</param>
    /// <returns>The ID.</returns>
    protected abstract T GetID(SQLiteDataReader reader, int column);

    /// <summary>
    /// Method to use when getting the item's value from a reader.
    /// </summary>
    /// <param name="reader">Reader to get the value from.</param>
    /// <param name="column">Column index among the parameters.</param>
    /// <returns>The value.</returns>
    protected abstract U GetValue(SQLiteDataReader reader, int column);

    /// <summary>
    /// Gets the table to be used to get the values from.
    /// </summary>
    /// <returns>The table's name.</returns>
    protected abstract string GetTable();

    /// <summary>
    /// Gets the ID column's name.
    /// </summary>
    /// <returns>The ID column's name.</returns>
    protected virtual string GetColumnID() => "id";

    /// <summary>
    /// Gets the value (value) column's name.
    /// </summary>
    /// <returns>The value column's name.</returns>
    protected virtual string GetColumnValue() => "value";

    /// <summary>
    /// Gets the cache column's name.
    /// </summary>
    /// <returns>The cache column's name.</returns>
    protected virtual string GetColumnCache() => "cache";

    #endregion

    #region private

    private EnumerableDelegate<V> GetEnum<V>(string column, Func<SQLiteDataReader, V> func)
    {
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"select {column} from {GetTable()}";
      SQLiteDataReader reader = cmd.ExecuteReader();
      return new EnumerableDelegate<V>(() => new SQLiteEnumeratorDelegate<V>(reader, func));
    }

    #endregion
  }
}
