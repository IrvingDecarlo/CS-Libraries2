using Cephei.Objects;

namespace Cephei.Commands
{
  /// <summary>
  /// The CommandCouldNotBeAddedException is thrown when the command ultimately fails to be created.
  /// </summary>
  public class CommandCouldNotBeCreatedException : ObjectException<Command>, ICommandCreationException
  {
    internal CommandCouldNotBeCreatedException(Command command, Command? master, params string[] idents)
      : base(command, $"The command with identifiers '{string.Join(',', idents)}' could not be added under the {master.GetString()}.")
    {
      Identifiers = idents;
      Master = master;
    }

    #region overrides

    /// <summary>
    /// The master command involved in the exception.
    /// </summary>
    public Command? Master { get; }

    #endregion

    #region public

    /// <summary>
    /// The identifiers that were attempted to be added to the command.
    /// </summary>
    public readonly string[] Identifiers;

    #endregion
  }
}
