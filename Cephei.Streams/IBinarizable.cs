using System.IO;

namespace Cephei.Streams
{
  /// <summary>
  /// The IBinarizable interface denotes an object that can be Binarized using
  /// a BinaryWriter.
  /// </summary>
  public interface IBinarizable
  {
    /// <summary>
    /// Writes the object's content using a BinaryWriter.
    /// </summary>
    /// <param name="writer">BinaryWriter to use.</param>
    void ToBinary(BinaryWriter writer);
  }
}

