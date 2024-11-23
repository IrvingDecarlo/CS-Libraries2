using System.IO;

namespace Cephei.Streams
{
  /// <summary>
  /// The IStreamable denotes an object that can return a stream.
  /// </summary>
  public interface IStreamable
  {
    /// <summary>
    /// Gets the object's stream.
    /// </summary>
    /// <returns>The object's stream.</returns>
    Stream GetStream();
  }
}
