using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable enable

namespace Cephei.Tools
{
  /// <summary>
  /// Tools.Extensions serve only to extend .NET classes.
  /// </summary>
  public static class Extensions
  {
    //
    // PUBLIC
    //

    /// <summary>
    /// Reads a string and transforms it into a list of split strings, using a char separator. It also respects the "protector" chars, which are to act as quotes.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="sep">The separator char.</param>
    /// <param name="trim">Trim split strings?</param>
    /// <param name="prots">The protector chars.</param>
    /// <returns>The list of split strings.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static List<string> SplitProtected(this string input, char sep, bool trim, params char[] prots)
    {
      if (input is null || input.Equals("")) throw new ArgumentNullException("input");
      List<string> cmds = new List<string>();
      if (trim) input = input.TrimEnd();
      char init;
      int x, ilen;
      byte skip = 0;
      while (true)
      {
        if (trim) input = input.TrimStart();
        init = input[0];
        x = 0;
        ilen = input.Length;
        foreach (char prot in prots)
        {
          if (init.Equals(prot))
          {
            x = input.IndexOf(prot, 1);
            skip = 1;
            break;
          }
        }
        if (x.Equals(0))
        {
          x = input.IndexOf(sep);
          skip = 0;
        }
        if (x < 0 || x.Equals(ilen - 1))
        {
          cmds.Add(input.Substring(skip, ilen - (skip * 2)));
          break;
        }
        else
        {
          cmds.Add(input[skip..x]);
          input = input[(x + skip + 1)..];
        }
      }
      return cmds;
    }
    /// <summary>
    /// Splits the string using the default separator (blank spaces and tabs) and protectors (single and double quotes).
    /// </summary>
    /// <param name="input">The string to be separated.</param>
    /// <param name="tabsspaces">Should tabs be converted to spaces?</param>
    /// <returns>List of separated strings.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static List<string> SplitProtected(this string input, bool tabsspaces = true)
    {
      if (tabsspaces) input = input.Replace('\t', ' ');
      return input.SplitProtected(' ');
    }
    /// <summary>
    /// Splits the string using a custom separator, but with default protectors (single and double quotes).
    /// </summary>
    /// <param name="input">The string to be separated.</param>
    /// <param name="sep">The separator to be used.</param>
    /// <returns>The separated string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static List<string> SplitProtected(this string input, char sep) => input.SplitProtected(sep, true, '"', '\'');

    /// <summary>
    /// Breaks the input string into lines by length. Once the line grows to be longer than length then a line break will be introduced. It will prioritize breaking
    /// blank spaces. If none are found then the text itself will be broken.
    /// </summary>
    /// <param name="input">Input string to break.</param>
    /// <param name="length">Maximum character length per line.</param>
    /// <returns>The StringBuilder with input broken into parts.</returns>
    public static StringBuilder BreakLines(this string input, int length) => input.BreakLines(length);
    /// <summary>
    /// Breaks the StringBuilder into lines by length. Once the line grows to be longer than length then a line break will be introduced. It will prioritize breaking
    /// blank spaces. If none are found then the text itself will be broken.
    /// </summary>
    /// <param name="input">The StringBuilder.</param>
    /// <param name="length">Maximum character length per line.</param>
    /// <returns>Number of lines that were broken.</returns>
    public static int BreakLines(this StringBuilder input, int length)
    {
      int lines = 0;
      int len = 0;
      int s = 0;
      for (int i = 0; i < input.Length; i++)
      {
        switch (input[i])
        {
          case '\n':
            s = i;
            BoxLine(ref i, ref s, ref len, ref lines);
            continue;
          case ' ': case '\t': s = i; break;
        }
        len++;
        if (len >= length)
        {
          if (s > 0) input[s] = '\n';
          else
          {
            input.Insert(i, '\n');
            s = i;
            i++;
          }
          BoxLine(ref i, ref s, ref len, ref lines);
        }
      }
      return lines + 1;
    }

