using System;
using System.Collections.Generic;

using Cephei.Collections;

namespace Cephei.Files.DA
{
  /// <summary>
  /// The DaRow is the object that composes the 2da file's row.
  /// </summary>
  public class DaRow : ProtectedList<string>, IDaObject, IList<string>
  {
    /// <summary>
    /// Adds a 2da row to a file.
    /// </summary>
    /// <param name="file">File to add the row under.</param>
    /// <param name="content">The row's content. It is limited to the file's column count.</param>
    public DaRow(DaFile file, params string[] content) : base(new List<string>())
    {
      file.Add(this);
      SetUpRow(file, content);
    }
    /// <summary>
    /// Inserts a new 2da row into the file.
    /// </summary>
    /// <param name="file">File to insert the row under.</param>
    /// <param name="index">Index to insert the row.</param>
    /// <param name="content">The row's content. It is limited to the file's column count.</param>
    public DaRow(DaFile file, int index, params string[] content) : base(new List<string>())
    {
      file.Insert(index, this);
      SetUpRow(file, content);
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Reference to this row's file. Can be null if it is an orphan (removed) row.
    /// </summary>
    public DaFile? File { internal set; get; }

    /// <summary>
    /// Is the row read only? Returns false by default.
    /// </summary>
    public bool IsReadOnly => false;

    void IList<string>.Insert(int index, string item) => throw new NotSupportedException();
    void IList<string>.RemoveAt(int index) => throw new NotSupportedException();
    void ICollection<string>.Add(string item) => throw new NotSupportedException();
    void ICollection<string>.Clear() => throw new NotSupportedException();
    bool ICollection<string>.Remove(string item) => throw new NotSupportedException();

    //
    // PUBLIC
    //

    // METHODS

    /// <summary>
    /// Sets or gets the row's value at a certain column index.
    /// </summary>
    /// <param name="index">The column's index.</param>
    /// <returns>The row's value at the column index.</returns>
    public new string this[int index]
    {
      set => Collection[index] = value;
      get => Collection[index];
    }

    /// <summary>
    /// Copies a column from another row with the same column name.
    /// </summary>
    /// <param name="column">The column's name.</param>
    /// <param name="copyfrom">DaRow to get the value from.</param>
    /// <param name="filed">The DaRow file's column dictionary.</param>
    /// <param name="fromd">The other DaRow file's column dictionary.</param>
    /// <returns>The column's name.</returns>
    public string CopyFrom(string column, DaRow copyfrom, IReadOnlyDictionary<string, int> filed, IReadOnlyDictionary<string, int> fromd)
      => this[filed[column]] = copyfrom[fromd[column]];
    /// <summary>
    /// Copies all of the row's column values from another row.
    /// </summary>
    /// <param name="copyfrom">DaRow to get the values from.</param>
    /// <param name="filed">The DaRow file's column dictionary.</param>
    /// <param name="fromd">The other DaRow file's column dictionary.</param>
    public void CopyFrom(DaRow copyfrom, IReadOnlyDictionary<string, int> filed, IReadOnlyDictionary<string, int> fromd)
    {
      foreach (string key in filed.Keys) CopyFrom(key, copyfrom, filed, fromd);
    }

    /// <summary>
    /// Checks if the row is empty, containing only empty/null strings.
    /// </summary>
    /// <returns>True if the row has no data.</returns>
    public bool IsEmpty()
    {
      for (int i = 0; i < Count; i++)
      {
        if (!string.IsNullOrWhiteSpace(this[i])) return false;
      }
      return true;
    }

    //
    // INTERNAL
    //

    // METHODS

    internal void Add(string col) => Collection.Add(col);

    internal void Insert(int index, string col) => Collection.Insert(index, col);

    internal void Remove(int index) => Collection.RemoveAt(index);

    internal void Clear() => Collection.Clear();

    //
    // PRIVATE
    //

    // METHODS

    private void SetUpRow(DaFile file, params string[] content)
    {
      File = file;
      int c = file.Columns.Count;
      int l = content.Length;
      int i;
      for (i = 0; i < l; i++)
      {
        if (i == c) return;
        Collection.Add(content[i]);
      }
      for (; i < c; i++) Collection.Add("");
    }
  }
}
