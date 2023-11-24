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
    /// <returns>The string identifying the command. Can be "system" if the object is null.</returns>
    public static string GetString(this Command? command)
      => command is null ? "system" : "command " + command.ToString();
  }
}
