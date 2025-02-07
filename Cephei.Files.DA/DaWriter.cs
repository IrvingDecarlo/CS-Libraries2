using System.IO;
using System.Collections.Generic;
using Cephei.Numericals;

namespace Cephei.Files.DA
{
  /// <summary>
  /// 2da writers are the primary writers for 2da file objects.
  /// </summary>
  public class DaWriter : IFileWriter<DaFile>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Writes the 2da file object's content to its file.
    /// </summary>
    /// <param name="file">The DaFile object.</param>
    public void Write(DaFile file)
    {
      int[] rowids = file.Columns.GetLengths(1, file.EmptyField.Length);
      int ncol = rowids.Length - 1;
      int rowc = file.Count;
      double rowid = ((double)(rowc - 1)).GetNumberOfDigits() + 1d;
      using StreamWriter writer = new StreamWriter(file.FilePath);
      writer.WriteLine(file.Header);
      WriteRow(file, writer, "", rowid, file.Columns, rowids, ncol);
      for (int i = 0; i < rowc; i++) WriteRow(file, writer, i.ToString(), rowid, file[i], rowids, ncol);
    }

    //
    // PRIVATE
    //

    // METHODS

    private void WriteRow(DaFile file, StreamWriter writer, string srow, double rowid, IReadOnlyList<string> row, int[] rowids, int ncol)
    {
      writer.WriteLine();
      WriteField(file, writer, srow, rowid, true, true);
      for (int i = 0; i <= ncol; i++) WriteField(file, writer, row[i], rowids[i], false, i < ncol);
    }

    private void WriteField(DaFile file, StreamWriter writer, string value, double wid, bool allowblank, bool fill)
    {
      if (!allowblank && string.IsNullOrWhiteSpace(value)) value = file.EmptyField;
      writer.Write(value);
      if (!fill) return;
      for (int i = 0; i < wid - value.Length; i++) writer.Write(' ');
    }
  }
}
