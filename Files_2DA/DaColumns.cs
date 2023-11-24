using System.Collections;
using System.Collections.Generic;

namespace Cephei.Files.DA
{
  /// <summary>
  /// DaColumns is the class where the file's 2DA columns are saved in.
  /// </summary>
  public class DaColumns : IDaObject, IList<string>, IReadOnlyList<string>
  {
    internal DaColumns(DaFile file) => File = file;

    //
    // OVERRIDES
    //

    /// <summary>
    /// Reference to the columns' file.
    /// </summary>
    public DaFile File { get; private set; }

    /// <summary>
    /// Gets the index of a column by its name.
    /// </summary>
    /// <param name="col">The column's name to look for.</param>
    /// <returns>The column's index.</returns>
    public int IndexOf(string col) => columns.IndexOf(col);

    /// <summary>
    /// Inserts a column into the file, inserting an empty string to all of the file's rows at that specific index.
    /// </summary>
    /// <param name="index">Index to insert the column at.</param>
    /// <param name="col">The column's name.</param>
    public void Insert(int index, string col)
    {
      columns.Insert(index, col);
      DaFile file = File;
      for (int i = 0; i < file.Count; i++) file[i].Insert(index, "");
    }

    /// <summary>
    /// Removes the column at the specified index, removing it from the file's rows.
    /// </summary>
    /// <param name="index">The column's index to remove.</param>
    public void RemoveAt(int index)
    {
      columns.RemoveAt(index);
      DaFile file = File;
      for (int i = 0; i < file.Count; i++) file[i].Remove(index);
    }

    /// <summary>
    /// Gets or sets the column's name at a specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    public string this[int index]
    {
      set => columns[index] = value;
      get => columns[index];
    }

    /// <summary>
    /// Adds a column to the file, adding an empty entry to all of the file's rows.
    /// </summary>
    /// <param name="col">The new column's name.</param>
    public void Add(string col)
    {
      columns.Add(col);
      DaFile file = File;
      for (int i = 0; i < file.Count; i++) file[i].Add("");
    }

    /// <summary>
    /// Clears all of the file's columns, removing all of the row's content as well (does not remove the rows entirely, just their content).
    /// </summary>
    public void Clear()
    {
      columns.Clear();
      DaFile file = File;
      for (int i = 0; i < file.Count; i++) file[i].Clear();
    }

    /// <summary>
    /// Checks if a certain column exists.
    /// </summary>
    /// <param name="col">The column's name to look for.</param>
    /// <returns>True if the column exists.</returns>
    public bool Contains(string col) => columns.Contains(col);

    /// <summary>
    /// Copies the file's column names to an array. Does not copy the rows.
    /// </summary>
    /// <param name="array">The array to receive the columns.</param>
    /// <param name="i">Starting index.</param>
    public void CopyTo(string[] array, int i) => columns.CopyTo(array, i);

    /// <summary>
    /// Removes a column from the file, removing the column's content from the rows as well.
    /// </summary>
    /// <param name="col">The column to remove.</param>
    /// <returns>True if the column exists and it was removed.</returns>
    public bool Remove(string col)
    {
      int i = IndexOf(col);
      if (i < 0) return false;
      RemoveAt(i);
      return true;
    }

    /// <summary>
    /// Returns the number of columns.
    /// </summary>
    public int Count => columns.Count;

    /// <summary>
    /// Are the columns read only? Returns false by default.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Returns the enumerator for the file's columns.
    /// </summary>
    /// <returns>The enumerator for the file's columns.</returns>
    public IEnumerator<string> GetEnumerator() => columns.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    //
    // PUBLIC
    //

    // METHODS

    /// <summary>
    /// Gets a dictionary containing all columns' names referencing their respective indexes.
    /// </summary>
    /// <returns>The columns' dictionary.</returns>
    public Dictionary<string, int> GetDictionary()
    {
      Dictionary<string, int> dict = new Dictionary<string, int>();
      GetDictionary(dict);
      return dict;
    }
    /// <summary>
    /// Feeds a dictionary with the columns' names referencing their respective indexes.
    /// </summary>
    /// <param name="dict">Dictionary to receive the new data.</param>
    public void GetDictionary(IDictionary<string, int> dict)
    {
      for (int i = 0; i < Count; i++) dict.Add(this[i], i);
    }

    /// <summary>
    /// Returns the lengths of all columns.
    /// </summary>
    /// <param name="space">Extra spacing for the columns, use 0 to get the columns' precise length.</param>
    /// <param name="minlen">The columns' minimum length.</param>
    /// <returns>The lengths of all columns.</returns>
    public int[] GetLengths(int space = 0, int minlen = 0)
    {
      int c = Count;
      int[] lens = new int[c];
      for (int i = 0; i < c; i++) lens[i] = GetLength(i, minlen) + space;
      return lens;
    }

    /// <summary>
    /// Gets a column's length, taking into consideration its own and the row's string lengths.
    /// </summary>
    /// <param name="index">The column's index.</param>
    /// <param name="minlen">The column's minimum acceptable length.</param>
    /// <returns>The column's string length.</returns>
    public int GetLength(int index, int minlen = 0)
    {
      int l = this[index].Length;
      DaFile file = File;
      int len;
      for (int i = 0; i < file.Count; i++)
      {
        len = file[i][index].Length;
        if (len > l) l = len;
      }
      return l < minlen ? minlen : l;
    }

    //
    // PRIVATE
    //

    private readonly List<string> columns = new List<string>();
  }
}
