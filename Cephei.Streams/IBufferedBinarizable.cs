using System;
using System.IO;

namespace Cephei.Streams
{
  /// <summary>
  /// The IBufferedBinarizable interface denotes an object that is binarized through a buffer.
  /// </summary>
  public interface IBufferedBinarizable
  {
    /// <summary>
    /// Writes the object's content using a BinaryWriter.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    /// <param name="buffer">Buffer to use.</param>
    void ToBinary(BinaryWriter writer, Span<byte> buffer);
  }
}
