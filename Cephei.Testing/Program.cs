using Cephei.Commands.Consoles;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  class Program
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
      MainLoop();
    }
  }
}
