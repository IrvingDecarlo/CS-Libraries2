using System;

namespace Cephei.Networking.Servers
{
  /// <summary>
  /// The IServer interface denotes an object that can establish connections like a server.
  /// </summary>
  /// <typeparam name="T">Object type that is created when a new connection is established.</typeparam>
  public interface IServer<T> : IServer
  {
    /// <summary>
    /// OnConnectionEstablished is raised when a new client connection is established with the server.
    /// </summary>
    event Action<T> OnConnectionEstablished;
  }
  /// <summary>
  /// The IServer interface denotes an object that can establish connections like a server.
  /// </summary>
  public interface IServer : IDisposable
  {
    /// <summary>
    /// Is the server listening to new connections?
    /// </summary>
    bool Listening { get; set; }

    /// <summary>
    /// Initiates the server.
    /// </summary>
    /// <remarks>It is not equal to the Listening property. Initiate only sets up initial data (such as port binding).</remarks>
    void Initiate();
  }
}
