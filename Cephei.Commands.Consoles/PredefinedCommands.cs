using Cephei.Assemblies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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
    public static Command? AddHelpCommand(CommandReference master)
      => CreateCommand((excs) => GetHelpCommand(master, excs), false);

    /// <summary>
    /// Creates the Console command family. Does not generate its subcommands along with it.
    /// </summary>
    /// <param name="cmd">Command to place the console command family under.</param>
    /// <returns>The new Console command.</returns>
    /// <exception cref="ApplicationException"></exception>
    public static Command AddConsoleCommand(CommandReference cmd) => CreateCommand(null, () => "Console command: Console. Has no action.", cmd, "console", "con")
      ?? throw new ApplicationException("Console command failed to create.");

    /// <summary>
    /// Creates the Console command family and its subcommands.
    /// </summary>
    /// <param name="master">Master command to add its subcommands under.</param>
    /// <exception cref="ApplicationException"></exception>
    public static void AddConsoleCommands(CommandReference master)
    {
      List<CommandAlreadyExistsException> excs = new List<CommandAlreadyExistsException>();
      CreateCommand((x) =>
      {
        Looping = false;
        return Task.CompletedTask;
      }, () => "Console command: Exit. Exits the console's main loop. Has no arguments.", master, excs, "exit", "end");
      CreateCommand((x) =>
      {
        Console.Clear();
        return Task.CompletedTask;
      }, () => "Console command: Clear. Clears the console window. Has no arguments.", master, excs, "clear", "clr");
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
        return Task.CompletedTask;
      }, () => "Console Command: Color. Changes the console's color.\n\n"
        + "Arguments:\n[-fore]: The foreground (letter) color. Has the following available colors:"
        + "\n* Black, Dark Blue, Dark Green, Dark Cyan, Dark Red, Dark Magenta, Dark Yellow, Gray, Dark Gray,"
        + "\n* Blue, Green, Cyan, Red, Magenta, Yellow and White."
        + "\n[-back]: The background color. Has the same colors as the foreground.", master, excs, "color", "col");
      CreateCommand((x) =>
      {
        Out.WriteLine(GetInfo());
        return Task.CompletedTask;
      }, () => "Console command: Information. Outputs the program's and the console's version. Has no arguments.", master, excs, "info");
      Command cmd = CreateCommand((x) =>
      {
        Out.WriteLine(Directory.GetCurrentDirectory());
        return Task.CompletedTask;
      }, () => "Console command: Working directory. Outputs the application's working directory. Has no arguments.", master, excs, "workingdirectory", "workdir")
        ?? throw new NullReferenceException("Working Directory command failed to create.");
      CreateCommand((x) =>
      {
        string path = x["path"][0];
        Directory.SetCurrentDirectory(path);
        Out.WriteLine("Working directory set to: " + path);
        return Task.CompletedTask;
      }, () => "Console command: Set working directory. Sets the application's working directory to the specified path.\n\n"
        + "Arguments:\n-path: The path to set the working directory to.", cmd, excs, "set");
      CreateCommand(async (x) =>
      {
        string path = x["path"][0];
        using Stream stream = File.OpenRead(path);
        TextWriter ot = Out;
        ot.WriteLine($"Executing commands from file '{path}'...");
        using StreamReader reader = new StreamReader(stream);
        string cmd;
        while (!reader.EndOfStream)
        {
          cmd = (await reader.ReadLineAsync()).Trim();
          if (string.IsNullOrEmpty(cmd)) continue;
          ot.WriteLine();
          ot.WriteLine(">" + cmd);
          try { await Command.Run(cmd); }
          catch (Exception e) { ShowException(e); }
        }
        ot.WriteLine();
        ot.WriteLine("File command execution finished.");
      }, () => "Console command: Run. Runs a list of commands from a text file.\n\n"
        + "Arguments:\n-path: Path to the file containing the list of commands.", master, excs, "run");
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
