using System.IO;
using System.Collections.Generic;
using System.Text;
using Cephei.Collections;

namespace Cephei.Files.INI
{
  /// <summary>
  /// The IniReader class is the primary reader used for importing data from the .ini file to the class.
  /// </summary>
  public class IniReader : IFileReader<Ini>
  {
    //
    // OVERRIDES
    //

    /// <summary>
    /// Reads the file to its end. Note that the file will NOT be closed once it is read. Sections and key/value data will be added to its connected Ini object.
    /// </summary>
    public virtual void Read(Ini ini)
    {
      using StreamReader reader = new StreamReader(ini.FilePath);
      bool insec = false, inkey = true, skip = false;
      StringBuilder value = new StringBuilder();
      IDictionary<string, string>? sect = null;
      string key = "";
      ushort ln = 1;
      string section;
      char cha;
      int chr = reader.Read();
      while (chr >= 0)
      {
        switch (chr)
        {
          case 10:
            insec = false;
            inkey = true;
            skip = false;
            AssignData(ini, sect, key, GetAndReset(value), ln);
            key = "";
            ln++;
            break;
          case 59: skip = true; break;
          case 61:
            if (skip) break;
            if (!inkey) AddChar(value, chr);
            else key = GetAndReset(value);
            break;
          case 91:
            if (skip) break;
            if (inkey)
            {
              insec = true;
              value.Clear();
              break;
            }
            AddChar(value, chr);
            break;
          case 93:
            if (skip) break;
            if (insec)
            {
              section = GetAndReset(value);
              if (section.Equals("")) InvalidSectionName(ini, section, ln);
              else sect = ini.GetOrAddSection(section);
              insec = false;
              skip = true;
              break;
            }
            AddChar(value, chr);
            break;
          default:
            if (skip) break;
            cha = (char)chr;
            if (!char.IsControl(cha)) value.Append(cha);
            break;
        }
        chr = reader.Read();
      }
      if (!key.Equals("")) AssignData(ini, sect, key, GetAndReset(value), ln);
    }

    //
    // PUBLIC
    //

    //
    // PROTECTED
    //

    // METHODS

    /// <summary>
    /// What to do when a KeyValuePair is inserted into a null section.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected virtual void SectionDoesNotExist(Ini ini, string key, string value, ushort ln)
    { }

    /// <summary>
    /// What to do when a section name is invalid.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="name">The attempted section name.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected virtual void InvalidSectionName(Ini ini, string name, ushort ln)
    { }

    /// <summary>
    /// What to do when a value is added to an invalid key.
    /// </summary>
    /// <param name="ini">Reference to the ini object.</param>
    /// <param name="key">The attempted key name.</param>
    /// <param name="value">Value that was meant to be added to the key.</param>
    /// <param name="ln">Line in the file in which the issue occurred.</param>
    protected virtual void InvalidKeyName(Ini ini, string key, string value, ushort ln)
    { }

    //
    // PRIVATE
    //

    // METHODS

    private string GetAndReset(StringBuilder value)
    {
      string stx = value.ToString();
      value.Clear();
      return stx;
    }

    private void AddChar(StringBuilder value, int chr) => value.Append((char)chr);

    private void AssignData(Ini ini, IDictionary<string, string>? sect, string key, string value, ushort ln)
    {
      if (sect is null)
      {
        SectionDoesNotExist(ini, key, value, ln);
        return;
      }
      key = key.Trim();
      value = value.Trim();
      if (key.Equals(""))
      {
        if (value.Equals("")) return;
        InvalidKeyName(ini, key, value, ln);
        return;
      }
      sect.AddOrSet(key, value.Trim());
    }
  }
}