    /// <summary>
    /// Inserts a substring into a string, always keeping index within bounds and also considering if the string is empty or null.
    /// </summary>
    /// <param name="str">The string to receive.</param>
    /// <param name="index">Index to insert.</param>
    /// <param name="value">String to insert.</param>
    /// <returns>The string with the insertion.</returns>
    public static string SafeInsert(this string str, int index, string value)
    {
      if (string.IsNullOrEmpty(str)) return value;
      else return str.Insert(index.Clamp(str.Length, 0), value);
    }

    /// <summary>
    /// Returns a string containing the KeyValuePair, in the format "key=value".
    /// </summary>
    /// <typeparam name="T">The key type.</typeparam>
    /// <typeparam name="U">The value type.</typeparam>
    /// <param name="kvp">The KeyValuePair.</param>
    /// <param name="sep">Separator to use between the Key and the Value.</param>
    /// <returns>A string representing the KeyValuePair.</returns>
    public static string ToPairString<T, U>(this KeyValuePair<T, U> kvp, string sep = "=") => $"{kvp.Key + sep + kvp.Value}";
    /// <summary>
    /// Returns a string representing a DictionaryEntry, in the format "key=value".
    /// </summary>
    /// <param name="entry">The DictionaryEntry.</param>
    /// <param name="sep">Separator to use between the Key and the Value.</param>
    /// <returns>A string representing the DictionaryEntry.</returns>
    public static string ToPairString(this DictionaryEntry entry, string sep = "=") => $"{entry.Key + sep + entry.Value}";
    
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
    /// Clamps a double between a min and max value.
    /// </summary>
    /// <param name="val">The value to be clamped.</param>
    /// <param name="max">Max value.</param>
    /// <param name="min">Min value.</param>
    /// <returns>The clamped value.</returns>
    public static double Clamp(this double val, double max, double min)
        => val > max ? max : val < min ? min : val;
    /// <summary>
    /// Clamps an int between a min and max value.
    /// </summary>
    /// <param name="val">The value to be clamped.</param>
    /// <param name="max">Max value.</param>
    /// <param name="min">Min value.</param>
    /// <returns>The clamped value.</returns>
    public static int Clamp(this int val, int max, int min)
        => val > max ? max : val < min ? min : val;
    /// <summary>
    /// Clamps a short between a min and max value.
    /// </summary>
    /// <param name="val">The value to be clamped.</param>
    /// <param name="max">Max value.</param>
    /// <param name="min">Min value.</param>
    /// <returns>The clamped value.</returns>
    public static short Clamp(this short val, short max, short min)
        => val > max ? max : val < min ? min : val;
    /// <summary>
    /// Clamps a byte between a min and max value.
    /// </summary>
    /// <param name="val">The value to be clamped.</param>
    /// <param name="max">Max value.</param>
    /// <param name="min">Min value.</param>
    /// <returns>The clamped value.</returns>
    public static byte Clamp(this byte val, byte max, byte min)
        => val > max ? max : val < min ? min : val;

    /// <summary>
    /// Gets the number of non-decimal digits a double has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    public static double GetNumberOfDigits(this double val) => val == 0 ? 1 : Math.Abs(Math.Floor(Math.Log10(val) + 1));
    /// <summary>
    /// Gets the number of digits a convertible an int has.
    /// </summary>
    /// <param name="val">Value to get the number of digits from.</param>
    /// <returns>The value's number of digits.</returns>
    /// <remarks>This is double.GetNumberOfDigits but with the appropriate casts. Use the double's version if the value was already casted or if
    /// a double with the same value exists.</remarks>
    public static int GetNumberOfDigits(this int val) => (int)((double)val).GetNumberOfDigits();

    /// <summary>
    /// Copies a portion of a string to a character array, keeping it within bounds so that it does not overflow.
    /// </summary>
    /// <param name="str">Source string.</param>
    /// <param name="sourceindex">Starting index for the source.</param>
    /// <param name="destination">Destination character array.</param>
    /// <param name="destindex">Starting index for the destination.</param>
    /// <param name="count">Number of characters to be copied to the destination.</param>
    /// <returns>The actual number of copied characters.</returns>
    public static int SafeCopyTo(this string str, int sourceindex, char[] destination, int destindex, int count)
    {
      count = Tool.Min(count, destination.Length - destindex, str.Length - sourceindex);
      str.CopyTo(sourceindex, destination, destindex, count);
      return count;
    }

