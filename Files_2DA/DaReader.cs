using System.IO;
using System.Text;

using Cephei.Collections;

namespace Cephei.Files.DA
{
  /// <summary>
  /// The DaReader is the base reader for 2da files.
  /// </summary>
  public class DaReader : IFileReader<DaFile>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Reads the 2da file's content into a DaFile.
    /// </summary>
    /// <param name="file">The DaFile object to receive the 2da's content.</param>
    /// <remarks>The DaFile's content is cleared when the reading process is initiated and the header is overwritten.</remarks>
    public void Read(DaFile file)
    {
      file.Clear();
      StringBuilder builder = new StringBuilder();
      int i = 0;
      ushort line = 1;
      bool readcol = true;
      bool reading = false;
      bool skip = false;
      DaRow? row = null;
      char cha;
      using StreamReader reader = new StreamReader(file.FilePath);
      file.Header = reader.ReadLine().Trim();
      int chr = reader.Read();
      while (chr > -1)
      {
        switch (chr)
        {
          case 10:
            line++;
            if (reading) i = CloseField(file, builder, ref row, i, readcol, ref reading, line);
            if (i < 1) break;
            if (readcol) readcol = false;
            skip = true;
            row = null;
            i = 0;
            break;
          case 32:
          case 9:
            if (skip)
            {
              skip = false;
              break;
            }
            if (!reading) break;
            i = CloseField(file, builder, ref row, i, readcol, ref reading, line);
            break;
          default:
            if (skip) break;
            cha = (char)chr;
            if (char.IsControl(cha)) break;
            builder.Append(cha);
            reading = true;
            break;
        }
        chr = reader.Read();
      }
    }

    //
    // PROTECTED
    //

    // METHODS

    /// <summary>
    /// What to do when too many columns are attempted to be inserted into a row.
    /// </summary>
    /// <param name="file">Reference to the file object.</param>
    /// <param name="text">The field's original text.</param>
    /// <param name="count">The row's maximum size.</param>
    /// <param name="col">The column index that was attempted to be inserted.</param>
    /// <param name="line">The file's line.</param>
    protected virtual void TooManyColumns(DaFile file, string text, int count, int col, ushort line)
    { }

    //
    // PRIVATE
    //

    // METHODS

    private int CloseField(DaFile file, StringBuilder builder, ref DaRow? row, int index, bool readcol, ref bool reading, ushort line)
    {
      string str = builder.ToString();
      if (str.Equals(file.EmptyField)) str = "";
      AddFieldToFile(file, str, ref row, index, readcol, line);
      builder.Clear();
      reading = false;
      return index + 1;
    }

    private void AddFieldToFile(DaFile file, string str, ref DaRow? row, int index, bool readcol, ushort line)
    {
      if (readcol)
      {
        file.Columns.AddOrSet(index, str, "");
        return;
      }
      row ??= new DaRow(file);
      int c = row.Count;
      if (index >= c)
      {
        TooManyColumns(file, str, c, index, line);
        return;
      }
      row[index] = str;
    }
  }
}
