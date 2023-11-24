using System.IO;
using System.Collections.Generic;
using Cephei.Streams;

namespace Cephei.Files.INI
{
  /// <summary>
  /// The IniWriter class is the primary writer for the ini object.
  /// </summary>
  public class IniWriter : IFileWriter<Ini>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Writes the ini's content.
    /// </summary>
    public void Write(Ini ini)
    {
      using (StreamWriter writer = new StreamWriter(ini.FilePath)) writer.WriteLine<string, string, string, IDictionary<string, string>>(ini);
    }
  }
}
