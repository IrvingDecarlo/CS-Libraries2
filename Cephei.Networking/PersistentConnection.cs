using Cephei.Logging;
using System;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The PersistentConnection object manages a connection that is to be maintained for longer than a single call.
  /// </summary>
  public abstract class PersistentConnection : IPersistentConnection
  {
    /// <summary>
    /// Disposes of the socket once destructor is called.
    /// </summary>
    ~PersistentConnection() => Dispose();

    #region overrides

    /// <summary>
    /// Disposes the socket, closing its connection.
    /// </summary>
    public void Dispose()
    {
      Close();
      OnDispose();
    }

    /// <summary>
    /// OnConnectionError is called when the socket loses contact with its endpoint.
    /// </summary>
    public event Action? OnConnectionError;

    /// <summary>
    /// OnConnectionSuccess is called when the socket gains/restores contact with its endpoint.
    /// </summary>
    public event Action? OnConnectionSuccess;

    /// <summary>
    /// Is the socket connected to its endpoint? This property is only updated when a communication is attempted, no matter if successful.
    /// </summary>
    public bool Connected
    {
      protected set
      {
        if (value == connected) return;
        if (value) OnConnectionSuccess?.Invoke();
        else OnConnectionError?.Invoke();
        connected = value;
      }
      get => connected;
    }

    /// <summary>
    /// Default method for connecting to the endpoint (used in CommunicateAsync).
    /// </summary>
    /// <returns>True if connection was successful or if it's already active, false otherwise.</returns>
    public abstract Task<bool> TryConnectAsync();

    /// <summary>
    /// Closes the connection to the target.
    /// </summary>
    public abstract void Close();

    #endregion

    #region protected

    // METHODS

    /// <summary>
    /// Handles communication failure, logging it and closing the connection.
    /// </summary>
    /// <param name="e">Exception to be outputted.</param>
    /// <param name="message">Message to be logged.</param>
    /// <param name="delay">Delay until next connection attempt.</param>
    /// <returns>Task that performs the communication failure.</returns>
    protected async Task OnCommunicationFailure(Exception e, string message, int delay)
    {
      GetLogger()?.LogInfo(message + ": " + e.Message);
      Close();
      await Task.Delay(delay);
    }

    /// <summary>
    /// Gets the writer for writing the socket's log. Can be null to write nothing.
    /// </summary>
    /// <returns>The writer for writing the socket's log.</returns>
    protected abstract ILogger? GetLogger();

    /// <summary>
    /// Additional actions to take when the persistent connection is disposed.
    /// </summary>
    protected virtual void OnDispose() { }

    #endregion

    #region private

    // VARIABLES

    private bool connected = false;

    #endregion
  }
}
