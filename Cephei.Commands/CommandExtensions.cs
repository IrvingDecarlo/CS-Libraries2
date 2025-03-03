using System.Collections.Generic;

namespace Cephei.Commands
{
  /// <summary>
  /// Extensions for the Command system.
  /// </summary>
  public static class CommandExtensions
  {
    /// <summary>
    /// Gets the string identifying the command.
    /// </summary>
    /// <param name="command">Command to be identified.</param>
    /// <returns>The string identifying the command.</returns>
    public static string GetString(this CommandReference command)
    { 
      string str = command.ToString();
      return string.IsNullOrEmpty(str) ? "system" : "command " + str;
    }

    /// <summary>
    /// Links all enumerated commands under a target command reference. Will avoid adding links if a command of similar ID already exists under the target.
    /// </summary>
    /// <typeparam name="T">Command object type.</typeparam>
    /// <param name="cmds">Collection of commands.</param>
    /// <param name="target">Command to put the links under.</param>
    /// <param name="excs">List of exceptions.</param>
    public static void LinkAllUnder<T>(this IEnumerable<T> cmds, CommandReference target, IList<CommandAlreadyExistsException>? excs) where T : Command
    {
      foreach (T cmd in cmds)
      {
        if (target.ContainsKey(cmd.ID)) continue;
        new CommandLink(cmd, target, excs);
      }
    }
  }
}
