using Cephei.Commands;
using Cephei.Commands.Consoles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class ThreadingTest
  {
    #region internal

    // VARIABLES

    internal static Command Main;

    // METHODS

    internal static void AddCommands()
    {
      op.OnThreadCheck += Op_OnThreadCheck;
      Main = CreateCommand(null, null, Command.Root, "threading", "thread");
      Command cmd = CreateCommand((x) =>
      {
        CancellationTokenSource cts = PrepareForThreadTest(x, out int cd);
        thread = new Thread(() => op.CheckThread(cd, cts.Token)) { IsBackground = true, Name = "ThreadTest" };
        BasicThreadTesting(cts);
        return Task.CompletedTask;
      }, null, Main, "test");
      CreateCommand((x) =>
      {
        CancellationTokenSource cts = PrepareForThreadTest(x, out int cd);
        thread = new Thread(() => op.CheckThreadAsync(cd, cts.Token)) { IsBackground = true, Name = "ThreadTest" };
        BasicThreadTesting(cts);
        return Task.CompletedTask;
      }, null, cmd, "async");
    }

    #endregion

    #region private

    // CLASSES

    private class ThreadOperator
    {
      internal event Action OnThreadCheck;

      internal void CheckThread(int cooldown, CancellationToken token)
      {
        while (!token.IsCancellationRequested)
        {
          Out.WriteLine("Thread test:");
          Program.OutputThread();
          OnThreadCheck?.Invoke();
          Thread.Sleep(cooldown);
        }
        Out.WriteLine("Thread checking process finalized.");
      }

      internal async void CheckThreadAsync(int cooldown, CancellationToken token)
      {
        while (!token.IsCancellationRequested)
        {
          Out.WriteLine("Thread test:");
          Program.OutputThread();
          OnThreadCheck?.Invoke();
          await Task.Delay(cooldown);
        }
        Out.WriteLine("Thread checking process finalized.");
      }
    }

    // VARIABLES

    private static Thread thread;
    private readonly static ThreadOperator op = new ThreadOperator();

    // METHODS

    private static CancellationTokenSource PrepareForThreadTest(IReadOnlyDictionary<string, IReadOnlyList<string>> x, out int cd)
    {
      Out.WriteLine("Initiating thread test...");
      Out.WriteLine("Current thread:");
      Program.OutputThread();
      cd = x.TryGetValue("cooldown", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 1000;
      Out.WriteLine($"Starting ThreadTest: Cooldown={cd}ms");
      return new CancellationTokenSource();
    }

    private static void BasicThreadTesting(CancellationTokenSource cts)
      => LoopThreadTesting(cts, (x) =>
      {
        switch (x)
        {
          case ConsoleKey.Escape: return false;
          case ConsoleKey.I: DisplayThreadInfo(); break;
        }
        return true;
      }, "Testing initiated. Press <ESC> to stop, <I> for Thread Info.");

    private static void LoopThreadTesting(CancellationTokenSource cts, Func<ConsoleKey, bool> ckcheck, string text)
    {
      Out.WriteLine("Thread for testing:");
      Program.OutputThread(thread);
      Out.WriteLine(text);
      thread.Start();
      bool loop = true;
      while (loop) loop = ckcheck(Console.ReadKey(true).Key);
      Out.WriteLine("Loop terminated. Cancelling thread checking...");
      cts.Cancel();
      cts.Dispose();
      Out.WriteLine("Thread checking cancelled.");
    }

    private static void DisplayThreadInfo()
    {
      Out.WriteLine("Thread info:");
      Program.OutputThread(thread, true);
    }

    private static void Op_OnThreadCheck()
    {
      Out.WriteLine("Event thread information:");
      Program.OutputThread();
    }

    #endregion
  }
}
