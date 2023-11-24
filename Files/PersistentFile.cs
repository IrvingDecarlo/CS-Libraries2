using System;
using System.IO;

namespace Cephei.Files
{
  /// <summary>
  /// The PersistentFile class is the abstract implementation of IPersistentFile.
  /// </summary>
  public abstract class PersistentFile : IPersistentFile
  {
    /// <summary>
    /// Creates a new PersistentFile, setting a path reference, setting the file's extension to a certain value.
    /// Throws exception if the target path does not contain a valid file name without extension.
    /// </summary>
    /// <param name="path">The path the file will be using.</param>
    /// <param name="ext">The file extension to use.</param>
    /// <param name="read">Read the file upon instantiation?</param>
    /// <exception cref="ArgumentException"></exception>
    protected PersistentFile(string path, string ext, bool read) : this(path, ext)
    {
      if (read) Read();
    }
    /// <summary>
    /// Creates a new PersistentFile, setting a path reference, setting the file's extension to a certain value.
    /// Throws exception if the target path does not contain a valid file name without extension.
    /// </summary>
    /// <param name="path">The path the file will be using.</param>
    /// <param name="ext">The file extension to use.</param>
    /// <exception cref="ArgumentException"></exception>
    protected PersistentFile(string path, string ext)
    {
      CheckFileName(path);
      if (!Path.GetExtension(path).Equals(ext)) path = Path.ChangeExtension(path, ext);
      FilePath = path;
    }
    /// <summary>
    /// Creates a new PersistentFile, setting a path reference.
    /// Throws exception if the target path does not contain a valid file name without extension.
    /// </summary>
    /// <param name="path">The path the file will be using.</param>
    /// <param name="read">Read the file upon instantiation?</param>
    /// <exception cref="ArgumentException"></exception>
    protected PersistentFile(string path, bool read) : this(path)
    {
      if (read) Read();
    }
    /// <summary>
    /// Creates a new PersistentFile, setting a path reference.
    /// Throws exception if the target path does not contain a valid file name without extension.
    /// </summary>
    /// <param name="path">The path the file will be using.</param>
    /// <exception cref="ArgumentException"></exception>
    protected PersistentFile(string path)
    {
      CheckFileName(path);
      FilePath = path;
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Checks if the two file classes are equal, based on their FilePaths.
    /// </summary>
    /// <param name="file">The file to compare.</param>
    /// <returns>True if both files have the same paths.</returns>
    public bool Equals(IPersistentFile file) => FilePath.Equals(file.FilePath);
    /// <summary>
    /// Checks if this file object is equal to another object.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>True if both are PersistentFiles and have the same path.</returns>
    public override bool Equals(object obj)
    {
      if (obj is IPersistentFile persistent) return Equals(persistent);
      return false;
    }

    /// <summary>
    /// Returns this file's path.
    /// </summary>
    /// <returns>Returns the file's path.</returns>
    public override string ToString() => FilePath;

    /// <summary>
    /// Gets this file's hash code, based on its FilePath.
    /// </summary>
    /// <returns>The HashCode based on its FilePath.</returns>
    public override int GetHashCode() => FilePath.GetHashCode();

    /// <summary>
    /// The class' file path.
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// Reads the file's content to the object.
    /// </summary>
    public abstract void Read();

    /// <summary>
    /// Outputs the object's content to the file.
    /// </summary>
    public abstract void Write();

    //
    // PUBLIC
    //

    //
    // PRIVATE
    //

    // METHODS

    private void CheckFileName(string path)
    {
      if (Path.GetFileNameWithoutExtension(path).Equals(string.Empty)) throw new ArgumentException($"Path '{path}' does not point to a valid file.", "path");
    }
  }
}
