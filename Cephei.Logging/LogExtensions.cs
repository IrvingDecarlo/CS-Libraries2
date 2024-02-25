using System;

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
    /// Logs a critical message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogCritical(this ILogger logger, string message) => logger.Log(message, LogLevel.Critical);

    /// <summary>
    /// Logs an exception message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogException(this ILogger logger, string message) => logger.Log(message, LogLevel.Exception);
    /// <summary>
    /// Logs an exception to the logger.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="e">Exception to output.</param>
    /// <param name="level">The message's level.</param>
    /// <param name="showstack">Show the stack trace?</param>
    public static void LogException(this ILogger logger, Exception e, LogLevel level = LogLevel.Exception, bool showstack = false) 
      => logger.Log(e.GetType().ToString() + ": " + e.Message + (showstack ? "\n" + e.StackTrace : ""), level);

    /// <summary>
    /// Logs a crash message.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="message">Message to output.</param>
    public static void LogCrash(this ILogger logger, string message) => logger.Log(message, LogLevel.Crash);
    /// <summary>
    /// Logs a crash exception to the logger.
    /// </summary>
    /// <param name="logger">Logger to use.</param>
    /// <param name="e">Exception to output.</param>
    /// <param name="showstack">Show the exception's stack trace?</param>
    public static void LogCrash(this ILogger logger, Exception e, bool showstack = false) => logger.LogException(e, LogLevel.Crash, showstack);
  }
}
