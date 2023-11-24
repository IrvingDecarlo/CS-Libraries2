using System.IO;
using System.Collections.Generic;

namespace Cephei.Files.DA
{
  /// <summary>
  /// The Fast Writer saves the 2da's data without formatting, saving processing time.
  /// </summary>
  public class DaFastWriter : IFileWriter<DaFile>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Writes the 2da's data.
    /// </summary>
    /// <param name="file">2da file object.</param>
    public void Write(DaFile file)
    {
      using (StreamWriter writer = new StreamWriter(file.FilePath))
      {
        writer.WriteLine(file.Header);
        WriteRow(writer, "", file.Columns);
        for (int i = 0; i < file.Count; i++) WriteRow(writer, i.ToString(), file[i]);
      }
    }

    //
    // PRIVATE
    //

    // METHODS

    private void WriteRow(StreamWriter writer, string srow, IReadOnlyList<string> row)
    {
      writer.WriteLine();
      writer.Write(srow + " " + string.Join(" ", row));
    }
  }
}
