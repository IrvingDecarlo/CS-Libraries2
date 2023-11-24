using System.Collections;
using System.Collections.Generic;

using Cephei.Collections;

namespace Cephei.Files
{
  /// <summary>
  /// The FileManager class is the base class for the management of multiple IPersistentFiles.
  /// </summary>
  public abstract class FileManager<T> : IReadOnlyDictionary<string, T>, ICollection<T> where T : IPersistentFile
  {
    /// <summary>
    /// Creates a new file manager, adding multiple file objects under it.
    /// </summary>
    /// <param name="files">Collection of file objects to add under this manager.</param>
    public FileManager(params T[] files)
    {
      this.files = new Dictionary<string, T>();
      this.AddAll(files);
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Checks if a file with a specific path is managed by this manager.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>True if the file object defined by path is managed by this manager.</returns>
    public bool ContainsKey(string path) => files.ContainsKey(path);

    /// <summary>
    /// Tries go get a file object using a specific path.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <param name="file">The file object (if found).</param>
    /// <returns>True if the object is being managed by this manager.</returns>
    public bool TryGetValue(string path, out T file) => files.TryGetValue(path, out file);

    /// <summary>
    /// Gets the amount of file objects managed by this manager.
    /// </summary>
    public int Count => files.Count;

    /// <summary>
    /// Gets the file object by path.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>The file object in the aforementioned path.</returns>
    public T this[string path] => files[path];

    /// <summary>
    /// Gets the paths to all files managed by this manager.
    /// </summary>
    public IEnumerable<string> Keys => files.Keys;

    /// <summary>
    /// Gets all the file objects managed by this manager.
    /// </summary>
    public IEnumerable<T> Values => files.Values;

    /// <summary>
    /// Gets the enumerator for all the objects under this manager.
    /// </summary>
    /// <returns>This manager's enumerator.</returns>
    public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();
    IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator() => files.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Adds a file object under this manager.
    /// </summary>
    /// <param name="file">The file object to add.</param>
    public void Add(T file) => files.Add(file.FilePath, file);
    /// <summary>
    /// Adds a file object under this manager. If an object with the specified path already exists within the manager, returns it.
    /// </summary>
    /// <param name="path">The file's target path.</param>
    /// <returns>The new file object.</returns>
    public T Add(string path) => TryGetValue(path, out T file) ? file : AddFromPath(path, false);
    /// <summary>
    /// Adds a file object using a file path. If a file with the specified path already exists within the manager, returns it.
    /// </summary>
    /// <param name="path">The file's target file path.</param>
    /// <param name="read">Should the file's content be read upon instantiation/file find?</param>
    /// <returns>The new file object.</returns>
    public T Add(string path, bool read)
    {
      if (TryGetValue(path, out T file))
      {
        if (read) file.Read();
        return file;
      }
      return AddFromPath(path, read);
    }

    /// <summary>
    /// Clears all files from this manager.
    /// </summary>
    public void Clear() => files.Clear();
    /// <summary>
    /// Clears all files from this manager.
    /// </summary>
    /// <param name="write">Write their data to their files?</param>
    public void Clear(bool write)
    {
      if (write) Values.WriteAll();
      Clear();
    }

    /// <summary>
    /// Copies the file objects under this manager to an array, starting at a specific index.
    /// </summary>
    /// <param name="files">Target file object array.</param>
    /// <param name="i">Starting index.</param>
    public void CopyTo(T[] files, int i) => this.files.Values.CopyTo(files, i);

    /// <summary>
    /// Checks if this manager contains a specific file object under it.
    /// </summary>
    /// <param name="file">The file object to check.</param>
    /// <returns>True if the object is managed by this manager.</returns>
    public bool Contains(T file) => ContainsKey(file.FilePath);

    /// <summary>
    /// Removes a file object from this manager.
    /// </summary>
    /// <param name="file">The file object to remove.</param>
    /// <returns>True if the object was removed.</returns>
    public bool Remove(T file) => Remove(file.FilePath);
    /// <summary>
    /// Removes a file object from this manager.
    /// </summary>
    /// <param name="path">The file's path.</param>
    /// <returns>True if the object was removed.</returns>
    public bool Remove(string path) => files.Remove(path);
    /// <summary>
    /// Removes a file object from this manager.
    /// </summary>
    /// <param name="path">The file's path.</param>
    /// <param name="write">Save the object's data to its file?</param>
    /// <returns>True if the object was removed.</returns>
    public bool Remove(string path, bool write)
    {
      if (!TryGetValue(path, out T file)) return false;
      if (write) file.Write();
      files.Remove(path);
      return true;
    }

    /// <summary>
    /// Is this manager ReadOnly? Returns false by default.
    /// </summary>
    public bool IsReadOnly => false;

    //
    // PROTECTED
    //

    // METHODS

    /// <summary>
    /// Creates a new file object from path. Override this with a proper creation of the manager's target type.
    /// </summary>
    /// <param name="path">The file's path.</param>
    /// <param name="read">Read the file upon creation?</param>
    /// <returns>The new file object.</returns>
    protected abstract T CreateFileObject(string path, bool read);

    //
    // PRIVATE
    //

    // VARIABLES

    private readonly Dictionary<string, T> files;

    // METHODS

    private T AddFromPath(string path, bool read)
    {
      T file = CreateFileObject(path, read);
      Add(file);
      return file;
    }
  }
}