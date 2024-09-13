using Cephei.Commands.Consoles;
using System.Threading;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal class Program
  {
    static void Main(string[] args)
    {
      Setup();
      CommandTest.AddCommands();
      SocketTest.AddCommands();
      //StatTest.CreateCommands();
      HttpTest.AddCommands();
      MathTest.AddCommands();
      StreamTest.AddCommands();
      ThreadingTest.AddCommands();
      OutputThread();
      MainLoop();
    }

    internal static void OutputThread(Thread thread, bool showalive = false)
      => Out.WriteLine($"Current Thread: ID={thread.ManagedThreadId} Name={thread.Name}" + (showalive ? " Alive=" + thread.IsAlive : ""));
    internal static void OutputThread(bool showalive = false) => OutputThread(Thread.CurrentThread, showalive);
  }
}
