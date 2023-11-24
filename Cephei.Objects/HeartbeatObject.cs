using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Objects
{
  /// <summary>
  /// HeartbeatObject is a simple implementation for IHeartbeatable.
  /// </summary>
  public abstract class HeartbeatObject : IHeartbeatable
  {
    #region overrides

    /// <summary>
    /// Is the object performing its heartbeat action?
    /// </summary>
    public bool Heartbeating { get; private set; } = false;

    /// <summary>
    /// Starts the object's heartbeat.
    /// </summary>
    public async void HeartbeatStart()
    {
      if (Heartbeating) return;
      Heartbeating = true;
      OnHeartbeatStart?.Invoke();
      cancel_source = new CancellationTokenSource();
      try { while (Heartbeating) await Task.Delay(await OnHeartbeat(), cancel_source.Token); }
      catch (TaskCanceledException) { }
      finally
      {
        cancel_source.Dispose();
        cancel_source = null;
      }
    }

    /// <summary>
    /// Stops the object's heartbeat.
    /// </summary>
    public void HeartbeatStop()
    { 
      if (!Heartbeating) return;
      if (cancel_source is null) throw new NullReferenceException(nameof(cancel_source));
      cancel_source.Cancel();
      Heartbeating = false;
      OnHeartbeatStop?.Invoke();
    }

    #endregion

    #region public

    // EVENTS

    /// <summary>
    /// Additional actions to take when heartbeat starts.
    /// </summary>
    public event Action? OnHeartbeatStart;

    /// <summary>
    /// Additional actions to take when heartbeat stops.
    /// </summary>
    public event Action? OnHeartbeatStop;

    #endregion

    #region protected

    /// <summary>
    /// Action to take when the object heartbeats.
    /// </summary>
    /// <returns>Delay for the next heartbeat (in miliseconds).</returns>
    protected abstract Task<int> OnHeartbeat();

    #endregion

    #region private

    private CancellationTokenSource? cancel_source;

    #endregion
  }
}
