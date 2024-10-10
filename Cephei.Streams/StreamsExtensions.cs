using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cephei.Strings;

namespace Cephei.Streams
{
  /// <summary>
  /// StreamsExtensions contains extension methods for streams.
  /// </summary>
  public static class StreamsExtensions
  {
    /// <summary>
    /// Reads data from a reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="amount">Amount of data to extract.</param>
    /// <returns>The IMemoryOwner containing the extracted data.</returns>
    public static IMemoryOwner<byte> Read(this BinaryReader reader, int amount)
    {
      IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(amount);
      reader.Read(owner.Memory.Span);
      return owner;
    }

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
    /// Reads a string out of a BinaryReader on to a buffer. The string is not size-prefixed, so strings written through the standard Write(string) method will not work.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <param name="enc">Encoder to use.</param>
    /// <returns>The debinarized string.</returns>
    public static string ReadString(this BinaryReader reader, Span<byte> buffer, Encoding enc)
      => enc.GetString(buffer[..reader.Read(buffer)]);
    /// <summary>
    /// Reads a string out of a BinaryReader on to a buffer. The string is not size-prefixed, so strings written through the standard Write(string) method will not work.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <returns>The debinarized string.</returns>
    /// <remarks>Uses UTF-8 encoding.</remarks>
    public static string ReadString(this BinaryReader reader, Span<byte> buffer)
      => reader.ReadString(buffer, Encoding.UTF8);

    /// <summary>
    /// Reads a byte size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromByte(this BinaryReader reader, Span<byte> buffer, Encoding enc)
      => reader.ReadString(buffer[..reader.ReadByte()], enc);
    /// <summary>
    /// Reads a byte size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromByte(this BinaryReader reader, Span<byte> buffer)
      => reader.ReadStringFromByte(buffer, Encoding.UTF8);
    /// <summary>
    /// Reads a byte size-prefixed string from a binary reader. The buffer size will automatically be defined.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromByte(this BinaryReader reader, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(reader.ReadByte());
      return reader.ReadString(owner.Memory.Span, enc);
    }
    /// <summary>
    /// Reads a byte size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromByte(this BinaryReader reader)
      => reader.ReadStringFromByte(Encoding.UTF8);

