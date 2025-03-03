using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cephei.Commands
{
  /// <summary>
  /// The Pseudo Command is a command that simply mimics another command's action and description.
  /// </summary>
  public class CommandLink : Command
  {
    /// <summary>
    /// Creates a new Pseudo Command.
    /// </summary>
    /// <param name="refc">Reference command to get the action and description from.</param>
    /// <param name="master">Command to assign it under.</param>
    /// <param name="errors">List of exceptions.</param>
    /// <param name="idents">Identifiers to use. If none is set then it will copy the reference's identifiers.</param>
    public CommandLink(Command refc, CommandReference master, IList<CommandAlreadyExistsException>? errors, params string[] idents)
      : base(master, errors, idents.Length < 1 ? refc.Identifiers : idents)
      => Reference = refc;

    #region overrides

    /// <summary>
    /// Returns the link's reference to string.
    /// </summary>
    /// <returns>The link's reference to string.</returns>
    public override string ToString() => Reference.ToString();

    /// <summary>
    /// Gets the reference command's description.
    /// </summary>
    /// <returns>The reference command's description.</returns>
    public override string GetDescription() => Reference.GetDescription();

    /// <summary>
    /// Calls the reference command's action.
    /// </summary>
    /// <param name="args">Arguments to use in the execution.</param>
    protected override async Task DoExecute(IReadOnlyDictionary<string, IReadOnlyList<string>> args)
      => await Reference.ExecuteAsync(args);

    /// <summary>
    /// Clones this command, creating a new Pseudo Command with the same reference command.
    /// </summary>
    /// <param name="com">Command to assign the copy under.</param>
    /// <param name="excs">List of exceptions.</param>
    /// <param name="idents">Identifiers to use.</param>
    /// <returns>The newly created command.</returns>
    public override Command Clone(CommandReference com, IList<CommandAlreadyExistsException> excs, string[] idents)
      => new CommandLink(Reference, com, excs, idents);

    /// <summary>
    /// Seeks the specified command under the reference command.
    /// </summary>
    /// <param name="str">Command to seek.</param>
    /// <returns>The command, if found. Null otherwise.</returns>
    public override Command? Seek(string str) => Reference.Seek(str);

    #endregion

    #region public

    /// <summary>
    /// Reference command used by this command.
    /// </summary>
    public readonly Command Reference;

    #endregion
  }
}
