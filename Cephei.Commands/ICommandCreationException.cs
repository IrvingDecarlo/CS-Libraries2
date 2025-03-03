using Cephei.Objects;

namespace Cephei.Commands
{
  /// <summary>
  /// ICommandCreationExceptions envelop all throwable command creation exceptions.
  /// </summary>
  public interface ICommandCreationException : IObjectException<Command>
  {
    /// <summary>
    /// Gets the master command involved in the exception. Can be null if the master is the static system itself.
    /// </summary>
    CommandReference Master { get; }
  }
}
