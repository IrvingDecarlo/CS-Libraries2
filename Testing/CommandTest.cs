using Cephei.Commands;
using Cephei.Commands.Consoles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal class CommandTest
  {
    public static void AddCommands()
    {
      Main = CreateCommand(null, null, null, "comtest", "ct");
      CreateCommand((x) => Console.WriteLine("foquo"), null, Main, "test");
    }

    public static Command Main;
  }
}
