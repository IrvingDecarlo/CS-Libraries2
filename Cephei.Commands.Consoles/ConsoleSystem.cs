using Cephei.Assemblies;
using Cephei.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Cephei.Commands.Consoles
{
  /// <summary>
  /// The ConsoleSystem class is the core for the console command system.
  /// </summary>
  public static class ConsoleSystem
  {
    static ConsoleSystem()
    {
      @in = Console.In;
      @out = Console.Out;
      Inputs = new InputCollection();
      Exceptions = new ExceptionCollection();
      get_help = (x, y) => new CommandHelp(x, y, "help");
      get_info = () => "Program: " + Assembly.GetEntryAssembly().ToDynamicString()
        + $"\nConsole version: {Assembly.GetExecutingAssembly().ToDynamicString(false, false)} ({Assembly.GetAssembly(typeof(Command)).ToDynamicString(false, false)})";
    }

    #region public static

    // PROPERTIES

    /// <summary>
    /// The function that generates help commands via CreateCommand.
    /// </summary>
    public static Func<CommandReference, IList<CommandAlreadyExistsException>, Command> GetHelpCommand
    {
      set
      {
        if (!modif) throw new SystemNotModifiableException();
        get_help = value;
      }
      get => get_help;
    }

    /// <summary>
    /// Function that returns the system's info.
    /// </summary>
    public static Func<string> GetInfo
    {
      set
      {
        if (!modif) throw new SystemNotModifiableException();
        get_info = value;
      }
      get => get_info;
    }

    /// <summary>
    /// Gets or sets the console's current reader.
    /// </summary>
    /// <exception cref="SystemNotModifiableException"></exception>
    public static TextReader In
    {
      set
      {
        if (!modif) throw new SystemNotModifiableException();
        @in = value;
      }
      get => @in;
    }

    /// <summary>
    /// Gets or sets the console's current writer.
    /// </summary>
    /// <exception cref="SystemNotModifiableException"></exception>
    public static TextWriter Out
    {
      set
      {
        if (!modif) throw new SystemNotModifiableException();
        @out = value;
      }
      get => @out;
    }

    /// <summary>
    /// Is the system modifiable? Once set to false then it won't be able to be reverted back.
    /// </summary>
    /// <exception cref="SystemNotModifiableException"></exception>
    public static bool Modifiable
    {
      set
      {
        if (!modif) throw new SystemNotModifiableException();
        modif = value;
      }
      get => modif;
    }

    // VARIABLES

    /// <summary>
    /// Is the collection of exceptions that have occurred during the console's execution.
    /// </summary>
    public static readonly ExceptionCollection Exceptions;

    /// <summary>
    /// Is the console's input history.
    /// </summary>
    public static readonly InputCollection Inputs;

    /// <summary>
    /// Defines whether the system is in the main loop, capturing user input. Will stop the MainLoop once set to false.
    /// </summary>
    public static bool Looping;

    /// <summary>
    /// Show stack traces when exceptions occur?
    /// </summary>
    public static bool ShowStack = false;

    // METHODS

    /// <summary>
    /// Creates a new delegate command, handling all exceptions automatically and outputting them to the console. A help command is automatically added under it.
    /// </summary>
    /// <param name="action">Action for the command to perform.</param>
    /// <param name="descriptor">Descriptor that is used to provide the command's description.</param>
    /// <param name="master">Master command to assign the new one under.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static CommandDelegate? CreateCommand(CommandDelegate.Executor? action, Func<string>? descriptor, CommandReference master, params string[] idents)
      => CreateCommand(action, descriptor, master, true, idents);
    /// <summary>
    /// Creates a new delegate command, handling all exceptions automatically and outputting them to the console. A help command is automatically added under it.
    /// </summary>
    /// <param name="action">Action for the command to perform.</param>
    /// <param name="descriptor">Descriptor that is used to provide the command's description.</param>
    /// <param name="master">Master command to assign the new one under.</param>
    /// <param name="exceptions">List of exceptions to use for outputting exceptions. Will be cleared upon execution.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static CommandDelegate? CreateCommand(CommandDelegate.Executor? action, Func<string>? descriptor, CommandReference master
      , IList<CommandAlreadyExistsException> exceptions, params string[] idents)
      => CreateCommand(action, descriptor, master, true, exceptions, idents);
    /// <summary>
    /// Creates a new delegate command, handling all exceptions automatically and outputting them to the console.
    /// </summary>
    /// <param name="action">Action for the command to perform.</param>
    /// <param name="descriptor">Descriptor that is used to provide the command's description.</param>
    /// <param name="master">Master command to assign the new one under.</param>
    /// <param name="help">Create a help command under the newly created command?</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static CommandDelegate? CreateCommand(CommandDelegate.Executor? action, Func<string>? descriptor, CommandReference master, bool help, params string[] idents)
      => CreateCommand((exceptions) => new CommandDelegate(action, descriptor, master, exceptions, idents), help);
    /// <summary>
    /// Creates a new delegate command, handling all exceptions automatically and outputting them to the console.
    /// </summary>
    /// <param name="action">Action for the command to perform.</param>
    /// <param name="descriptor">Descriptor that is used to provide the command's description.</param>
    /// <param name="master">Master command to assign the new one under.</param>
    /// <param name="help">Create a help command under the newly created command?</param>
    /// <param name="exceptions">List of exceptions to use for outputting exceptions. Will be cleared upon execution.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static CommandDelegate? CreateCommand(CommandDelegate.Executor? action, Func<string>? descriptor, CommandReference master, bool help
      , IList<CommandAlreadyExistsException> exceptions, params string[] idents)
      => CreateCommand((exceptions) => new CommandDelegate(action, descriptor, master, exceptions, idents), help, exceptions);
    /// <summary>
    /// Creates a new command using a delegate function. Handles all exceptions and outputs them to the console.
    /// </summary>
    /// <typeparam name="T">Command type to use.</typeparam>
    /// <param name="creator">Function that will create the command. Uses an exception list as parameter.</param>
    /// <param name="help">Create the help command under it?</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static T? CreateCommand<T>(Func<IList<CommandAlreadyExistsException>, T> creator, bool help) where T : Command
      => CreateCommand(creator, help, new List<CommandAlreadyExistsException>());
    /// <summary>
    /// Creates a new command using a delegate function. Handles all exceptions and outputs them to the console.
    /// </summary>
    /// <typeparam name="T">Command type to use.</typeparam>
    /// <param name="creator">Function that will create the command. Uses an exception list as parameter.</param>
    /// <param name="help">Create the help command under it?</param>
    /// <param name="exceptions">List of exceptions to use for outputting exceptions. Will be cleared upon execution.</param>
    /// <returns>The new command, if created successfully. Returns null if an error has occurred.</returns>
    public static T? CreateCommand<T>(Func<IList<CommandAlreadyExistsException>, T> creator, bool help, IList<CommandAlreadyExistsException> exceptions) where T : Command
    {
      exceptions.Clear();
      T com;
      try { com = creator(exceptions); }
      catch (CommandCouldNotBeCreatedException ex)
      {
        @out.WriteLine(ex.Message + '\n' + string.Join('\n', exceptions));
        return null;
      }
      catch (Exception ex)
      {
        @out.WriteLine("Unknown exception when creating command: " + ex.Message);
        return null;
      }
      if (exceptions.Count > 0)
      {
        @out.WriteLine("Exceptions when creating command " + com + ":");
        @out.WriteLine(string.Join('\n', exceptions));
      }
      if (help) PredefinedCommands.AddHelpCommand(com);
      return com;
    }

    /// <summary>
    /// Creates a link command.
    /// </summary>
    /// <param name="refc">Command to use as reference.</param>
    /// <param name="master">Reference to put the link under.</param>
    /// <param name="idents">Identifiers to use for the link command.</param>
    public static void CreateLinkCommand(Command refc, CommandReference master, params string[] idents)
      => CreateCommand((exceptions) => new CommandLink(refc, master, exceptions, idents), false, new List<CommandAlreadyExistsException>());
    /// <summary>
    /// Creates a link command.
    /// </summary>
    /// <param name="refc">Command to use as reference.</param>
    /// <param name="master">Reference to put the link under.</param>
    /// <param name="exceptions">List of exceptions to use.</param>
    /// <param name="idents">Identifiers to use for the link command.</param>
    public static void CreateLinkCommand(Command refc, CommandReference master, IList<CommandAlreadyExistsException> exceptions, params string[] idents)
      => CreateCommand((exceptions) => new CommandLink(refc, master, exceptions, idents), false, exceptions);

    /// <summary>
    /// Creates link commands for a collection of commands under a command reference.
    /// </summary>
    /// <typeparam name="T">Command object type.</typeparam>
    /// <param name="cmds">Commands to create links for.</param>
    /// <param name="master">Command reference to add the links under.</param>
    public static void CreateLinkCommands<T>(IEnumerable<T> cmds, CommandReference master) where T : Command
      => CreateLinkCommands(cmds, master, new List<CommandAlreadyExistsException>());
    /// <summary>
    /// Creates link commands for a collection of commands under a command reference.
    /// </summary>
    /// <typeparam name="T">Command object type.</typeparam>
    /// <param name="cmds">Commands to create links for.</param>
    /// <param name="master">Command reference to add the links under.</param>
    /// <param name="exceptions">List of exceptions.</param>
    public static void CreateLinkCommands<T>(IEnumerable<T> cmds, CommandReference master, IList<CommandAlreadyExistsException> exceptions) where T : Command
    {
      exceptions.Clear();
      foreach (T cmd in cmds)
      {
        if (master.ContainsKey(cmd.ID)) continue;
        CreateCommand<Command>((exceptions) => new CommandLink(cmd, master, exceptions), false, exceptions);
      }
    }

    /// <summary>
    /// Prepares the console system. Is not a mandatory method to run, the system can run by itself with the MainLoop method.
    /// This method only adds predefined commands and sets the console's window title.
    /// </summary>
    /// <param name="linkcon">Link all console commands back to root?</param>
    public static void Setup(bool linkcon = true)
      => Setup(Assembly.GetEntryAssembly().ToDynamicString(), linkcon);
    /// <summary>
    /// Prepares the console system. Is not a mandatory method to run, the system can run by itself with the MainLoop method.
    /// This method only adds predefined commands and sets the console's window title.
    /// </summary>
    /// <param name="conname">The console's name.</param>
    /// <param name="linkcon">Link all console commands back to root?</param>
    public static void Setup(string conname, bool linkcon = true)
    {
      @out.WriteLine(get_info());
      @out.WriteLine();
      @out.WriteLine("Preparing console application...");
      Console.Title = conname;
      @out.WriteLine("Adding basic commands...");
      PredefinedCommands.AddHelpCommand(Command.Root);
      Command console = PredefinedCommands.AddConsoleCommand(Command.Root);
      PredefinedCommands.AddConsoleCommands(console);
      if (linkcon) CreateLinkCommands(console.Values, Command.Root);
      @out.WriteLine("Console prepared successfully.");
      @out.WriteLine();
    }

    /// <summary>
    /// Makes the system enter the MainLoop. Will be in it while Looping is true.
    /// </summary>
    /// <param name="input">Represents when the system awaits user input.</param>
    public static async Task MainLoopAsync(string input = ":>")
    {
      Looping = true;
      string str;
      while (Looping)
      {
        @out.Write(input);
        str = @in.ReadLine();
        Inputs.Add(str);
        try { await Command.Run(str); }
        catch (Exception e) { ShowException(e); }
      }
    }

    /// <summary>
    /// Makes the system enter the MainLoop. Will be in it while Looping is true.
    /// </summary>
    /// <param name="input">Represents when the system awaits user input.</param>
    public static void MainLoop(string input = ":>") => MainLoopAsync(input).Wait();

    /// <summary>
    /// Outputs an exception to the console.
    /// </summary>
    /// <param name="e">Exception to be outputted.</param>
    public static void ShowException(Exception e)
    {
      if (e is CommandExecutionException) ShowException(e.Message + ": " + e.InnerException.Message, e);
      else if (e is CommandNotFoundException) ShowException(e.Message, e);
      else ShowException("An unknown exception has occurred while executing the command: " + e.Message, e);
    }

    #endregion

    #region private static

    private static void ShowException(string text, Exception ex)
    {
      @out.WriteLine(text + (ShowStack ? "\n" + ex.StackTrace : ""));
      Exceptions.GetCollection().Add(ex);
    }

    private static Func<CommandReference, IList<CommandAlreadyExistsException>, Command> get_help;
    private static Func<string> get_info;
    private static TextReader @in;
    private static TextWriter @out;
    private static bool modif = true;

    #endregion
  }
}
