using System;
using System.Net.Sockets;
using System.Text;

namespace Cephei.Networking
{
  /// <summary>
  /// The string listening socket is a wrapper that receives socket messages and encodes them into readable strings.
  /// </summary>
  public class ListeningSocketString : ListeningSocket, IListeningConnection<string>
  {
    /// <summary>
    /// Wraps the string socket listener around an already existing socket.
    /// </summary>
    /// <param name="socket">Socket to wrap the listener around.</param>
    public ListeningSocketString(Socket socket) : base(socket) 
      => base.OnMessageReceived += ListeningSocketString_OnMessageReceived;
    /// <summary>
    /// Wraps the string socket listener around a default IPv4 TCP socket.
    /// </summary>
    public ListeningSocketString() : base()
      => base.OnMessageReceived += ListeningSocketString_OnMessageReceived;

    #region overrides

    /// <summary>
    /// OnMessageReceived is raised when a message is received by the socket.
    /// </summary>
    public new event Action<string>? OnMessageReceived;

    #endregion

    #region protected

    /// <summary>
    /// Gets the encoding to be used by the socket's encoder.
    /// </summary>
    /// <returns>The encoding to be used by the socket's encoder. Returns UTF8 by default.</returns>
    protected virtual Encoding GetEncoding() => Encoding.UTF8;

    #endregion

    #region private

    private void ListeningSocketString_OnMessageReceived(Memory<byte> msg) => OnMessageReceived?.Invoke(GetEncoding().GetString(msg.Span));

    #endregion
  }
}
