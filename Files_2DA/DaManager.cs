namespace Cephei.Files.DA
{
  /// <summary>
  /// The DaManager manages multiple 2da files under it.
  /// </summary>
  public class DaManager : FileManager<DaFile>
  {
    /// <summary>
    /// Creates a new 2da file manager.
    /// </summary>
    /// <param name="files">Files to add under this manager.</param>
    public DaManager(params DaFile[] files) : this("2DA V2.0", "****", "2da", files)
    { }
    /// <summary>
    /// Creates a new DaManager object, setting custom data to it for new 2da files created under it.
    /// </summary>
    /// <param name="header">Header for the new files to use.</param>
    /// <param name="empty">Empty field identifier.</param>
    /// <param name="ext">The file's extension.</param>
    /// <param name="files">Files to add under it.</param>
    public DaManager(string header, string empty, string ext, params DaFile[] files) : base(files)
    {
      Header = header;
      EmptyField = empty;
      Extension = ext;
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Creates the file's object from path.
    /// </summary>
    /// <param name="path">Path to create the file from.</param>
    /// <param name="read">Read the file upon creation?</param>
    /// <returns>The new file.</returns>
    protected override DaFile CreateFileObject(string path, bool read) => new DaFile(path, read, Header, EmptyField, Extension);

    //
    // PUBLIC
    //

    // VARIABLES

    /// <summary>
    /// The files' default identifier for empty fields.
    /// </summary>
    /// <remarks>Is only used for files created directly by the manager.</remarks>
    public string EmptyField;

    /// <summary>
    /// The default extension for files created under this manager.
    /// </summary>
    /// <remarks>Is only used for files created directly by the manager.</remarks>
    public string Extension;

    /// <summary>
    /// The default header for files created under this manager.
    /// </summary>
    /// <remarks>Is only used for files created directly by the manager.</remarks>
    public string Header;
  }
}
