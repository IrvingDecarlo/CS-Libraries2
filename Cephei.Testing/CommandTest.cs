using Cephei.Commands;
using Cephei.Commands.Consoles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal class CommandTest
  {
    public static void AddCommands()
    {
      Main = CreateCommand(null, null, null, "comtest", "ct");
      CreateCommand((x) =>
      {
        Out.WriteLine("foquo");
        return Task.CompletedTask;
      }, null, Main, "test");
      CreateCommand(async (x) =>
      {
        int delay = x.TryGetValue("delay", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 1000;
        Out.WriteLine("Delaying for " + delay + "ms");
        await Task.Delay(delay);
        Out.WriteLine("Delay complete.");
      }, null, Main, "async");
    }

    public static Command Main;
  }
}
