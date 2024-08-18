using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The NetworkExtensions class contains extension methods for many networking-related functionalities.
  /// </summary>
  public static class NetworkExtensions
  {

    /// <summary>
    /// Downloads data from an uri and saves it in the specified path.
    /// </summary>
    /// <param name="client">HttpClient to use to download data.</param>
    /// <param name="uri">Uri to download the data from.</param>
    /// <param name="path">Path to save the data to.</param>
    /// <returns>True if a valid (not-empty) data was downloaded.</returns>
    /// <remarks>Exceptions may be thrown. Be sure to protect the code with try/catch blocks.</remarks>
    public static async Task<bool> DownloadAsync(this HttpClient client, string uri, string path)
    {
      using Stream stream = await client.GetStreamAsync(uri);
      int byt = stream.ReadByte();
      if (byt < 0) return false;
      using BinaryWriter writer = new BinaryWriter(File.OpenWrite(path));
      while (byt > -1)
      {
        writer.Write(byt);
        byt = stream.ReadByte();
      }
      return true;
    }

    /// <summary>
    /// Sends a packet to the socket's destination.
    /// </summary>
    /// <param name="socket">Socket to use for transferring data.</param>
    /// <param name="msg">Message to send.</param>
    /// <param name="attempts">Attempts before ultimately aborting the operation.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for sending the packet.</param>
    /// <param name="flags">Socket flags to use.</param>
    /// <returns>The task for sending the packet.</returns>
    /// <remarks>Upon failing on the last attempt, the generated exception will be thrown.</remarks>
    public static async Task SendAsync(this Socket socket, ReadOnlyMemory<byte> msg, int attempts, TimeSpan cooldown, TimeSpan timeout, SocketFlags flags = SocketFlags.None)
    {
      if (!socket.Connected) throw new SocketException((int)SocketError.NotConnected);
      for (int i = 1; i <= attempts; i++)
      {
        try
        {
          await socket.SendAsync(msg, timeout, flags);
          return;
        }
        catch
        {
          if (attempts == i) throw;
          await Task.Delay(cooldown);
        }
      }
    }
    /// <summary>
    /// Sends a packet to the socket's destination.
    /// </summary>
    /// <param name="socket">Socket to use for transferring data.</param>
    /// <param name="msg">Message to send.</param>
    /// <param name="timeout">Timeout for sending the packet.</param>
    /// <param name="flags">Socket flags to use.</param>
    /// <returns>The task for sending the packet.</returns>
    public static async Task SendAsync(this Socket socket, ReadOnlyMemory<byte> msg, TimeSpan timeout, SocketFlags flags = SocketFlags.None)
    {
      using CancellationTokenSource cts = new CancellationTokenSource(timeout);
      await socket.SendAsync(msg, flags, cts.Token);
      cts.Dispose();
    }

    /// <summary>
    /// Connects a socket to an endpoint.
    /// </summary>
    /// <param name="socket">Socket to use for connecting.</param>
    /// <param name="endpoint">Endpoint to connect to.</param>
    /// <param name="attempts">Attempts before ultimately aborting the operation.</param>
    /// <param name="cooldown">Cooldown between attempts.</param>
    /// <param name="timeout">Timeout for connecting.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    public static async Task ConnectAsync(this Socket socket, EndPoint endpoint, int attempts, TimeSpan cooldown, TimeSpan timeout)
    {
      if (socket.Connected) throw new SocketException((int)SocketError.IsConnected);
      for (int i = 1; i <= attempts; i++)
      {
        try
        {
          await socket.ConnectAsync(endpoint, timeout);
          return;
        }
        catch
        {
          if (attempts == i) throw;
          await Task.Delay(cooldown);
        }
      }
    }
    /// <summary>
    /// Connects a socket to an endpoint.
    /// </summary>
    /// <param name="socket">Socket to use for connecting.</param>
    /// <param name="endpoint">Endpoint to connect to.</param>
    /// <param name="timeout">Timeout for connecting.</param>
    /// <returns>The task for connecting to the endpoint.</returns>
    public static async Task ConnectAsync(this Socket socket, EndPoint endpoint, TimeSpan timeout)
    {
      using CancellationTokenSource cts = new CancellationTokenSource(timeout);
      await socket.ConnectAsync(endpoint);
      cts.Dispose();
    }
  }
}
