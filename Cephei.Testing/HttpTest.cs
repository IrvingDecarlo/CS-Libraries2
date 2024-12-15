using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Cephei.Commands;
using Cephei.Commands.Consoles;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class HttpTest
  {
    public static void AddCommands() 
    {
      Main = CreateCommand(null, null, null, "http"); 
      CreateCommand(async (x) => 
      {
        HttpContent content;
        if (x.TryGetValue("message", out IReadOnlyList<string> args)) content = new StringContent(args[0]);
        else
        {
          using StreamReader stream = new StreamReader(x["file"][0]);
          content = new StringContent(stream.ReadToEnd());
        }
        HttpResponseMessage msg = await client.PostAsync(x["uri"][0], content);
        Out.WriteLine($"Message sent: {msg.RequestMessage}\n");
        Out.WriteLine($"Status Code={msg.StatusCode} Successful={msg.IsSuccessStatusCode}");
        Out.WriteLine($"Reason Phrase: {msg.ReasonPhrase}\n");
        Out.WriteLine($"Content: {await msg.Content.ReadAsStringAsync()}\n");
      }, null, Main, "post");
    }

    public static Command Main;

    #region private

    private static readonly Dictionary<string, HttpClient> clients = new Dictionary<string, HttpClient>();

    private static readonly HttpClient client = new HttpClient();

    private static HttpClient GetClient(IReadOnlyDictionary<string, IReadOnlyList<string>> x) => GetClient(x["uri"][0]);
    private static HttpClient GetClient(string uri)
    {
      if (clients.TryGetValue(uri, out HttpClient client)) return client;
      client = new HttpClient()
      {
        BaseAddress = new Uri(uri)
      };
      clients.Add(uri, client);
      return client;
    }

    #endregion
  }
}
