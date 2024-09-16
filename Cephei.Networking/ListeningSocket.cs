using System;
using System.Net.Sockets;
using System.Threading;

namespace Cephei.Networking
{
  /// <summary>
  /// The ListeningSocket is a wrapper that enables a socket to continuously listen to incoming messages.
  /// </summary>
  public class ListeningSocket : IListeningConnection<Memory<byte>>
  {
    /// <summary>
    /// Wraps the ListeningSocket around a socket.
    /// </summary>
    /// <param name="socket">Socket to wrap the listener around.</param>
    public ListeningSocket(Socket socket) => Socket = socket;
    /// <summary>
    /// Creates a ListeningSocket wrapping around a default IPv4 TCP socket.
    /// </summary>
    public ListeningSocket() : this(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) 
    { }

    #region overrides

    /// <summary>
    /// OnMessageReceived is raised when the socket receives an incoming message.
    /// </summary>
    public event Action<Memory<byte>>? OnMessageReceived;

    /// <summary>
    /// OnListeningException is raised when an exception is thrown while the connection is awaiting messages.
    /// </summary>
    public event Action<Exception>? OnListeningException;

    /// <summary>
    /// OnListeningDisconnected is raised when the connection is closed while the connection is awaiting messages.
    /// </summary>
    public event Action? OnListeningDisconnected;

    /// <summary>
    /// Is the socket currently awaiting for incoming messages?
    /// </summary>
    public bool AwaitingMessages
    {
      set
      {
        if (value == AwaitingMessages) return;
        if (value)
        {
          cts = new CancellationTokenSource();
          AwaitMessages(cts.Token);
          return;
        }
        DisposeCTS();
      }
      get => !(cts is null);
    }

    /// <summary>
    /// Disposes the listener and the socket.
    /// </summary>
    public virtual void Dispose()
    {
      Socket.Dispose();
      DisposeCTS();
    }

    #endregion

    #region public

    /// <summary>
    /// Reference to the socket wrapped by this object.
    /// </summary>
    public readonly Socket Socket;

    #endregion

    #region protected

    /// <summary>
    /// Gets the buffer that will be used for receiving incoming messages.
    /// </summary>
    /// <returns>The buffer that will be used for receiving incoming messages.</returns>
    /// <remarks>Default buffer size is of 1024 bytes.</remarks>
    protected Memory<byte> GetBuffer() => new byte[1024];

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
          OnMessageReceived?.Invoke(buffer[..c]);
        }
        catch (OperationCanceledException) { return; }
        catch (SocketException e)
        {
          if (e.SocketErrorCode == SocketError.ConnectionReset)
          {
            DisconnectListening();
            return;
          }
          OnListeningException?.Invoke(e);
        }
        catch (Exception e) { OnListeningException?.Invoke(e); }
      }
    }

    private void DisposeCTS()
    {
      if (cts is null) return;
      cts.Cancel();
      cts.Dispose();
      cts = null;
    }

    private void DisconnectListening()
    {
      OnListeningDisconnected?.Invoke();
      AwaitingMessages = false;
    }

    #endregion
  }
}
