using System.IO;
using System.Collections.Generic;

using Cephei.Collections;

namespace Cephei.Files.DA
{
  /// <summary>
  /// DaFile is the core of the 2da file.
  /// </summary>
  public class DaFile : ProtectedList<DaRow>, IPersistentFile, IReadableExceptionFile
  {
    /// <summary>
    /// Creates a new 2DA file object, populating the object with the file's columns and rows if read is true.
    /// </summary>
    /// <param name="path">Path to the 2da file.</param>
    /// <param name="read">Read the file, populating the object with its content?</param>
    /// <param name="header">The 2da's header string.</param>
    /// <param name="empty">The file's identifier for empty fields when reading and writing.</param>
    /// <param name="ext">The 2da file's custom extension.</param>
    public DaFile(string path, bool read, string header = "2DA V2.0", string empty = "****", string ext = "2da") : this(path, header, empty, ext)
    {
      if (read) Read();
    }
    /// <summary>
    /// Creates a new 2DA file object. It does not prepare the object's columns based on the file, use the constructor with the read parameter for that./>
    /// </summary>
    /// <param name="path">The 2da's file path.</param>
    /// <param name="header">The 2da's header string.</param>
    /// <param name="empty">The file's identifier for empty fields when reading and writing.</param>
    /// <param name="ext">The 2da file's custom extension.</param>
    public DaFile(string path, string header = "2DA V2.0", string empty = "****", string ext = "2da") : base()
    {
      FilePath = Path.ChangeExtension(path, ext);
      Columns = new DaColumns(this);
      EmptyField = empty;
      Header = header;
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Path to the 2da file.
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// Reads the file's content to the 2da, populating it with its columns and rows. Clears its previous content.
    /// </summary>
    public virtual void Read() => new DaReader().Read(this);
    /// <summary>
    /// Reads the file's content to the object, outputting exceptions when they occur.
    /// </summary>
    /// <param name="excs">List of outputted exceptions.</param>
    public virtual void Read(List<FileReadException> excs) => new DaExceptionReader().Read(this, excs);

    /// <summary>
    /// Writes the object's content to its path.
    /// </summary>
    public virtual void Write() => new DaWriter().Write(this);

    /// <summary>
    /// Checks if this file is equal to another file object, comparing both FilePaths.
    /// </summary>
    /// <param name="file">File to equate to.</param>
    /// <returns>True if both file objects are the same.</returns>
    public bool Equals(IPersistentFile file) => FilePath == file.FilePath;
    /// <summary>
    /// Checks if this object is equal to another object.
    /// </summary>
    /// <param name="obj">Object to equate to.</param>
    /// <returns>True if both objects are the same.</returns>
    public override bool Equals(object obj)
      => obj is IPersistentFile persistent && Equals(persistent);

    /// <summary>
    /// Gets the file's hash code by its file path.
    /// </summary>
    /// <returns>The FilePath's hash code.</returns>
    public override int GetHashCode() => FilePath.GetHashCode();

    //
    // PUBLIC
    //

    // VARIABLES

    /// <summary>
    /// The 2da file's columns.
    /// </summary>
    public readonly DaColumns Columns;

    /// <summary>
    /// The file's identifier for an empty field. Note that in the object, empty fields will still be "". The variable should only be used
    /// by the 2da file's writers and readers.
    /// </summary>
    public string EmptyField;

    /// <summary>
    /// The 2da's header string. May be overwritten when the file's content is read into this object using the standard reader.
    /// </summary>
    public string Header;

    // METHODS

    /// <summary>
    /// Removes a row at a certain index.
    /// </summary>
    /// <param name="index">The index to remove.</param>
    public void Remove(int index)
    {
      this[index].File = null;
      Collection.RemoveAt(index);
    }

    /// <summary>
    /// Clears all of the file's content.
    /// </summary>
    public void Clear()
    {
      for (int i = 0; i < Count; i++) this[i].File = null;
      Collection.Clear();
      Columns.Clear();
    }

    /// <summary>
    /// Copies the rows from another file, getting both files' column dictionaries.
    /// </summary>
    /// <param name="file">File to copy the rows from.</param>
    public void CopyFrom(DaFile file) => CopyFrom(file, Columns.GetDictionary(), file.Columns.GetDictionary());
    /// <summary>
    /// Copies the rows from another file, creating new rows if necessary.
    /// </summary>
    /// <param name="file">The file to copy the data from.</param>
    /// <param name="filed">The file's dictionary.</param>
    /// <param name="fromd">The other file's dictionary.</param>
    public void CopyFrom(DaFile file, IReadOnlyDictionary<string, int> filed, IReadOnlyDictionary<string, int> fromd)
    {
      int c = Count;
      DaRow row;
      for (int i = 0; i < file.Count; i++)
      {
        if (c <= i) row = new DaRow(this);
        else row = this[i];
        row.CopyFrom(file[i], filed, fromd);
      }
    }

    //
    // INTERNAL
    //

    // METHODS

    internal void Add(DaRow row) => Collection.Add(row);

    internal void Insert(int index, DaRow row) => Collection.Insert(index, row);
  }
}
