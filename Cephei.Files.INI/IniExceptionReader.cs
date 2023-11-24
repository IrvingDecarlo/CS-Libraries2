using System.Collections.Generic;

namespace Cephei.Files.INI
{
  /// <summary>
  /// The IniExceptionReader class works the same as its base version, but will accumulate a list of exceptions during its reading process.
  /// </summary>
  public class IniExceptionReader : IniReader, IFileExceptionReader<Ini>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Reads the ini file, outputting exceptions to a list.
    /// </summary>
    /// <param name="ini">The ini file to read.</param>
    /// <param name="excs">Exceptions list.</param>
    public void Read(Ini ini, List<FileReadException> excs)
    {
      Exceptions = excs;
      Read(ini);
    }

    //
    // PUBLIC
    //

    // VARIABLES

    /// <summary>
    /// List of FileReadExceptions that are outputted when the Read method is called. It is populated even when the regular Read method is called.
    /// </summary>
    public List<FileReadException> Exceptions = new List<FileReadException>();

    //
    // PROTECTED
    //

    // METHODS

    /// <summary>
    /// Outputs an exception defining the issue.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="key">The attempted key name.</param>
    /// <param name="value">Value that was meant to be added to the key.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected sealed override void InvalidKeyName(Ini ini, string key, string value, ushort ln)
        => Exceptions.Add(new FileReadException<ushort>(ini.FilePath, ln, $"The key '{key}' is invalid, therefore the value '{value}' was not added."));

    /// <summary>
    /// Outputs an exception defining the issue.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="name">The attempted section name.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected sealed override void InvalidSectionName(Ini ini, string name, ushort ln)
        => Exceptions.Add(new FileReadException<ushort>(ini.FilePath, ln, $"The section name '{name}' is invalid, therefore the section was not added."));

    /// <summary>
    /// Outputs an exception defining the issue.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected sealed override void SectionDoesNotExist(Ini ini, string key, string value, ushort ln)
        => Exceptions.Add(new FileReadException<ushort>(ini.FilePath, ln
            , $"The key '{key}={value}' pair do not have a valid section, therefore they were not added."));
  }
}
