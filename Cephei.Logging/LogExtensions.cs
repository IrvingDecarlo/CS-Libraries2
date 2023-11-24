namespace Cephei.Logging
{
  /// <summary>
  /// The LogExtensions class contains extension methods related to logging.
  /// </summary>
  public static class LogExtensions
  {
    /// <summary>
    /// Gets a string identifying the LogLevel's type.
    /// </summary>
    /// <param name="loglevel">LogLevel to use as reference.</param>
    /// <returns>The LogLevel as string.</returns>
    public static string GetIdentifier(this LogLevel loglevel)
      => loglevel switch
      {
        LogLevel.All => "DETAIL",
        LogLevel.Debug => "DEBUG",
        LogLevel.Information => "INFO",
        LogLevel.Exception => "EXCEPTION",
        LogLevel.Crash => "CRASH",
        _ => "UNIDENTIFIED",
      };

    /// <summary>
    /// Logs a detail message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogDetail(this ILogger logger, string message) => logger.Log(message, LogLevel.All);

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogDebug(this ILogger logger, string message) => logger.Log(message, LogLevel.Debug);

    /// <summary>
    /// Logs an information message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogInfo(this ILogger logger, string message) => logger.Log(message, LogLevel.Information);

    /// <summary>
    /// Logs an exception message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogException(this ILogger logger, string message) => logger.Log(message, LogLevel.Exception);

    /// <summary>
    /// Logs a crash message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogCrash(this ILogger logger, string message) => logger.Log(message, LogLevel.Crash);
  }
}
