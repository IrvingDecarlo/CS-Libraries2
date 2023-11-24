using System;
using System.Threading.Tasks;

namespace Cephei.Objects
{
  /// <summary>
  /// The HeartbeatDelegate object is a delegate implementation for IHeartbeatable.
  /// </summary>
  public class HeartbeatDelegate : HeartbeatObject
  {
    #region protected

    /// <summary>
    /// Performs the OnHeartbeatEvent.
    /// </summary>
    /// <returns>The event's value or 1 if none is assigned.</returns>
    protected sealed override Task<int> OnHeartbeat() => OnHeartbeatEvent is null ? Task.FromResult(1) : OnHeartbeatEvent.Invoke();

    #endregion

    /// <summary>
    /// Actions to perform for the object's heartbeat.
    /// </summary>
    public event Func<Task<int>>? OnHeartbeatEvent;
  }
}
