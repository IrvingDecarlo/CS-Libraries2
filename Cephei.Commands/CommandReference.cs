using Cephei.Collections;

namespace Cephei.Commands
{
  /// <summary>
  /// The CommandReference class is a base reference for all commands. All commands must be placed under a CommandReference.
  /// </summary>
  public class CommandReference : ProtectedDictionary<string, Command>
  {
    internal CommandReference() : base()
    { }

    #region overrides

    /// <summary>
    /// Returns a string identifying this Command Reference.
    /// </summary>
    /// <returns>A string identifying this Command Reference.</returns>
    public override string ToString() => "";

    #endregion

    #region public

    /// <summary>
    /// Seeks a command under this reference of the specified name.
    /// </summary>
    /// <param name="str">The command's name.</param>
    /// <returns>The command, if found. Null if no command with the specified name exists.</returns>
    public virtual Command? Seek(string str)
    {
      if (TryGetValue(str, out Command cmd)) return cmd;
      return null;
    }

    #endregion

    #region internal

    internal void Add(string str, Command cmd) => Collection.Add(str, cmd);

    #endregion
  }
}
