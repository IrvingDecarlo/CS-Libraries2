using System.Collections.Generic;
using System.IO;

namespace Cephei.Files
{
  /// <summary>
  /// The AssemblyExtensions class contains extension methods for files and assemblies.
  /// </summary>
  public static class FileExtensions
  {
    /// <summary>
    /// Gets and creates the path to the directory, adding the properly sided slash when returning the full path.
    /// </summary>
    /// <param name="path">Path to the directory.</param>
    /// <returns>Path to the directory with a slash.</returns>
    public static string GetAndCreateDirectoryPath(this string path)
    {
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
      return path + Path.DirectorySeparatorChar;
    }

    /// <summary>
    /// Checks if the file object's file exists.
    /// </summary>
    /// <param name="file">The IPersistentFile object.</param>
    /// <returns>True if the object's file exists.</returns>
    public static bool FileExists(this IPersistentFile file) => File.Exists(file.FilePath);

    /// <summary>
    /// Reads all the files' content to the objects defined by the collection.
    /// </summary>
    /// <typeparam name="T">Any IPersistentFile object.</typeparam>
    /// <param name="files">Collection of Persistent Files to read.</param>
    public static void ReadAll<T>(this IEnumerable<T> files) where T : IReadableFile
    {
      foreach (T file in files) file.Read();
    }
    /// <summary>
    /// Reads the file's content into its object using multiple readers.
    /// </summary>
    /// <typeparam name="T">The reader type.</typeparam>
    /// <typeparam name="U">The file object type.</typeparam>
    /// <param name="readers">Collection of readers to use.</param>
    /// <param name="file">File to receive the readers' content.</param>
    public static void ReadAll<T, U>(this IEnumerable<T> readers, U file) where T : IFileReader<U>
    {
      foreach (T reader in readers) reader.Read(file);
    }
    /// <summary>
    /// Reads the content of multiple file objects using a single reader.
    /// </summary>
    /// <typeparam name="T">The reader type.</typeparam>
    /// <typeparam name="U">The file object type.</typeparam>
    /// <param name="files">Collection of files to read.</param>
    /// <param name="reader">Reader to use.</param>
    public static void ReadAll<T, U>(this IEnumerable<U> files, T reader) where T : IFileReader<U>
    {
      foreach (U file in files) reader.Read(file);
    }

    /// <summary>
    /// Writes all the objects' content to their files.
    /// </summary>
    /// <typeparam name="T">Any IPersistentFile object.</typeparam>
    /// <param name="files">Collection of Persistent Files to write.</param>
    public static void WriteAll<T>(this IEnumerable<T> files) where T : IWriteableFile
    {
      foreach (T file in files) file.Write();
    }
    /// <summary>
    /// Writes the file's content into files using multiple writers.
    /// </summary>
    /// <typeparam name="T">The writer type.</typeparam>
    /// <typeparam name="U">The file object type.</typeparam>
    /// <param name="writers">Collection of writers to use.</param>
    /// <param name="file">File object to output the data from.</param>
    public static void WriteAll<T, U>(this IEnumerable<T> writers, U file) where T : IFileWriter<U>
    {
      foreach (T writer in writers) writer.Write(file);
    }
    /// <summary>
    /// Writes the content of multiple files using a single writer.
    /// </summary>
    /// <typeparam name="T">The writer type.</typeparam>
    /// <typeparam name="U">The file object type.</typeparam>
    /// <param name="files">Collection of files to write.</param>
    /// <param name="writer">Writer to use.</param>
    public static void WriteAll<T, U>(this IEnumerable<U> files, T writer) where T : IFileWriter<U>
    {
      foreach (U file in files) writer.Write(file);
    }

    /// <summary>
    /// Performs the reader's Read method, outputting a new list of exceptions.
    /// </summary>
    /// <typeparam name="T">The file object type that the reader uses.</typeparam>
    /// <param name="reader">The reader.</param>
    /// <param name="file">File to be read.</param>
    /// <param name="excs">List of outputted exceptions.</param>
    public static void Read<T>(this IFileExceptionReader<T> reader, T file, out List<FileReadException> excs)
    {
      excs = new List<FileReadException>();
      reader.Read(file, excs);
    }
    /// <summary>
    /// Reads the file, outputting a new list of exceptions.
    /// </summary>
    /// <param name="file">File to be read.</param>
    /// <param name="excs">List of outputted exceptions.</param>
    public static void Read(this IReadableExceptionFile file, out List<FileReadException> excs)
    {
      excs = new List<FileReadException>();
      file.Read(excs);
    }

    /// <summary>
    /// Creates the necessary directories for the defined path.
    /// </summary>
    /// <param name="path">Path to the desired file.</param>
    public static void CreateDirectoryForPath(this string path)
    {
      path = Path.GetDirectoryName(path);
      if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
  }
}
