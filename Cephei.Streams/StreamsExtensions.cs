using System.Collections.Generic;
using System.IO;
using Cephei.Tools;

namespace Cephei.Streams
{
  /// <summary>
  /// StreamsExtensions contains extension methods for streams.
  /// </summary>
  public static class StreamsExtensions
  {
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