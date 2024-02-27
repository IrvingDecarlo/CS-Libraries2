using Cephei.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The PersistentServerSocketString is a persistent socket server that communicates via strings.
  /// </summary>
  public abstract class PersistentServerSocketString : PersistentServerSocket<string>
  {
    /// <summary>
    /// Creates a PersistentServerSocket that listens to a local endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to use.</param>
    /// <param name="listening">Should the server automatically start listening and interpreting incoming connections?</param>
    public PersistentServerSocketString(EndPoint endpoint, bool listening) : base(endpoint, listening) 
    { }

    #region overrides

    /// <summary>
    /// Gets the message from an incoming connection.
    /// </summary>
    /// <param name="socket">Socket that handles the connection.</param>
    /// <param name="cancelsource">Cancellation token source.</param>
    /// <returns>The incoming socket's connection.</returns>
    protected override async Task<string> GetMessage(Socket socket, CancellationTokenSource cancelsource)
    {
      byte[] buffer = GetBuffer();
      string msg = Encoding.UTF8.GetString(buffer, 0, await socket.ReceiveAsync(buffer, SocketFlags.None, cancelsource.Token));
      GetLogger()?.LogDetail("Message received from server:" + msg);
      return msg;
    }

    #endregion

    #region protected

    /// <summary>
    /// Gets the buffer for handling incoming connections.
    /// </summary>
    /// <returns>The buffer for handling incoming connections.</returns>
    protected abstract byte[] GetBuffer();

    #endregion
  }
}
