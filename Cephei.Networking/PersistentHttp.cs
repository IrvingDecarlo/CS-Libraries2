using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The Persistent Http Connection offers a persistent connection for HttpClients.
  /// </summary>
  /// <remarks>Not all functionalities offered by a PersistentConnection are available for HttpClients.</remarks>
  public abstract class PersistentHttp : PersistentConnection<HttpContent, HttpResponseMessage>
  {
    #region overrides

    /// <summary>
    /// Disposes of the HttpClient.
    /// </summary>
    public override void Dispose()
    {
      base.Dispose();
      GetClient().Dispose();
    }

    /// <summary>
    /// Communicates with the Http webservice. Uses a POST action.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="token">Cancellation token for timeouts.</param>
    /// <returns>The server's response.</returns>
    protected override async Task<HttpResponseMessage> CommunicateAsync(HttpContent message, CancellationToken token)
    {
      HttpResponseMessage msg = await GetClient().PostAsync(GetUri(message), message, token);
      msg.EnsureSuccessStatusCode();
      return msg;
    }

    /// <summary>
    /// Sends an unidirectional message to the server.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="token">Cancellation token for timeouts.</param>
    /// <returns>An exception since Http webservices don't support unidirectional communication.</returns>
    /// <exception cref="NotSupportedException"></exception>
    protected override Task SendMessageAsync(HttpContent message, CancellationToken token)
      => throw new NotSupportedException();

    /// <summary>
    /// Awaiting for messages is not supported for Http connections.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    protected override void StartAwaiting() => throw new NotSupportedException();

    /// <summary>
    /// Stops awaiting for messages.
    /// </summary>
    /// <returns>Task for stopping to await for messages.</returns>
    protected override Task StopAwaiting() => Task.CompletedTask;

    #endregion

    #region protected

    /// <summary>
    /// Gets the clients that will be used for transactions.
    /// </summary>
    /// <returns>The clients that will be used for transactions.</returns>
    protected abstract HttpClient GetClient();

    /// <summary>
    /// Gets the target uri for the defined message.
    /// </summary>
    /// <param name="message">Message that will be sent by the client.</param>
    /// <returns>The target uri for the client's message.</returns>
    protected abstract Uri GetUri(HttpContent message);

    #endregion
  }
}
