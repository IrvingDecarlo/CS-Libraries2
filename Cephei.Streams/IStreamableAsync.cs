using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Streams
{
  /// <summary>
  /// The IStreamableAsync denotes an object that can yield a stream asynchronously.
  /// </summary>
  public interface IStreamableAsync
  {
    /// <summary>
    /// Gets the object's stream asynchronously.
    /// </summary>
    /// <param name="token">CancellationToken for aborting the operation.</param>
    /// <returns>The object's stream.</returns>
    ValueTask<Stream> GetStreamAsync(CancellationToken token = default);
  }
}
