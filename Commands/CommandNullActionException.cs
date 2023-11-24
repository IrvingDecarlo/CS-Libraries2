using Cephei.Objects;

namespace Cephei.Commands
{
  /// <summary>
  /// NullActionCommandExceptions are thrown when the command is executed but has no action assigned to it.
  /// </summary>
  public class CommandNullActionException : ObjectException<Command>
  {
    internal CommandNullActionException(Command command)
      : base(command, $"The command {command} does not have an action assigned to it.")
    { }
  }
}
