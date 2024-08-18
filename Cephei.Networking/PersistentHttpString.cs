using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The Persistent Http Connection offers a persistent connection for HttpClients while also working with strings.
  /// </summary>
  /// <remarks>Not all functionalities offered by a PersistentConnection are available for HttpClients.</remarks>
  public abstract class PersistentHttpString : PersistentHttp, IPersistentConnection<string, string>
  {
    #region overrides

    /// <summary>
    /// OnMessageReceived is raised when a message is received while the connection is awaiting messages.
    /// </summary>
    public new event Action<string>? OnMessageReceived;

    /// <summary>
    /// Attempts to communicate, sending a message and then awaiting for a response.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>The message received.</returns>
    public async Task<string> CommunicateAsync(string message, int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      using HttpResponseMessage msg = await CommunicateAsync(new StringContent(message), attempts, cooldown, timeout);
      return await msg.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Tries to send a message.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="attempts">Number of attempts to perform.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for each attempt.</param>
    /// <returns>An exception since Http webservices don't support unidirectional communication.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public Task SendMessageAsync(string message, int attempts, TimeSpan cooldown, TimeSpan timeout)
      => throw new NotSupportedException();

    #endregion
  }
}
