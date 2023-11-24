namespace Cephei.Files.INI
{
  /// <summary>
  /// The IniManager class is used to manage a collection of Ini objects, instantiating and disposing them as necessary.
  /// </summary>
  public class IniManager : FileManager<Ini>
  {
    /// <summary>
    /// Creates a new IniManager object, adding multiple ini objects to it.
    /// </summary>
    /// <param name="inis">Collection of ini objects to add under this manager.</param>
    public IniManager(params Ini[] inis) : base(inis)
    { }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Creates a new ini from the target path.
    /// </summary>
    /// <param name="path">The ini's path.</param>
    /// <param name="read">Read the file upon creation?</param>
    /// <returns>The new ini object.</returns>
    protected override Ini CreateFileObject(string path, bool read) => new Ini(path, read);
  }
}