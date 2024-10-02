using System.Collections.Generic;
using System.IO;
using Cephei.Strings;

namespace Cephei.Streams
{
  /// <summary>
  /// StreamsExtensions contains extension methods for streams.
  /// </summary>
  public static class StreamsExtensions
  {
    /// <summary>
    /// Reads data using a reader and returns a string.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <param name="offset">Initial index for writing in the buffer.</param>
    /// <param name="count">Number of cases that ought to be read.</param>
    /// <param name="readbytes">Number of cases that were actually read.</param>
    /// <returns>The string that was read by the reader.</returns>
    public static string Read(this TextReader reader, char[] buffer, int offset, int count, out int readbytes)
    {
      readbytes = reader.Read(buffer, offset, count);
      return new string(buffer, offset, readbytes);
    }

    /// <summary>
    /// Reads an Int64 from a reader using the 7Bit format.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The Int64 extracted from the reader.</returns>
    /// <remarks>UNFINISHED! Do not use in production.</remarks>
    public static long Read7BitInt64(this BinaryReader reader)
    {
      byte v = reader.ReadByte();
      long i = (v & 1) > 0 ? 1 : -1;
      long value = 0;
      byte it = 2;
      while (true)
      {
        if (it == 128)
        {
          if ((v & it) == 0) break;
          v = reader.ReadByte();
          it = 1;
        }
        if ((v & it) > 0) value += i;
        i *= 2;
        it *= 2;
      }
      return value;
    }

    /// <summary>
    /// Writes an integer in the 7Bit format.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="value">Value to be written.</param>
    /// <remarks>UNFINISHED! Do not use in production.</remarks>
    public static void Write7Bit(this BinaryWriter writer, long value)
    {
      int s = value.CompareTo(0);
      long i = s;
      byte v;
      if (s < 0) v = 0;
      else v = 1;
      byte it = 2;
      int c = value.CompareTo(i);
      while (c == s || c == 0)
      {
        if (it == 128)
        {
          v += it;
          writer.Write(v);
          it = 1;
          v = 0;
        }
        if ((value & i) != 0) v += it;
        i *= 2;
        it *= 2;
        c = value.CompareTo(i);
      }
    }

    /// <summary>
    /// Writes a KeyValuePair enumerable containing another KeyValuePair enumerable in its value in a stream.
    /// </summary>
    /// <typeparam name="T">The main enumerable's key type.</typeparam>
    /// <typeparam name="U">The secondary enumerable's key type.</typeparam>
    /// <typeparam name="V">The secondary enumerable's value type.</typeparam>
    /// <typeparam name="W">The secondary enumerable's type.</typeparam>
    /// <param name="writer">TextWriter to use.</param>
    /// <param name="dict">The collection to print.</param>
    public static void WriteLine<T, U, V, W>(this TextWriter writer, IEnumerable<KeyValuePair<T, W>> dict) where W : IEnumerable<KeyValuePair<U, V>>
    {
      foreach (KeyValuePair<T, W> sect in dict)
      {
        writer.WriteLine($"[{sect.Key}]");
        foreach (KeyValuePair<U, V> kvp in sect.Value) writer.WriteLine(kvp.ToPairString());
      }
    }

    /// <summary>
    /// Binarizes all binarizable objects in a collection using a BinaryWriter.
    /// </summary>
    /// <typeparam name="T">Binarizable object type.</typeparam>
    /// <param name="en">Collection of binarizable objects.</param>
    /// <param name="writer">BinaryWriter to use.</param>
    public static void BinarizeAll<T>(this IEnumerable<T> en, BinaryWriter writer)
      where T : IBinarizable
    {
      foreach (IBinarizable bin in en) bin.ToBinary(writer);
    }

    /// <summary>
    /// Debinarizes all debinarizable objects in a collection using a BinaryReader.
    /// </summary>
    /// <typeparam name="T">Debinarizable object type.</typeparam>
    /// <param name="en">Collection of debinarizable objects.</param>
    /// <param name="reader">BinaryReader to use.</param>
    public static void DebinarizeAll<T>(this IEnumerable<T> en, BinaryReader reader)
      where T : IDebinarizable
    {
      foreach (IDebinarizable bin in en) bin.FromBinary(reader);
    }
  }
}