    /// <summary>
    /// Copies a portion of a string to a character span, keeping it within bounds so that it does not overflow.
    /// </summary>
    /// <param name="str">Source string.</param>
    /// <param name="span">Target span.</param>
    /// <param name="sourceindex">Starting index for the source.</param>
    /// <param name="targetindex">Starting index for the target.</param>
    /// <param name="count">Number of characters to be copied.</param>
    /// <returns>Actual number of copied characters.</returns>
    public static int CopyTo(this string str, Span<char> span, int sourceindex = 0, int targetindex = 0, int count = int.MaxValue)
    {
      count = Tool.Min(count, str.Length - sourceindex, span.Length - targetindex);
      for (int i = 0; i < count; i++) span[targetindex + i] = str[sourceindex + i];
      return count;
    }

    /// <summary>
    /// Is this char a backspace?
    /// </summary>
    /// <param name="chr">The char to check.</param>
    /// <returns>True if is a backspace, false otherwise.</returns>
    public static bool IsBackspace(this char chr) => chr.Equals('\b');

    /// <summary>
    /// Converts a string to bool using default collections.
    /// True: "TRUE", "T", "YES", "Y", "1".
    /// False: "FALSE", "F", "NO", "N", "0".
    /// </summary>
    /// <param name="str">The string to be converted.</param>
    /// <param name="handlenotfound">If the string does not match into any of the default collections, then it will return this parameter.
    /// If this parameter is set to null then an exception will be thrown instead.</param>
    /// <param name="toupper">Convert string to upper invariant?</param>
    /// <returns>The string converted to bool.</returns>
    public static bool ToBool(this string str, bool? handlenotfound = false, bool toupper = true)
        => (toupper ? str.ToUpperInvariant() : str).ToBool(new string[] { "TRUE", "T", "YES", "1", "Y" }, new string[] { "FALSE", "F", "NO", "N", "0" }, handlenotfound);
    /// <summary>
    /// Converts a value to bool.
    /// </summary>
    /// <param name="str">The value to be converted.</param>
    /// <param name="truecol">Collection of values that represent "true".</param>
    /// <param name="falsecol">Collection of values that represent "false".</param>
    /// <param name="handlenotfound">If the string does not match into any of the collections then it will return this parameter.
    /// If this parameter is set to null then an exception will be thrown instead.</param>
    /// <returns>The string converted to bool.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static bool ToBool<T>(this T str, ICollection<T> truecol, ICollection<T> falsecol, bool? handlenotfound = null)
    {
      if (truecol.Contains(str)) return true;
      else if (falsecol.Contains(str)) return false;
      else if (handlenotfound is null) throw new ArgumentException("The string was not found in any of the collections.", "str");
      else return (bool)handlenotfound;
    }

    /// <summary>
    /// A safe version of Equals where both objects are checked if they are null.
    /// </summary>
    /// <param name="this">Object that will equate to the other.</param>
    /// <param name="other">Object that will be equated by the first.</param>
    /// <returns>True if both are null or if both are equals.</returns>
    public static bool SafeEquals(this object? @this, object? other)
    {
      if (@this is null && other is null) return true;
      if (@this is null || other is null) return false;
      return @this.Equals(other);
    }

    /// <summary>
    /// Tries to append a character to the StringBuilder. Control characters are not added.
    /// </summary>
    /// <param name="sb">StringBuilder to receive the character.</param>
    /// <param name="add">Character to add to the StringBuilder.</param>
    /// <returns>True if the char was added.</returns>
    public static bool TryAppend(this StringBuilder sb, char add)
    {
      bool app = !char.IsControl(add);
      if (app) sb.Append(add);
      return app;
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

    //
    // PRIVATE
    //

    // METHODS

    private static void BoxLine(ref int i, ref int s, ref int len, ref int lines)
    {
      len = i - s;
      s = 0;
      lines++;
    }
  }
}
