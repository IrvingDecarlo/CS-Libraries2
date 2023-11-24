using Cephei.Commands;
using Cephei.Commands.Consoles;
using Cephei.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
/*
namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class SocketTest
  {
    #region internal

    internal static void AddCommands()
    {
      TextWriter writer = Console.Out;
      writer.WriteLine("Adding networking commands...");

      CreateCommand((x) =>
      {
        if (x.TryGetValue("set", out IReadOnlyList<string> args)) timeout = int.Parse(args[0]);
        if (x.TryGetValue("send", out args)) sendtimeout = int.Parse(args[0]);
        if (x.TryGetValue("rec", out args)) rectimeout = int.Parse(args[0]);
        OutputTimeouts();
      }, null, Main, "timeout");

      CreateCommand((x) =>
      {
        if (x.TryGetValue("set", out IReadOnlyList<string> args)) endpoint = IPEndPoint.Parse(args[0]);
        OutputEndpoint();
      }, null, Main, "endpoint");

      CreateCommand((x) =>
      {
        if (x.TryGetValue("set", out IReadOnlyList<string> args)) comattempts = int.Parse(args[0]);
        if (x.TryGetValue("delay", out args)) comdelay = int.Parse(args[0]);
        OutputConnectionAttempts();
      }, null, Main, "attempts");

      CreateCommand((x) => { tester.TryConnect(); }, null, Main, "connect");

      CreateCommand(async (x) =>
      {
        string header = GetCommunicationParameters(x, out int minlen, out int maxlen);
        await tester.Communicate(string.Join(' ', x["message"]), header, minlen, maxlen);
      }, null, Main, "communicate", "send", "comm");

      CreateCommand((x) =>
      {
        string header = GetCommunicationParameters(x, out int minlen, out int maxlen);
        tester.StartPoll(int.Parse(x["delay"][0]), string.Join(' ', x["message"]), header, minlen, maxlen);
      }, null, Main, "poll");

      CreateCommand((x) => tester.StopPoll(), null, Main, "stop");

      writer.WriteLine("Tester configured as:");
      OutputEndpoint();
      OutputTimeouts();
      OutputConnectionAttempts();
    }

    #endregion

    #region private

    // CLASSES

    private class NetworkTester : SocketPolling
    {
      internal NetworkTester() => writer = new StreamWriter(@"F:\temp\polltest.log");

      protected override Task ConnectAsync(Socket socket) => socket.ConnectAsync(endpoint);

      protected override byte[] GetBuffer() => new byte[1024];

      protected override TextWriter GetLogWriter() => writer;

      protected override Socket GetSocket() => new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      protected override Task<bool> TryConnectAsync() => TryConnectAsync(timeout, sendtimeout, rectimeout);

      protected override async Task<int> PollCommunicateAsync()
      {
        await CommunicateAsync(poll_message, comattempts, comdelay, poll_header, poll_minlen, poll_maxlen);
        return poll_delay;
      }

      protected override void OnDispose() => writer.Dispose();

      internal void TryConnect() => TryConnectAsync();

      internal async Task<string> Communicate(string message, string header, int minlen, int maxlen) 
        => await CommunicateAsync(message, comattempts, comdelay, header, minlen, maxlen);

      internal void StartPoll(int delay, string message, string header, int minlen, int maxlen)
      {
        poll_delay = delay;
        poll_header = header;
        poll_minlen = minlen;
        poll_maxlen = maxlen;
        poll_message = message;
        PollStart();
      }

      internal void StopPoll() => PollStop();

      private string poll_message, poll_header;
      private int poll_minlen, poll_maxlen, poll_delay;

      private TextWriter writer;
    }

    #endregion

    #region internal static

    internal static readonly Command Main = CreateCommand(null, null, null, "socket");

    #endregion

    #region private static

    // VARIABLES

    private static readonly NetworkTester tester = new NetworkTester();
    private static IPEndPoint endpoint = new IPEndPoint(new IPAddress(new byte[4] { 201, 33, 27, 6 }), 7054);
    private static int timeout = 5000, sendtimeout = 5000, rectimeout = 5000, comattempts = 3, comdelay = 500;

    // METHODS

    private static string GetCommunicationParameters(IReadOnlyDictionary<string, IReadOnlyList<string>> x, out int minlen, out int maxlen)
    {
      if (x.TryGetValue("minlen", out IReadOnlyList<string> args)) minlen = int.Parse(args[0]);
      else minlen = 0;
      if (x.TryGetValue("maxlen", out args)) maxlen = int.Parse(args[0]);
      else maxlen = int.MaxValue;
      if (x.TryGetValue("header", out args)) return args[0];
      else return "";
    }

    private static void OutputTimeouts()
    {
      TextWriter writer = Console.Out;
      writer.WriteLine("Timeout: " + timeout.ToString());
      writer.WriteLine("Send Timeout: " + sendtimeout.ToString());
      writer.WriteLine("Receive Timeout: " + rectimeout.ToString());
    }

    private static void OutputEndpoint() => Console.Out.WriteLine("Endpoint: " + endpoint.ToString());

    private static void OutputConnectionAttempts()
    {
      TextWriter writer = Console.Out;
      writer.WriteLine("Connection Attempts: " + comattempts.ToString());
      writer.WriteLine("Reconnection Delay: " + comdelay.ToString());
    }

    #endregion
  }
}
*/