using System;

namespace Cephei.Networking
{
  /// <summary>
  /// The generic IPollingConnection is a persistent connection that can poll to its target, transferring a specific type of data to its target.
  /// </summary>
  /// <typeparam name="T">Object type to transfer.</typeparam>
  public interface IPollingConnection<T> : IPersistentConnection<T>, IPollingConnection
  {

  }
  /// <summary>
  /// IPollingConnection represents a persistent connection that repeatedly polls to its target.
  /// </summary>
  public interface IPollingConnection : IPersistentConnection
  {
    /// <summary>
    /// Actions to take when polling starts.
    /// </summary>
    event Action? OnPollStart;

    /// <summary>
    /// Actions to take when polling stops.
    /// </summary>
    event Action? OnPollStop;

    /// <summary>
    /// Is the connection currently polling?
    /// </summary>
    bool Polling { get; }

    /// <summary>
    /// Starts to poll the connection.
    /// </summary>
    void PollStart();

    /// <summary>
    /// Stops the connection's polling.
    /// </summary>
    void PollStop();
  }
}
