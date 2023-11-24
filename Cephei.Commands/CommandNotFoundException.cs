using Cephei.Objects;
using System.Collections.Generic;

namespace Cephei.Commands
{
  /// <summary>
  /// CommandNotFoundExceptions are thrown when a command is not found via a command queue.
  /// </summary>
  public class CommandNotFoundException : ObjectException<Command?>
  {
    internal CommandNotFoundException(string ident, Queue<string> queue, Command? com)
      : base(com, $"The command {ident} was not found under the {com.GetString()}.")
    {
      Identifier = ident;
      Queue = queue;
    }

    /// <summary>
    /// The remainder of the command queue that was used.
    /// </summary>
    public readonly Queue<string> Queue;

    /// <summary>
    /// The identifier that was attempted to be used to find the command.
    /// </summary>
    public readonly string Identifier;
  }
}
