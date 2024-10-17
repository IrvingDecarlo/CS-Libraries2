using System;

namespace Cephei.Networking
{
  /// <summary>
  /// IListeningConnections are connections that are capable of listening to incoming messages.
  /// </summary>
  /// <typeparam name="T">Message type that will be received.</typeparam>
  public interface IListeningConnection<T> : IListeningConnection
  {
    /// <summary>
    /// The OnMessageReceived is raised when a message is received by the endpoint.
    /// </summary>
    /// <remarks>It is only raised when AwaitingMessages is true.</remarks>
    event Action<T> OnMessageReceived;
  }
  /// <summary>
  /// IListeningConnections are connections that are capable of listening to incoming messages.
  /// </summary>
  public interface IListeningConnection : IDisposable, IAsyncDisposable
  {
    // EVENTS

    /// <summary>
    /// OnListeningException is raised when an exception is thrown while the connection is awaiting messages.
    /// </summary>
    event Action<Exception> OnListeningException;

    /// <summary>
    /// OnListeningDisconnected is raised when the connection is closed while the connection is awaiting messages.
    /// </summary>
    event Action OnListeningDisconnected;

    /// <summary>
    /// Is the connection currently waiting for messages?
    /// </summary>
    /// <remarks>Cannot be used in conjunction with CommunicateAsync in many cases.</remarks>
    bool AwaitingMessages { get; set; }
  }
}
