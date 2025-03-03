using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cephei.Commands.Consoles
{
  using static ConsoleSystem;

  /// <summary>
  /// The standard help command.
  /// </summary>
  public class CommandHelp : Command
  {
    /// <summary>
    /// Creates a new help command.
    /// </summary>
    /// <param name="master">Command to assign it under.</param>
    /// <param name="excs">List of exceptions outputted.</param>
    /// <param name="ident">Command identifiers to use.</param>
    public CommandHelp(CommandReference master, IList<CommandAlreadyExistsException>? excs, params string[] ident) :
      base(master, excs, ident)
    { }

    #region overrides

    /// <summary>
    /// Gets the command's description.
    /// </summary>
    /// <returns>The command's description.</returns>
    public override string GetDescription()
      => "Core command: Help. Does not have any arguments.";

    /// <summary>
    /// Identifies the master's description and subcommands. Outputs it all to console.
    /// Will identify all of the system's commands if it is not assigned to a master command.
    /// </summary>
    /// <param name="args">The command's arguments.</param>
    protected override Task DoExecute(IReadOnlyDictionary<string, IReadOnlyList<string>> args)
    {
      TextWriter writer = Out;
      writer.WriteLine();
      if (Master is Command cmd)
      {
        writer.WriteLine(cmd.GetDescription());
        writer.WriteLine();
        writer.WriteLine("Commands under " + Master + ":");
      }
      else writer.WriteLine("Commands in the system:");
      writer.WriteLine(string.Join('\n', Master));
      writer.WriteLine();
      return Task.CompletedTask;
    }

    /// <summary>
    /// Clones this help command.
    /// </summary>
    /// <param name="com">Command to add the copy under.</param>
    /// <param name="excs">List of exceptions.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new copied command.</returns>
    public override Command Clone(CommandReference com, IList<CommandAlreadyExistsException> excs, params string[] idents)
      => new CommandHelp(com, excs, idents);

    #endregion
  }
}