    /// <summary>
    /// Reads a UInt16 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromUInt16(this BinaryReader reader, Span<byte> buffer, Encoding enc)
      => reader.ReadString(buffer[..reader.ReadUInt16()], enc);
    /// <summary>
    /// Reads a UInt16 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromUInt16(this BinaryReader reader, Span<byte> buffer)
      => reader.ReadStringFromUInt16(buffer, Encoding.UTF8);
    /// <summary>
    /// Reads a UInt16 size-prefixed string from a binary reader. The buffer size will automatically be defined.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromUInt16(this BinaryReader reader, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(reader.ReadUInt16());
      return reader.ReadString(owner.Memory.Span, enc);
    }
    /// <summary>
    /// Reads a UInt16 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromUInt16(this BinaryReader reader)
      => reader.ReadStringFromUInt16(Encoding.UTF8);

    /// <summary>
    /// Reads a Int32 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromInt32(this BinaryReader reader, Span<byte> buffer, Encoding enc)
      => reader.ReadString(buffer[..reader.ReadInt32()], enc);
    /// <summary>
    /// Reads a Int32 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer for transferring data.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    public static string ReadStringFromInt32(this BinaryReader reader, Span<byte> buffer)
      => reader.ReadStringFromInt32(buffer, Encoding.UTF8);
    /// <summary>
    /// Reads a Int32 size-prefixed string from a binary reader. The buffer size will automatically be defined.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="enc">Encoding to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromInt32(this BinaryReader reader, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(reader.ReadInt32());
      return reader.ReadString(owner.Memory.Span, enc);
    }
    /// <summary>
    /// Reads a Int32 size-prefixed string from a binary reader.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The string extracted from the binary reader.</returns>
    /// <remarks>It is recommended to use the other overloads with buffer definitions for multiple read operations.</remarks>
    public static string ReadStringFromInt32(this BinaryReader reader)
      => reader.ReadStringFromInt32(Encoding.UTF8);

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
    /// Reads data to a buffer with the size prefixed by a byte.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <returns>The number of bytes read to the buffer.</returns>
    public static int ReadFromByte(this BinaryReader reader, Span<byte> buffer)
      => reader.Read(buffer[..reader.ReadByte()]);
    /// <summary>
    /// Rents a memory owner and fills it with a byte-prefixed amount of data.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The filled memory owner.</returns>
    public static IMemoryOwner<byte> ReadFromByte(this BinaryReader reader)
      => reader.Read(reader.ReadByte());

    /// <summary>
    /// Reads data to a buffer with the size prefixed by a UInt16.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <returns>The number of bytes read to the buffer.</returns>
    public static int ReadFromUInt16(this BinaryReader reader, Span<byte> buffer)
      => reader.Read(buffer[..reader.ReadUInt16()]);
    /// <summary>
    /// Rents a memory owner and fills it with a UInt16-prefixed amount of data.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The filled memory owner.</returns>
    public static IMemoryOwner<byte> ReadFromUInt16(this BinaryReader reader)
      => reader.Read(reader.ReadUInt16());

    /// <summary>
    /// Reads data to a buffer with the size prefixed by a Int32.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <returns>The number of bytes read to the buffer.</returns>
    public static int ReadFromInt32(this BinaryReader reader, Span<byte> buffer)
      => reader.Read(buffer[..reader.ReadInt32()]);
    /// <summary>
    /// Rents a memory owner and fills it with a Int32-prefixed amount of data.
    /// </summary>
    /// <param name="reader">Reader to use.</param>
    /// <returns>The filled memory owner.</returns>
    public static IMemoryOwner<byte> ReadFromInt32(this BinaryReader reader)
      => reader.Read(reader.ReadInt32());

    /// <summary>
    /// Writes a non-prefixed string using a binary writer and a buffer for encoding.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <param name="enc">Encoder to use.</param>
    public static void Write(this BinaryWriter writer, string value, Span<byte> buffer, Encoding enc)
      => writer.Write(buffer[..enc.GetBytes(value, buffer)]);
    /// <summary>
    /// Writes a non-prefixed string using a binary writer and a buffer for encoding.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <remarks>Uses UTF-8 encoding.</remarks>
    public static void Write(this BinaryWriter writer, string value, Span<byte> buffer)
      => writer.Write(value, buffer, Encoding.UTF8);

    /// <summary>
    /// Writes a byte size-prefixed span of values.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">Values to write.</param>
    public static void WriteWithByte(this BinaryWriter writer, ReadOnlySpan<byte> value)
    {
      writer.Write((byte)value.Length);
      writer.Write(value);
    }
    /// <summary>
    /// Writes a byte size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithByte(this BinaryWriter writer, string value, Span<byte> buffer, Encoding enc)
      => writer.WriteWithByte(buffer[..enc.GetBytes(value, buffer)]);
    /// <summary>
    /// Writes a byte size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    public static void WriteWithByte(this BinaryWriter writer, string value, Span<byte> buffer)
      => writer.WriteWithByte(value, buffer, Encoding.UTF8);
    /// <summary>
    /// Writes a byte size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithByte(this BinaryWriter writer, string value, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(enc.GetMaxByteCount(value.Length));
      writer.WriteWithByte(value, owner.Memory.Span, enc);
    }
    /// <summary>
    /// Writes a byte size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    public static void WriteWithByte(this BinaryWriter writer, string value)
      => writer.WriteWithByte(value, Encoding.UTF8);

    /// <summary>
    /// Writes a UInt16 size-prefixed span of values.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">Values to write.</param>
    public static void WriteWithUInt16(this BinaryWriter writer, ReadOnlySpan<byte> value)
    {
      writer.Write((ushort)value.Length);
      writer.Write(value);
    }
    /// <summary>
    /// Writes a UInt16 size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithUInt16(this BinaryWriter writer, string value, Span<byte> buffer, Encoding enc)
      => writer.WriteWithUInt16(buffer[..enc.GetBytes(value, buffer)]);
    /// <summary>
    /// Writes a UInt16 size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    public static void WriteWithUInt16(this BinaryWriter writer, string value, Span<byte> buffer)
      => writer.WriteWithUInt16(value, buffer, Encoding.UTF8);
    /// <summary>
    /// Writes a UInt16 size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithUInt16(this BinaryWriter writer, string value, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(enc.GetMaxByteCount(value.Length));
      writer.WriteWithUInt16(value, owner.Memory.Span, enc);
    }
    /// <summary>
    /// Writes a UInt16 size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    public static void WriteWithUInt16(this BinaryWriter writer, string value)
      => writer.WriteWithUInt16(value, Encoding.UTF8);

    /// <summary>
    /// Writes a Int32 size-prefixed span of values.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">Values to write.</param>
    public static void WriteWithInt32(this BinaryWriter writer, ReadOnlySpan<byte> value)
    {
      writer.Write(value.Length);
      writer.Write(value);
    }
    /// <summary>
    /// Writes a Int32 size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithInt32(this BinaryWriter writer, string value, Span<byte> buffer, Encoding enc)
      => writer.WriteWithInt32(buffer[..enc.GetBytes(value, buffer)]);
    /// <summary>
    /// Writes a Int32 size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="buffer">Buffer to use.</param>
    public static void WriteWithInt32(this BinaryWriter writer, string value, Span<byte> buffer)
      => writer.WriteWithInt32(value, buffer, Encoding.UTF8);
    /// <summary>
    /// Writes a Int32 size-prefixed string.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    /// <param name="enc">Encoding to use.</param>
    public static void WriteWithInt32(this BinaryWriter writer, string value, Encoding enc)
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(enc.GetMaxByteCount(value.Length));
      writer.WriteWithInt32(value, owner.Memory.Span, enc);
    }
    /// <summary>
    /// Writes a Int32 size-prefixed string. Uses UTF-8 encoding.
    /// </summary>
    /// <param name="writer">Writer to use.</param>
    /// <param name="value">String to write.</param>
    public static void WriteWithInt32(this BinaryWriter writer, string value)
      => writer.WriteWithInt32(value, Encoding.UTF8);

    /// <summary>
    /// Writes a non-prefixed string using a binary writer.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="value">String to write.</param>
    public static void WriteUnfixed(this BinaryWriter writer, string value)
    {
      for (int i = 0; i < value.Length; i++) writer.Write(value[i]);
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
    public static void BinarizeAll<T>(this IEnumerable<T> en, BinaryWriter writer) where T : IBinarizable
    {
      foreach (T bin in en) bin.ToBinary(writer);
    }
    /// <summary>
    /// Binarizes all binarizable objects in a collection using a BinaryWriter.
    /// </summary>
    /// <typeparam name="T">Binarizable object type.</typeparam>
    /// <param name="en">Collection of binarizable objects.</param>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="buffersize">The buffer's size (in bytes).</param>
    public static void BinarizeAll<T>(this IEnumerable<T> en, BinaryWriter writer, int buffersize) where T : IBufferedBinarizable
    {
      using IMemoryOwner<byte> owner = MemoryPool<byte>.Shared.Rent(buffersize);
      en.BinarizeAll(writer, owner.Memory.Span);
    }
    /// <summary>
    /// Binarizes all binarizable objects in a collection using a BinaryWriter.
    /// </summary>
    /// <typeparam name="T">Binarizable object type.</typeparam>
    /// <param name="en">Collection of binarizable objects.</param>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="buffer">Buffer to use.</param>
    public static void BinarizeAll<T>(this IEnumerable<T> en, BinaryWriter writer, Span<byte> buffer) where T : IBufferedBinarizable
    {
      foreach (T bin in en) bin.ToBinary(writer, buffer);
    }

    /// <summary>
    /// Debinarizes all debinarizable objects in a collection using a BinaryReader.
    /// </summary>
    /// <typeparam name="T">Debinarizable object type.</typeparam>
    /// <param name="en">Collection of debinarizable objects.</param>
    /// <param name="reader">BinaryReader to use.</param>
    public static void DebinarizeAll<T>(this IEnumerable<T> en, BinaryReader reader) where T : IDebinarizable
    {
      foreach (T bin in en) bin.FromBinary(reader);
    }

    #region private

    #endregion
  }
}