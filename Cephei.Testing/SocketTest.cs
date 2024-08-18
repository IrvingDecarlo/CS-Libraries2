using Cephei.Commands;
using Cephei.Commands.Consoles;
using Cephei.Networking;
using Cephei.Networking.Servers;
using Cephei.Objects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class SocketTest
  {
    #region internal

    // VARIABLES

    internal static Command Main;

    // METHODS

    internal static void AddCommands()
    {
      Main = CreateCommand(null, null, null, "socket");
      Command cmd = CreateCommand(null, null, Main, "client");
      CreateCommand(async (x) =>
      {
        IPEndPoint ip = IPEndPoint.Parse(x["ip"][0]);
        int attempts = PrepareForTest("Creating client socket to connect to " + ip, x, out TimeSpan cooldown, out TimeSpan timeout);
        socket_client?.Dispose();
        socket_client = new SocketClientTest(ip);
        await socket_client.ConnectAsync(attempts, cooldown, timeout);
        Out.WriteLine("Connection established successfully.");
      }, null, cmd, "connect");
      CreateCommand(async (x) =>
      {
        string msg = x["message"][0];
        int attempts = PrepareForTest("Sending message to server:\n" + msg, x, out TimeSpan cooldown, out TimeSpan timeout);
        await socket_client.SendMessageAsync(msg, attempts, cooldown, timeout);
        Out.WriteLine("Message sent successfully.");
      }, null, cmd, "send");
      CreateCommand((x) => 
      { 
        DisposeClient();
        return Task.CompletedTask;
      }, null, cmd, "dispose");

      cmd = CreateCommand(null, null, Main, "server");
      CreateCommand((x) =>
      {
        IPEndPoint ip = IPEndPoint.Parse(x["ip"][0]);
        Out.WriteLine("Setting up server to " + ip);
        socket_server?.Dispose();
        socket_server = new SocketServerTest() { EndPoint = ip };
        socket_server.Initiate();
        socket_server.Listening = true;
        Out.WriteLine("Server set up successfully.");
        return Task.CompletedTask;
      }, null, cmd, "initiate", "init", "host");
      CreateCommand(async (x) =>
      {
        IPEndPoint ip = IPEndPoint.Parse(x["ip"][0]);
        string msg = x["message"][0];
        int attempts = PrepareForTest("Preparing to send server message...", x, out TimeSpan cooldown, out TimeSpan timeout);
        await socket_server.SendMessage(ip, msg, attempts, cooldown, timeout);
      }, null, cmd, "send");
      CreateCommand(async (x) => await socket_server.RemoveConnection(IPEndPoint.Parse(x["ip"][0])), null, cmd, "disconnect", "remove");
      CreateCommand(async (x) => await socket_server.DisplayConnections(), null, cmd, "list", "connections");
    }

    #endregion

    #region private

    // CLASSES

    private class SocketClientTest : PersistentSocketString
    {
      internal SocketClientTest(EndPoint ep) : base()
      {
        EndPoint = ep;
        OnMessageReceived += SocketClientTest_OnMessageReceived;
        OnConnectionEstablished += SocketClientTest_OnConnectionEstablished;
        OnConnectionLost += SocketClientTest_OnConnectionLost;
        OnListeningDisconnected += DisposeClient;
        OnListeningException += SocketClientTest_OnListeningException;
        AwaitingMessages = true;
      }

      protected override async Task ConnectAsync(CancellationToken token)
      {
        await Socket.ConnectAsync(EndPoint);
        await base.ConnectAsync(token);
      }

      internal readonly EndPoint EndPoint;

      private void WriteClientMessage(string message) => Out.WriteLine("CLIENT: " + message);

      private void SocketClientTest_OnMessageReceived(string msg) => WriteClientMessage("Message received:\n" + msg);

      private void SocketClientTest_OnConnectionEstablished() => WriteClientMessage("Connection established with server.");

      private void SocketClientTest_OnConnectionLost() => WriteClientMessage("Connection with server lost.");

      private void SocketClientTest_OnListeningException(Exception e)
      {
        WriteClientMessage($"Exception thrown by client while listening to messages:");
        ShowException(e);
      }
    }

    private class SocketServerTest : ServerSocket
    {
      internal SocketServerTest() : base()
        => OnConnectionEstablished += AcceptConnection;

      public override void Initiate() => Socket.Bind(EndPoint);

      public override void Dispose()
      {
        connections.Values.DisposeAll();
        base.Dispose();
      }

      protected override void HandleUnhandledSocket(Socket socket)
      {
        WriteServerMessage("Unhandled connection. Socket disposed.");
        base.HandleUnhandledSocket(socket);
      }

      internal EndPoint EndPoint;

      internal Task DisplayConnections()
      {
        WriteServerMessage("Current active connections:");
        Out.WriteLine(string.Join('\n', connections.Keys));
        return Task.CompletedTask;
      }

      internal Task RemoveConnection(EndPoint endpoint)
      {
        WriteServerMessage("Removing connection " + endpoint);
        connections[endpoint].Dispose();
        connections.Remove(endpoint);
        WriteServerMessage("Connection removed successfully.");
        return Task.CompletedTask;
      }

      internal async Task SendMessage(EndPoint endpoint, string message, int attempts, TimeSpan cooldown, TimeSpan timeout)
      {
        WriteServerMessage($"Sending message to {endpoint}:\n" + message);
        await connections[endpoint].Socket.SendAsync(Encoding.UTF8.GetBytes(message), attempts, cooldown, timeout);
        WriteServerMessage("Message sent successfully.");
      }

      internal void WriteServerMessage(string message) => Out.WriteLine("SERVER: " + message);

      private class SocketServerConnection : ListeningSocketString
      {
        internal SocketServerConnection(Socket socket) : base(socket)
        {
          OnMessageReceived += SocketServerConnection_OnMessageReceived;
          OnListeningDisconnected += SocketServerConnection_OnListeningDisconnected;
          OnListeningException += SocketServerConnection_OnListeningException;
          AwaitingMessages = true;
        }

        private void SocketServerConnection_OnListeningException(Exception e)
        {
          socket_server.WriteServerMessage($"Exception thrown by {Socket.RemoteEndPoint} while listening to messages:");
          ShowException(e);
        }

        private void SocketServerConnection_OnListeningDisconnected()
        {
          EndPoint ep = Socket.RemoteEndPoint;
          socket_server.WriteServerMessage($"Server connection terminated ({ep})");
          socket_server.RemoveConnection(ep);
        }

        private void SocketServerConnection_OnMessageReceived(string msg) => socket_server.WriteServerMessage("Message received:\n" + msg);
      }

      private readonly Dictionary<EndPoint, ListeningSocketString> connections = new Dictionary<EndPoint, ListeningSocketString>();

      private void AcceptConnection(Socket socket)
      {
        EndPoint endpoint = socket.RemoteEndPoint;
        WriteServerMessage("New incoming connection: " + endpoint);
        connections.Add(endpoint, new SocketServerConnection(socket));
        WriteServerMessage("Connection successfully established.");
      }
    }

    // VARIABLES

    private static SocketClientTest socket_client;
    private static SocketServerTest socket_server;

    // METHODS

    private static void DisposeClient()
    {
      socket_client.Dispose();
      socket_client = null;
      Out.WriteLine("Client connection terminated.");
    }

    private static int GetConnectionParams(IReadOnlyDictionary<string, IReadOnlyList<string>> x, out TimeSpan cooldown, out TimeSpan timeout)
    {
      cooldown = TimeSpan.FromMilliseconds(x.TryGetValue("cooldown", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 1000);
      timeout = TimeSpan.FromMilliseconds(x.TryGetValue("timeout", out args) ? int.Parse(args[0]) : 5000);
      return x.TryGetValue("attempts", out args) ? int.Parse(args[0]) : 1;
    }

    private static int PrepareForTest(string msg, IReadOnlyDictionary<string, IReadOnlyList<string>> x, out TimeSpan cooldown, out TimeSpan timeout)
    {
      int attempts = GetConnectionParams(x, out cooldown, out timeout);
      Out.WriteLine(msg);
      WriteParams(attempts, cooldown, timeout);
      return attempts;
    }

    private static void WriteParams(int attempts, TimeSpan cooldown, TimeSpan timeout)
      => Out.WriteLine($"Attempts={attempts}, Cooldown={cooldown.TotalMilliseconds}ms, Timeout={timeout.TotalMilliseconds}ms");

    #endregion
  }
}