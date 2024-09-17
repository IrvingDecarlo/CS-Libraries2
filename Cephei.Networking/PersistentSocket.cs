using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The PersistentConnectionSocket class offers a persistent connection manager for sockets.
  /// </summary>
  public class PersistentSocket : PersistentConnection<ReadOnlyMemory<byte>, Memory<byte>>
  {
    /// <summary>
    /// Wraps a persistent connection object around a socket. May raise the OnConnectionEstablished event if the socket already is connected.
    /// </summary>
    /// <param name="socket">Socket to wrap around.</param>
    public PersistentSocket(Socket socket)
    {
      Socket = socket;
      Connected = socket.Connected;
    }
    /// <summary>
    /// Wraps the persistent connection object around a brand-new socket, which uses IPv4 TCP connection.
    /// </summary>
    public PersistentSocket() : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) 
    { }

    #region overrides

    /// <summary>
    /// Disposes of the socket that is being used by the persistent connection.
    /// </summary>
    public override void Dispose()
    {
      base.Dispose();
      Socket.Dispose();
    }

    /// <summary>
    /// Communicates with the endpoint, returning the response.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <returns>The endpoint's reply.</returns>
    protected override async Task<Memory<byte>> CommunicateAsync(ReadOnlyMemory<byte> message, CancellationToken token)
    {
      await SendMessageAsync(message, token);
      Memory<byte> buffer = GetBuffer();
      return buffer[..await Socket.ReceiveAsync(buffer, SocketFlags.None, token)];
    }

    /// <summary>
    /// Sends the message to the target.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <returns>The task for sending the message.</returns>
    protected override async Task SendMessageAsync(ReadOnlyMemory<byte> message, CancellationToken token) 
      => await Socket.SendAsync(message, SocketFlags.None, token);

    /// <summary>
    /// Disconnects from the endpoint.
    /// </summary>
    /// <returns>The task for disconnecting from the endpoint.</returns>
    public override async Task DisconnectAsync()
    {
      await base.DisconnectAsync();
      Socket.Shutdown(SocketShutdown.Both);
      Socket.Disconnect(true);
    }

    /// <summary>
    /// Starts awaiting for incoming messages.
    /// </summary>
    protected override void StartAwaiting()
    {
      if (!(cts is null)) return;
      cts = new CancellationTokenSource();
      AwaitMessages(cts.Token);
    }

    /// <summary>
    /// Stops awaiting for incoming messages.
    /// </summary>
    /// <returns>The task for stopping incoming messages.</returns>
    protected override Task StopAwaiting()
    {
      if (cts is null) return Task.CompletedTask;
      cts.Cancel();
      cts.Dispose();
      cts = null;
      return Task.CompletedTask;
    }

    #endregion

    #region public

    /// <summary>
    /// Reference to the socket that is being used by the persistent connection.
    /// </summary>
    public readonly Socket Socket;

    #endregion

    #region protected

    /// <summary>
    /// Gets the buffer that is used for receiving the socket's response.
    /// </summary>
    /// <returns>The buffer that is used for receiving the socket's response.</returns>
    /// <remarks>Default buffer size is of 1024 bytes.</remarks>
    protected virtual Memory<byte> GetBuffer() => new byte[1024];

    #endregion

    #region private

    // VARIABLES

    private CancellationTokenSource? cts = null;

    // METHODS

    private async void AwaitMessages(CancellationToken token)
    {
      Memory<byte> buffer;
      int c;
      while (!token.IsCancellationRequested)
      {
        buffer = GetBuffer();
        try 
        {
          c = await Socket.ReceiveAsync(buffer, SocketFlags.None, token);
          if (c < 1)
          {
            DisconnectListening();
            return;
          }
          ReceiveMessage(buffer[..c]);
        }
        catch (OperationCanceledException) { return; }
        catch (SocketException e)
        {
          if (e.SocketErrorCode == SocketError.ConnectionReset)
          {
            DisconnectListening();
            return;
          }
          ReceiveException(e);
        }
        catch (Exception e) { ReceiveException(e); }
      }
    }

    private void DisconnectListening()
    {
      ReceiveDisconnect();
      AwaitingMessages = false;
    }

    #endregion
  }
}
