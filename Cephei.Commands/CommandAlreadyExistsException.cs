using System.ComponentModel;

namespace Cephei.Commands
{
  /// <summary>
  /// The CommandAlreadyExistsException is thrown when it is attempted to add a command under another when said identifier already exists under the master.
  /// </summary>
  public class CommandAlreadyExistsException : WarningException, ICommandCreationException
  {
    internal CommandAlreadyExistsException(Command command, CommandReference master, string id) 
      : base($"The {master.GetString()} already has a command with identifier '{id}'.")
    {
      Object = command;
      Master = master;
    }

    #region overrides

    /// <summary>
    /// The master command that attempted to receive the faulty child.
    /// </summary>
    public CommandReference Master { get; }

    /// <summary>
    /// The Command that was attempted to be added under the master.
    /// </summary>
    public Command Object { get; }

    #endregion
  }
}
