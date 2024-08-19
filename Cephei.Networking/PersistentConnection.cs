using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The one-generic PersistentConnection object offers the implementation for sending and receiving messages.
  /// </summary>
  /// <typeparam name="T">Object type that is sent.</typeparam>
  /// <typeparam name="U">Object type that is received.</typeparam>
  public abstract class PersistentConnection<T, U> : PersistentConnection<T>, IPersistentConnection<T, U>
  {
    #region overrides

    /// <summary>
    /// Disposes of the object.
    /// </summary>
    /// <remarks>Overriders must call this base method BEFORE their own implementation.</remarks>
    public override void Dispose()
    {
      base.Dispose();
      AwaitingMessages = false;
    }

    /// <summary>
    /// Disconnects from the endpoint, ceasing to await messages if AwaitingMessages is true.
    /// </summary>
    /// <returns>The task for disconnecting from the endpoint.</returns>
    /// <remarks>Overriders must call this base method BEFORE their own implementation.</remarks>
    public override async Task DisconnectAsync()
    {
      if (AwaitingMessages) await StopAwaiting();
      await base.DisconnectAsync();
    }

    /// <summary>
    /// Attempts to connect to the endpoint, resuming awaiting messages if AwaitingMessages is true.
    /// </summary>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    /// <remarks>Overriders should call this base method AFTER their own implementation.</remarks>
    protected override Task ConnectAsync(CancellationToken token)
    {
      if (AwaitingMessages) StartAwaiting();
      return Task.CompletedTask;
    }

    /// <summary>
    /// OnMessageReceived is raised when a message is received from the endpoint.
    /// </summary>
    public event Action<U>? OnMessageReceived;

    /// <summary>
    /// OnListeningException is raised when an exception is thrown while the connection is awaiting messages.
    /// </summary>
    public event Action<Exception>? OnListeningException;

    /// <summary>
    /// OnListeningDisconnected is raised when the connection is closed while the connection is awaiting messages.
    /// </summary>
    public event Action? OnListeningDisconnected;

    /// <summary>
    /// Is the connection currently awaiting for new incoming messages? Awaiting can only be enabled if the connection is established with the target.
    /// </summary>
    public bool AwaitingMessages
    {
      set
      {
        if (awaiting == value) return;
        if (value)
        {
          if (Connected) StartAwaiting();
        }
        else StopAwaiting();
        awaiting = value;
      }
      get => awaiting;
    }

    /// <summary>
    /// Tries to send a message.
    /// </summary>
    /// <param name="message">Message object to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for sending the message.</returns>
    public async Task<U> CommunicateAsync(T message, int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      for (int i = 0; i <= attempts; i++)
      {
        try
        {
          using CancellationTokenSource cts = new CancellationTokenSource(timeout);
          U msg = await CommunicateAsync(message, cts.Token);
          cts.Dispose();
          Connected = true;
          return msg;
        }
        catch
        {
          if (i == attempts)
          {
            Connected = false;
            throw;
          }
          await Task.Delay(cooldown);
        }
      }
      throw new ConnectionException($"Sending message '{message}' has failed", attempts);
    }

    #endregion

    #region protected

    /// <summary>
    /// Attempts to communicate with the endpoint.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <remarks>Should throw an exception if communication has failed.</remarks>
    protected abstract Task<U> CommunicateAsync(T message, CancellationToken token);

    /// <summary>
    /// StopAwaiting is called when awaiting is to be stopped, either by disconnection or by AwaitingMessages being set to false.
    /// </summary>
    /// <remarks>Note that this method may be called multiple times.</remarks>
    protected abstract Task StopAwaiting();

    /// <summary>
    /// StartAwaiting is called when awaiting is to be initiated, either by (re)connection or by AwaitingMessages being set to true.
    /// </summary>
    /// <remarks>Note that this method may be called multiple times.</remarks>
    protected abstract void StartAwaiting();

    /// <summary>
    /// Invokes the OnMessageReceived event.
    /// </summary>
    /// <param name="message">Message that was received.</param>
    protected void ReceiveMessage(U message) => OnMessageReceived?.Invoke(message);

    /// <summary>
    /// Invokes the OnListeningException event.
    /// </summary>
    /// <param name="e">Exception that was thrown.</param>
    protected void ReceiveException(Exception e) => OnListeningException?.Invoke(e);

    /// <summary>
    /// Invokes the OnListeningDisconnected event.
    /// </summary>
    protected void ReceiveDisconnect() => OnListeningDisconnected?.Invoke();

    #endregion

    #region private

    private bool awaiting = false;

    #endregion
  }
  /// <summary>
  /// The one-generic PersistentConnection object offers the implementation for sending messages.
  /// </summary>
  /// <typeparam name="T">Object type that is sent.</typeparam>
  public abstract class PersistentConnection<T> : PersistentConnection, IPersistentConnection<T>
  {
    #region overrides

    /// <summary>
    /// Tries to send a message.
    /// </summary>
    /// <param name="message">Message object to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for sending the message.</returns>
    public async Task SendMessageAsync(T message, int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      for (int i = 0; i <= attempts; i++)
      {
        try
        {
          using CancellationTokenSource cts = new CancellationTokenSource(timeout);
          await SendMessageAsync(message, cts.Token);
          cts.Dispose();
          Connected = true;
          return;
        }
        catch
        {
          if (i == attempts)
          {
            Connected = false;
            throw;
          }
          await Task.Delay(cooldown);
        }
      }
      throw new ConnectionException($"Sending message '{message}' has failed", attempts);
    }

    #endregion

    #region protected

    /// <summary>
    /// Attempts to send a message to the endpoint.
    /// </summary>
    /// <param name="message">Message to send to the endpoint.</param>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <returns>The task for sending the message.</returns>
    /// <remarks>Should throw an exception if sending the message has failed.</remarks>
    protected abstract Task SendMessageAsync(T message, CancellationToken token);

    #endregion
  }
  /// <summary>
  /// The PersistentConnection object presents a basic implementation for the IPersistentConnection interface.
  /// </summary>
  public abstract class PersistentConnection : IPersistentConnection
  {
    #region overrides

    /// <summary>
    /// OnConnectionEstablished is raised when the connection is established.
    /// </summary>
    public event Action? OnConnectionEstablished;

    /// <summary>
    /// OnConnectionLost is raised when the connection is lost.
    /// </summary>
    public event Action? OnConnectionLost;

    /// <summary>
    /// Is the connection to the endpoint up?
    /// </summary>
    /// <remarks>Setting this property will raise the OnConnection* events accordingly.</remarks>
    public bool Connected
    {
      protected set
      {
        if (value == connected) return;
        if (value) OnConnectionEstablished?.Invoke();
        else OnConnectionLost?.Invoke();
        connected = value;
      }
      get => connected;
    }

    /// <summary>
    /// Attempts to connect to the endpoint.
    /// </summary>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    public async Task ConnectAsync(int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      for (int i = 1; i <= attempts; i++)
      {
        try 
        {
          using CancellationTokenSource cts = new CancellationTokenSource(timeout);
          await ConnectAsync(cts.Token);
          cts.Dispose();
          Connected = true;
          return;
        }
        catch
        {
          if (i == attempts)
          {
            Connected = false;
            throw;
          }
          await Task.Delay(cooldown);
        }
      }
      throw new ConnectionException("Connection has failed", attempts);
    }

    /// <summary>
    /// Disconnects the connection.
    /// </summary>
    /// <returns>The task for disconnecting.</returns>
    /// <remarks>Overriders must call base method at the end of their implementation.</remarks>
    public virtual Task DisconnectAsync()
    {
      Connected = false;
      return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes of the connection.
    /// </summary>
    public virtual void Dispose() => connected = false;

    #endregion

    #region protected

    /// <summary>
    /// Attempts to connect to the endpoint.
    /// </summary>
    /// <param name="token">Cancellation token for timing the action out.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    /// <remarks>Should throw an exception if connection has failed. Should also do nothing if the connection is already up.</remarks>
    protected abstract Task ConnectAsync(CancellationToken token);

    #endregion

    #region private

    private bool connected = false;

    #endregion
  }
}
