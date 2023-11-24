using Cephei.Assemblies;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cephei.Commands.Consoles
{
  using static ConsoleSystem;

  /// <summary>
  /// The PredefinedCommands contains predefined commands that can be added at will to the system.
  /// </summary>
  public static class PredefinedCommands
  {
    #region public

    /// <summary>
    /// Adds the help command under another command or the console system. The command is defined by the GetHelpCommand property on the ConsoleSystem.
    /// </summary>
    /// <param name="master">Master command to add it under.</param>
    /// <returns>The new Help command.</returns>
    public static Command? AddHelpCommand(Command? master)
      => CreateCommand((excs) => GetHelpCommand(master, excs), false);

    /// <summary>
    /// Creates the Console command family and its subcommands.
    /// </summary>
    /// <param name="master">Master command to add it under.</param>
    /// <returns>The new Console command.</returns>
    /// <exception cref="ApplicationException"></exception>
    public static Command AddConsoleCommands(Command? master)
    {
      Command con = CreateCommand(null, () => "Console command: Console. Has no action.", master, "console", "con")
        ?? throw new ApplicationException("Console command failed to create.");
      Command exit = CreateCommand((x) => Looping = false, () => "Console command: Exit. Exits the console's main loop. Has no arguments.", con, "exit", "end")
        ?? throw new ApplicationException("Exit command failed to create.");
      CreateCommand((x) => new CommandPseudo(exit, null, x), true);
      CreateCommand((x) => Console.Clear(), () => "Console command: Clear. Clears the console window. Has no arguments.", con, "clear", "clr");
      CreateCommand((x) =>
      {
        ConsoleColor c;
        string cs;
        if (x.TryGetValue("fore", out IReadOnlyList<string> args))
        {
          c = GetConsoleColorFromArgument(args, out cs);
          Console.ForegroundColor = c;
          Out.WriteLine("Foreground (letter) color changed to: " + cs + ".");
        }
        if (x.TryGetValue("back", out args))
        {
          c = GetConsoleColorFromArgument(args, out cs);
          Console.BackgroundColor = c;
          Out.WriteLine("Background color changed to: " + cs + ".");
        }
      }, () => "Console Command: Color. Changes the console's color.\n\n"
        + "Arguments:\n[-fore]: The foreground (letter) color. Has the following available colors:"
        + "\n* Black, Dark Blue, Dark Green, Dark Cyan, Dark Red, Dark Magenta, Dark Yellow, Gray, Dark Gray,"
        + "\n* Blue, Green, Cyan, Red, Magenta, Yellow and White."
        + "\n[-back]: The background color. Has the same colors as the foreground.", con, "color", "col");
      CreateCommand((x) => Out.WriteLine(GetInfo()), () => "Console command: Version. Outputs the program's and the console's version. Has no arguments.", con, "info");
      return con;
    }

    #endregion

    #region private

    private static ConsoleColor GetConsoleColorFromArgument(IReadOnlyList<string> args, out string value)
    {
      value = string.Join("", args).ToUpperInvariant();
      return value switch
      {
        "BLACK" => ConsoleColor.Black,
        "DARKBLUE" => ConsoleColor.DarkBlue,
        "DARKGREEN" => ConsoleColor.DarkGreen,
        "DARKCYAN" => ConsoleColor.DarkCyan,
        "DARKRED" => ConsoleColor.DarkRed,
        "DARKMAGENTA" => ConsoleColor.DarkMagenta,
        "DARKYELLOW" => ConsoleColor.DarkYellow,
        "GRAY" => ConsoleColor.Gray,
        "DARKGRAY" => ConsoleColor.DarkGray,
        "BLUE" => ConsoleColor.Blue,
        "GREEN" => ConsoleColor.Green,
        "CYAN" => ConsoleColor.Cyan,
        "RED" => ConsoleColor.Red,
        "MAGENTA" => ConsoleColor.Magenta,
        "YELLOW" => ConsoleColor.Yellow,
        "WHITE" => ConsoleColor.White,
        _ => throw new ArgumentException("Unknown console color: " + value, "value"),
      };
    }

    #endregion
  }
}
