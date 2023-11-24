using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The HttpPersistent class denotes a persistent connection to a Http target. Note that Http connections cannot possibly be persistent, therefore this
  /// class is useful for handling faulty connections only.
  /// </summary>
  public abstract class HttpPersistent : PersistentConnectionString
  {
    #region overrides

    /// <summary>
    /// Sends a post request and returns a response to target.
    /// </summary>
    /// <param name="message">Request to send to target.</param>
    /// <returns>The target's response.</returns>
    public override async Task<string> ReceiveResponseAsync(string message)
    {
      HttpResponseMessage response = await GetClient(message).PostAsync(GetUri(message), new StringContent(message));
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Sends a message to target.
    /// </summary>
    /// <param name="msg">Message to be sent.</param>
    /// <returns>An empty task since this is not supported for Http connections.</returns>
    public override Task SendMessageAsync(string msg) => Task.CompletedTask;

    /// <summary>
    /// Tries to connect to target.
    /// </summary>
    /// <returns>True by default since Http connections cannot possibly be persistent.</returns>
    public override Task<bool> TryConnectAsync() => Task.FromResult(true);

    /// <summary>
    /// Closes the connection. Does nothing since connections to a Http target cannot be closed.
    /// </summary>
    public override void Close() { }

    #endregion

    #region protected

    /// <summary>
    /// Gets the HttpClient to use for requests.
    /// </summary>
    /// <param name="message">Message that is to be sent.</param>
    /// <returns>The HttpClient to use for requests.</returns>
    protected abstract HttpClient GetClient(string message);

    /// <summary>
    /// Gets the uri to be used for transactions.
    /// </summary>
    /// <param name="message">Message that is to be sent.</param>
    /// <returns>The uri to be used for transactions.</returns>
    protected abstract Uri GetUri(string message);

    #endregion
  }
}