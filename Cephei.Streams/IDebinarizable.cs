using System.IO;

namespace Cephei.Streams
{
  /// <summary>
  /// The Debinarizable interface denotes an object that can have its content
  /// determined by a BinaryReader.
  /// </summary>
  public interface IDebinarizable
  {
    /// <summary>
    /// Sets the object's data using a BinaryReader.
    /// </summary>
    /// <param name="reader">BinaryReader to use.</param>
    void FromBinary(BinaryReader reader);
  }
}

