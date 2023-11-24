namespace Cephei.Logging
{
  /// <summary>
  /// The LogLevel enum denotes the possible log levels a logger can output. The higher the level, the less information will be output.
  /// </summary>
  public enum LogLevel : byte
  {
    /// <summary>
    /// Denotes a LogLevel where all messages will be outputted.
    /// </summary>
    All,
    /// <summary>
    /// Denotes a level where debug messages will be outputted.
    /// </summary>
    Debug,
    /// <summary>
    /// Denotes a level where information messages will be outputted.
    /// </summary>
    Information,
    /// <summary>
    /// Denotes a level where exceptions will be outputted.
    /// </summary>
    Exception,
    /// <summary>
    /// Denotes a level where crash messages will be outputted.
    /// </summary>
    Crash,
    /// <summary>
    /// Denotes a level where no messages will be outputted.
    /// </summary>
    None
  }
}
