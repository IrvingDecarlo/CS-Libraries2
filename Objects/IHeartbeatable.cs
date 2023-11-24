namespace Cephei.Objects
{
  /// <summary>
  /// IHeartable objects denote an object that repeatedly performs an action.
  /// </summary>
  public interface IHeartbeatable
  {
    /// <summary>
    /// Is the object performing its heartbeat action?
    /// </summary>
    bool Heartbeating { get; }

    /// <summary>
    /// Stops the object's heartbeat.
    /// </summary>
    void HeartbeatStart();

    /// <summary>
    /// Starts the object's heartbeat.
    /// </summary>
    void HeartbeatStop();
  }
}
