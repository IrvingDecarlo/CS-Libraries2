using Cephei.Objects;
using System;

namespace Cephei.Commands
{
  /// <summary>
  /// CommandExecutionExceptions are thrown when an exception occurs during the command's execution.
  /// </summary>
  public class CommandExecutionException : ObjectException<Command>
  {
    internal CommandExecutionException(Command command, Exception inner) 
      : base(command, $"An exception has occurred while executing the command {command}.", inner)
    { }
  }
}
