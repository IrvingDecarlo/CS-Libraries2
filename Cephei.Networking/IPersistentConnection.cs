using System;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The generic IPersistentConnection interface denotes an object which contains a persistent connection to its target and is capable of sending/receiving
  /// a specific type of data to its endpoint.
  /// </summary>
  /// <typeparam name="T">Object type that is sent/received.</typeparam>
  public interface IPersistentConnection<T> : IPersistentConnection
  {
    /// <summary>
    /// Makes the connection communicate with its endpoint, sending a message and receiving another.
    /// </summary>
    /// <param name="message">Message to be sent to the endpoint.</param>
    /// <param name="attempts">Number of communication attempts until communication is aborted.</param>
    /// <param name="retrydelay">Delay between retry attempts.</param>
    /// <param name="expheader">Expected header to receive. Can be default to disable header validation.</param>
    /// <param name="minlen">Minimum response length.</param>
    /// <param name="maxlen">Maximum response length.</param>
    /// <returns>The message received from the endpoint. Returns default on error.</returns>
    Task<T> CommunicateAsync(T message, int attempts, int retrydelay, T expheader, int minlen, int maxlen);

    /// <summary>
    /// Receives the target's response.
    /// </summary>
    /// <param name="msg">Message to be sent to target.</param>
    /// <returns>The target's response.</returns>
    Task<T> ReceiveResponseAsync(T msg);

    /// <summary>
    /// Sends a message to target.
    /// </summary>
    /// <param name="msg">Message to be sent to target.</param>
    /// <returns>Task for sending the message.</returns>
    Task SendMessageAsync(T msg);
  }
  /// <summary>
  /// The IPersistentConnection interface denotes an object which contains a persistent connection to its target.
  /// </summary>
  public interface IPersistentConnection : IDisposable
  {
    /// <summary>
    /// OnConnectionError is called when the connection loses contact with its endpoint.
    /// </summary>
    event Action? OnConnectionError;

    /// <summary>
    /// OnConnectionSuccess is called when the connection gains/restores contact with its endpoint.
    /// </summary>
    event Action? OnConnectionSuccess;

    /// <summary>
    /// Gets the connection's connection status.
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Connects to the endpoint.
    /// </summary>
    /// <returns>True if connection was successful or if it's already active, false otherwise.</returns>
    Task<bool> TryConnectAsync();

    /// <summary>
    /// Closes the connection to the target.
    /// </summary>
    void Close();
  }
}
