using Cephei.Logging;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// Persistent Sockets are sockets that are refreshed upon connection failure.
  /// </summary>
  public abstract class SocketPersistent : PersistentConnectionString
  {
    #region overrides

    /// <summary>
    /// Receives the socket's reply, converting its content to a string.
    /// </summary>
    /// <param name="message">Message to be sent. Unused since sending and receiving in sockets are two different operations, use CommunicateAsync instead.</param>
    /// <returns>The socket's reply.</returns>
    public override async Task<string> ReceiveResponseAsync(string message)
    {
      byte[] buffer = GetBuffer();
      int cases = await Socket.ReceiveAsync(buffer, SocketFlags.None);
      return Encoding.UTF8.GetString(buffer, 0, cases);
    }

    /// <summary>
    /// Sends the message to the socket's target.
    /// </summary>
    /// <param name="msg">Message to be sent.</param>
    /// <returns>The task that performs the send message function.</returns>
    public override async Task SendMessageAsync(string msg)
      => await Socket.SendAsync(Encoding.UTF8.GetBytes(msg + (char)13 + (char)10), SocketFlags.None);

    /// <summary>
    /// Closes the connection.
    /// </summary>
    public override void Close()
    {
      if (Socket is null) return;
      Socket.Dispose();
      Socket = null;
      GetLogger()?.LogDebug("Connection closed.");
    }

    #endregion

    #region protected

    // PROPERTIES

    /// <summary>
    /// Reference to the currently used Socket.
    /// </summary>
    protected Socket? Socket { private set; get; }

    // METHODS
    
    /// <summary>
    /// Attempts to connect the socked to its endpoint.
    /// </summary>
    /// <param name="timeout">Connection timeout.</param>
    /// <returns>True if the connection was successful or if it's already active, false otherwise.</returns>
    protected async Task<bool> TryConnectAsync(int timeout)
    {
      Socket ??= GetNewSocket();
      if (Socket.Connected) return true;
      ILogger? logger = GetLogger();
      logger?.LogInfo($"Attempting connection to endpoint (Timeout={timeout})...");
      try
      {
        new CancellationTokenSource(timeout);
        await ConnectAsync(Socket);
      }
      catch (Exception ex)
      {
        logger?.Log(ex);
        logger?.LogInfo("Connection failed.");
        return false;
      }
      logger?.LogInfo($"Connection successful.");
      return true;
    }

    /// <summary>
    /// Gets the buffer for receiving messages.
    /// </summary>
    /// <returns>The buffer for receiving messages.</returns>
    protected abstract byte[] GetBuffer();

    /// <summary>
    /// Method that connects the socket to its endpoint.
    /// </summary>
    /// <param name="socket">Socket to use for connection. It is created via GetSocket().</param>
    /// <returns>The task for connecting the socket.</returns>
    /// <remarks>Do not call this manually since it does not contain exception protection.</remarks>
    protected abstract Task ConnectAsync(Socket socket);

    /// <summary>
    /// Method that generates a new socket.
    /// </summary>
    /// <returns>A new socket.</returns>
    protected abstract Socket GetNewSocket();

    #endregion
  }
}
