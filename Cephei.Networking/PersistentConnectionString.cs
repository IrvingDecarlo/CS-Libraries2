using Cephei.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The PersistentConnectionString is a PersistentConnection that operates with strings.
  /// </summary>
  public abstract class PersistentConnectionString : PersistentConnection, IPersistentConnection<string>
  {
    #region overrides

    /// <summary>
    /// Makes the socket communicate with its endpoint, sending a message and receiving another.
    /// </summary>
    /// <param name="message">Message to be sent to the endpoint.</param>
    /// <param name="attempts">Number of communication attempts until communication is aborted.</param>
    /// <param name="retrydelay">Delay between retry attempts.</param>
    /// <param name="expheader">Expected header to receive. Can be "" to disable header validation.</param>
    /// <param name="minlen">Minimum response length.</param>
    /// <param name="maxlen">Maximum response length.</param>
    /// <returns>The message received from the endpoint. Returns "" on error.</returns>
    public async Task<string> CommunicateAsync(string message, int attempts, int retrydelay, string expheader, int minlen, int maxlen)
    {
      ILogger? logger = GetLogger();
      int headlen = expheader.Length;
      bool hasheader = headlen > 0;
      string msg = "";
      int cases = 0;
      string header;
      logger?.LogDetail($"Attempting to send message [{message}] (Attempts={attempts}, RetryDelay={retrydelay}, ExpectedHeader={expheader}, MinimumLength={minlen}, MaximumLength={maxlen})...");
      for (int i = 1; i <= attempts; i++)
      {
        logger?.LogDetail($"Communication attempt number {i}...");
        if (!await TryConnectAsync()) continue;
        try { await SendMessageAsync(message); }
        catch (Exception e)
        {
          await OnCommunicationFailure(e, "Failed to send message", retrydelay);
          continue;
        }
        logger?.LogDetail("Message sent successfully. Awaiting reply...");
        try { msg = await ReceiveResponseAsync(message); }
        catch (Exception e)
        {
          await OnCommunicationFailure(e, "Failed to receive response", retrydelay);
          continue;
        }
        cases = msg.Length;
        logger?.LogDetail("Response received: " + msg);
        if (cases < minlen)
        {
          await OnCommunicationFailure(new InvalidDataException($"The response's length ({cases}) does not match the minimum requirement ({minlen}).")
            , "Communication failed", retrydelay);
          msg = "";
          continue;
        }
        if (cases > maxlen)
        {
          await OnCommunicationFailure(new InvalidDataException($"The response's length ({cases}) does not match the maximum requirement ({maxlen}).")
            , "Communication failed", retrydelay);
          msg = "";
          continue;
        }
        if (hasheader)
        {
          if (cases < headlen)
          {
            await OnCommunicationFailure(
              new InvalidDataException($"The received message does not contain enough cases ({cases}) to fit the expected header size ({headlen}).")
              , "Communication failed", retrydelay);
            msg = "";
            continue;
          }
          header = msg[..headlen];
          if (!header.Equals(expheader))
          {
            await OnCommunicationFailure(new InvalidDataException($"The received header '{header}' does not match the expected header '{expheader}'.")
              , "Communication failed", retrydelay);
            msg = "";
            continue;
          }
        }
        logger?.LogInfo("Communication successful.");
        break;
      }
      Connected = cases > 0;
      OnPostCommunication?.Invoke(msg);
      return msg;
    }

    /// <summary>
    /// Receives the target's reply.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns>The target's reply.</returns>
    public abstract Task<string> ReceiveResponseAsync(string message);

    /// <summary>
    /// Sends the message to its target.
    /// </summary>
    /// <remarks>Do not handle exceptions within this method since they are useful for determining fail states when communicating with its target.</remarks>
    /// <param name="msg">Message to be sent.</param>
    public abstract Task SendMessageAsync(string msg);

    #endregion

    #region protected

    /// <summary>
    /// OnPostCommunication is called after the communication is complete, regardless of being successful or not. The produced message are outputted to the event.
    /// </summary>
    protected event Action<string>? OnPostCommunication;

    #endregion
  }
}
