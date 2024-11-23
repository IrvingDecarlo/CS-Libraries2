using System.IO;

namespace Cephei.Streams
{
  /// <summary>
  /// The IStreamable interface denotes an object that can yield a Stream.
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
