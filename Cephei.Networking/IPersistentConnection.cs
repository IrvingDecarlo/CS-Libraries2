using System;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The two-generic IPersistentConnection interface contains message sending/receiving abstractions.
  /// </summary>
  /// <typeparam name="T">Object type that is sent.</typeparam>
  /// <typeparam name="U">Object type that is received.</typeparam>
  public interface IPersistentConnection<T, U> : IPersistentConnection<T>, IListeningConnection<U>
  {
    // METHODS

    /// <summary>
    /// Attempts to communicate, sending a message and then awaiting for a response.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The message received.</returns>
    /// <remarks>Cannot be used in conjunction with AwaitingMessages in many cases.
    /// Will throw the last exception if all attempts fail.</remarks>
    Task<U> CommunicateAsync(T message, int attempts, TimeSpan cooldown, TimeSpan timeout);
  }
  /// <summary>
  /// The one-generic IPersistentConnection interface contains message sending abstractions.
  /// </summary>
  /// <typeparam name="T">Object type that is sent.</typeparam>
  public interface IPersistentConnection<T> : IPersistentConnection
  {
    // METHODS

    /// <summary>
    /// Tries to send a message.
    /// </summary>
    /// <param name="message">Message object to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for sending the message.</returns>
    /// <remarks>Will throw the last exception if all attempts fail.</remarks>
    Task SendMessageAsync(T message, int attempts, TimeSpan cooldown, TimeSpan timeout);
  }
  /// <summary>
  /// The IPersistentConnection interface offers a base for all persistent connections, containing basic connectivity interfaces.
  /// </summary>
  public interface IPersistentConnection : IDisposable, IAsyncDisposable
  {
    // EVENTS

    /// <summary>
    /// OnConnectionEstablished is raised when the connection is successfully established.
    /// </summary>
    event Action OnConnectionEstablished;

    /// <summary>
    /// OnConnectionLost is raised when the connection to the endpoint is lost.
    /// </summary>
    event Action OnConnectionLost;

    // PROPERTIES

    /// <summary>
    /// Is the connection up?
    /// </summary>
    bool Connected { get; }

    // METHODS

    /// <summary>
    /// Attempts to connect to the endpoint.
    /// </summary>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    /// <remarks>Will throw the last exception if all attempts fail.</remarks>
    Task ConnectAsync(int attempts, TimeSpan cooldown, TimeSpan timeout);

    /// <summary>
    /// Disconnects the connection.
    /// </summary>
    /// <returns>The task for disconnecting.</returns>
    Task DisconnectAsync();
  }
}
