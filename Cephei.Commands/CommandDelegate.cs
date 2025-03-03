using Cephei.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cephei.Commands
{
  /// <summary>
  /// CommandDelegates have their actions and descriptions defined by delegate functions.
  /// </summary>
  public class CommandDelegate : Command
  {

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="action">Action for the command to perform.</param>
    /// <param name="descriptor">Function that returns the command's description.</param>
    /// <param name="master">The master command. Can be null to assign it directly to the static system.</param>
    /// <param name="excs">List for the exceptions that may occur during creation.</param>
    /// <param name="idents">Identifiers for the command. They will always be converted to upper case invariably.</param>
    /// <exception cref="CommandCouldNotBeCreatedException"></exception>
    public CommandDelegate(Executor? action, Func<string>? descriptor, CommandReference master, IList<CommandAlreadyExistsException>? excs, params string[] idents)
      : base(master, excs, idents)
    {
      this.action = action;
      this.descriptor = descriptor;
    }

    #region overrides

    /// <summary>
    /// Gets the command's description.
    /// </summary>
    /// <returns>The command's description. Can be an empty string if it has none.</returns>
    public override string GetDescription()
      => descriptor is null ? "" : descriptor();

    /// <summary>
    /// Executes the command's action.
    /// </summary>
    /// <param name="parameters">Parameters for the command's action.</param>
    /// <exception cref="CommandNullActionException"></exception>
    protected override async Task DoExecute(IReadOnlyDictionary<string, IReadOnlyList<string>> parameters)
    {
      if (action is null) throw new CommandNullActionException(this);
      await action(parameters);
    }

    /// <summary>
    /// Clones this delegate command.
    /// </summary>
    /// <param name="com">Command to add the copy under.</param>
    /// <param name="excs">List of exceptions.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The new copied command.</returns>
    public override Command Clone(CommandReference com, IList<CommandAlreadyExistsException> excs, string[] idents)
      => new CommandDelegate(action, descriptor, com, excs, idents);

    #endregion

    #region public

    /// <summary>
    /// The Executor is the main delegate for operating the command's action.
    /// </summary>
    /// <param name="args">Arguments that will be used for the execution of the command.</param>
    public delegate Task Executor(IReadOnlyDictionary<string, IReadOnlyList<string>> args);

    /// <summary>
    /// Gets or sets the command's action.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public Executor? Action
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        action = value;
      }
      get => action;
    }

    /// <summary>
    /// Gets or sets the function that returns the command's description.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public Func<string>? Descriptor
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        descriptor = value;
      }
      get => descriptor;
    }

    #endregion

    #region private

    private Executor? action;
    private Func<string>? descriptor;

    #endregion
  }
}
