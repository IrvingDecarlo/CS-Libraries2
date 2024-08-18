using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The String persistent socket connection is capable of operating sockets via their native connection methods and by encoding the sent/received packets into a 
  /// readable string.
  /// </summary>
  public abstract class PersistentSocketString : PersistentSocket, IPersistentConnection<string, string>
  {
    /// <summary>
    /// Creates a new string persistent socket connection.
    /// </summary>
    /// <param name="socket">Socket to wrap the persistent connection around.</param>
    public PersistentSocketString(Socket socket) : base(socket)
      => base.OnMessageReceived += EncodeReceivedMessage;
    /// <summary>
    /// Creates a new string persistent socket connection around a default IPv4 TCP socket.
    /// </summary>
    public PersistentSocketString() : base()
      => base.OnMessageReceived += EncodeReceivedMessage;

    #region overrides

    /// <summary>
    /// The OnMessageReceived event is raised when a message is received.
    /// </summary>
    public new event Action<string>? OnMessageReceived;

    /// <summary>
    /// Tries to send a message. Will disconnect and attempt reconnection upon failure.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for sending the message.</returns>
    public async Task<string> CommunicateAsync(string message, int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      Encoding encoding = GetEncoding();
      return encoding.GetString((await CommunicateAsync(encoding.GetBytes(message), attempts, cooldown, timeout)).Span);
    }

    /// <summary>
    /// Tries to send a message. Will disconnect and attempt reconnection upon failure.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for sending the message.</returns>
    public async Task SendMessageAsync(string message, int attempts, TimeSpan cooldown, TimeSpan timeout)
      => await SendMessageAsync(GetEncoding().GetBytes(message), attempts, cooldown, timeout);

    #endregion

    #region protected

    /// <summary>
    /// Gets the encoding that is used by the persistent connection.
    /// </summary>
    /// <returns>The encoding that is used by the persistent connection. Default is UTF8.</returns>
    protected virtual Encoding GetEncoding() => Encoding.UTF8;

    #endregion

    #region private

    private void EncodeReceivedMessage(Memory<byte> message) => OnMessageReceived?.Invoke(GetEncoding().GetString(message.Span));

    #endregion
  }
}
