﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cephei.Networking
{
  /// <summary>
  /// The NetworkExtensions class contains extension methods for objects related to networking.
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
  }
}
