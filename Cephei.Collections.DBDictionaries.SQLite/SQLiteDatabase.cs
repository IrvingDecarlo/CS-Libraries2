using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Cephei.Collections.DBDictionaries.SQLite
{
  /// <summary>
  /// The SQLitevalueDatabase is a non read-only version of the SQLite value database.
  /// </summary>
  /// <typeparam name="T">Type used for defining the values' IDs.</typeparam>
  /// <typeparam name="U">Type used for the database's values.</typeparam>
  public abstract class SQLiteDatabase<T, U> : SQLiteReadOnlyDatabase<T, U>, IDBDictionary<T, U>
  {
    /// <summary>
    /// Creates a new SQLitevalueDatabase object.
    /// </summary>
    /// <param name="dbpath">Path to the database.</param>
    public SQLiteDatabase(string dbpath) : base(dbpath) 
    { }

    #region overrides

    /// <summary>
    /// Is the database read-only? Returns false by default.
    /// </summary>
    /// <remarks>Setting this to true will disable writing operations.</remarks>
    public override bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets a specified ID's value.
    /// </summary>
    /// <param name="id">value ID to update or get.</param>
    /// <returns>The value attached to the aforementioned ID.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public virtual new U this[T id]
    {
      set
      {
        HandleReadOnly();
        using SQLiteCommand cmd = Connection.CreateCommand();
        cmd.CommandText = $"update {GetTable()} set {GetColumnValue()} = :value where {GetColumnID()} = :id";
        SQLiteParameterCollection parms = cmd.Parameters;
        parms.AddWithValue("value", value);
        parms.AddWithValue("id", id);
        if (cmd.ExecuteNonQuery() < 1) throw new KeyNotFoundException($"A value with the ID '{id}' does not exist.");
      }
      get => base[id];
    }

    /// <summary>
    /// Adds a value to the database.
    /// </summary>
    /// <param name="item">The KeyValuePair defining the value to be added.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(KeyValuePair<T, U> item) => Add(item.Key, item.Value);
    /// <summary>
    /// Adds a new value to the database. It is set to not cache.
    /// </summary>
    /// <param name="id">The new value's ID.</param>
    /// <param name="value">The value's text.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(T id, U value) => Add(id, value, false);
    /// <summary>
    /// Adds a new value to the database.
    /// </summary>
    /// <param name="id">The new value's ID.</param>
    /// <param name="value">The value's text.</param>
    /// <param name="cache">Should it be cached?</param>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Add(T id, U value, bool cache)
    {
      HandleReadOnly();
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"insert into values ({GetColumnID()}, {GetColumnValue()}, {GetColumnCache()}) values (:id, :value, :cache)";
      FillAddParameters(cmd, id, value, cache);
      cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Removes an item from the database. Only the Key component is used.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    /// <returns>True if the item was found and removed.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool Remove(KeyValuePair<T, U> item) => Remove(item.Key);
    /// <summary>
    /// Removes a value from the database.
    /// </summary>
    /// <param name="id">The value's ID.</param>
    /// <returns>True if the value was found and removed.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual bool Remove(T id)
    {
      HandleReadOnly();
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"delete from {GetTable()} where {GetColumnID()} = :id";
      cmd.Parameters.AddWithValue("id", id);
      return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Clears the value table.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void Clear()
    {
      HandleReadOnly();
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"delete from {GetTable()}";
      cmd.ExecuteNonQuery();
    }

    ICollection<T> IDictionary<T, U>.Keys => throw new NotSupportedException();

    ICollection<U> IDictionary<T, U>.Values => throw new NotSupportedException();

    #endregion

    #region public

    /// <summary>
    /// Inserts or updates a value.
    /// </summary>
    /// <param name="id">The value's ID.</param>
    /// <param name="value">The value's text.</param>
    /// <param name="cache">Should it be cached?</param>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual void AddOrUpdate(T id, U value, bool cache)
    {
      HandleReadOnly();
      using SQLiteCommand cmd = Connection.CreateCommand();
      cmd.CommandText = $"insert or replace into values({GetColumnID()}, {GetColumnValue()}, {GetColumnCache()}) values (:id, :value, :cache)";
      FillAddParameters(cmd , id, value, cache);
      cmd.ExecuteNonQuery();
    }

    #endregion

    #region private

    private void HandleReadOnly()
    {
      if (IsReadOnly) throw new InvalidOperationException("The database is read-only - write operations cannot be performed.");
    }

    private void FillAddParameters(SQLiteCommand cmd, T id, U value, bool cache)
    {
      SQLiteParameterCollection parms = cmd.Parameters;
      parms.AddWithValue("id", id);
      parms.AddWithValue("value", value);
      parms.AddWithValue("cache", cache);
    }

    #endregion
  }
}
