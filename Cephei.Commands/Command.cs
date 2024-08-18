using Cephei.Collections;
using Cephei.Objects;
using Cephei.Strings;
using Cephei.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cephei.Commands
{
  /// <summary>
  /// The command class is the basic class for the Command system. Through it, commands are executed and logged.
  /// </summary>
  public abstract class Command : ProtectedDictionary<string, Command>, IModifiable, IReadOnlyIdentifiable<string>, IEquatable<Command>, IDescribable<string>
    , IExecutableAsync<IReadOnlyDictionary<string, IReadOnlyList<string>>>, ICloneable<Command?, IList<CommandAlreadyExistsException>, Command>
    , ICloneable<Command?, IList<CommandAlreadyExistsException>, string[], Command>
  {
    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="master">The master command. Can be null to assign it directly to the static system.</param>
    /// <param name="excs">List for the exceptions that may occur during creation.</param>
    /// <param name="idents">Identifiers for the command. They will always be converted to upper case invariably.</param>
    /// <exception cref="CommandCouldNotBeCreatedException"></exception>
    public Command(Command? master, IList<CommandAlreadyExistsException>? excs, params string[] idents)
      : base()
    {
      IDictionary<string, Command> dict = master is null ? Commands.GetCollection() : master.Collection;
      List<string> ids = new List<string>();
      string id;
      for (int i = 0; i < idents.Length; i++)
      {
        id = idents[i].ToUpperInvariant();
        if (dict.ContainsKey(id)) 
        { 
          excs?.Add(new CommandAlreadyExistsException(this, master, id));
          continue;
        }
        dict.Add(id, this);
        ids.Add(id);
      }
      if (ids.Count < 1) throw new CommandCouldNotBeCreatedException(this, master, idents);
      Master = master;
      Identifiers = ids.ToArray();
    }

    #region overrides

    /// <summary>
    /// Is this command modifiable?
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public bool Modifiable
    {
      set
      {
        if (!modif) throw new ObjectNotModifiableException(this);
        modif = value;
      }
      get => modif;
    }

    /// <summary>
    /// Returns the command's primary identifier.
    /// </summary>
    public string ID => Identifiers[0];

    /// <summary>
    /// Checks if this command is equal to another object.
    /// </summary>
    /// <param name="obj">Object to be equated to.</param>
    /// <returns>True if both are commands and equals.</returns>
    public override bool Equals(object obj)
      => obj is Command command && Equals(command);
    /// <summary>
    /// Checks if this command is equal to another IIdentifiable object.
    /// </summary>
    /// <param name="other">The other IIdentifiable to be equated to.</param>
    /// <returns>True if both are commands and equals.</returns>
    public bool Equals(IReadOnlyIdentifiable<string> other)
      => other is Command command && Equals(command);
    /// <summary>
    /// Checks if this command is equal to another, verifying their masters as well.
    /// </summary>
    /// <param name="command">Command to be equated to.</param>
    /// <returns>True if this command is equal to the other.</returns>
    public bool Equals(Command command)
    {
      if (!ID.Equals(command.ID)) return false;
      return Master.SafeEquals(command.Master);
    }

    /// <summary>
    /// Gets the command's description.
    /// </summary>
    /// <returns>The command's description.</returns>
    public abstract string GetDescription();

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="args">Arguments to supply the command's execution.</param>
    /// <exception cref="CommandExecutionException"></exception>
    public async Task ExecuteAsync(IReadOnlyDictionary<string, IReadOnlyList<string>> args)
    {
      try { await DoExecute(args); }
      catch (Exception ex) { throw new CommandExecutionException(this, ex); }
    }

    /// <summary>
    /// Returns a string identifying this command and its masters.
    /// </summary>
    /// <returns>A string identifying this command and its masters.</returns>
    public override string ToString()
      => (Master is null ? "" : Master.ToString() + ".") + ID;

    /// <summary>
    /// Gets the command's hash code, taking its master command into consideration.
    /// </summary>
    /// <returns>The command's hash code.</returns>
    public override int GetHashCode()
      => ID.GetHashCode() + (Master is null ? 0 : Master.GetHashCode());

    /// <summary>
    /// Returns a copy of this command using its own identifiers.
    /// </summary>
    /// <param name="com">Command to add the copied command under.</param>
    /// <param name="excs">List of exceptions to output errors to.</param>
    /// <returns>This command's copy.</returns>
    public Command Clone(Command? com, IList<CommandAlreadyExistsException> excs) => Clone(com, excs, Identifiers);
    /// <summary>
    /// Returns a copy of this command.
    /// </summary>
    /// <param name="com">Command to add the copied command under.</param>
    /// <param name="excs">List of exceptions to output errors to.</param>
    /// <param name="idents">Identifiers to use for the new command.</param>
    /// <returns>The copied command.</returns>
    public abstract Command Clone(Command? com, IList<CommandAlreadyExistsException> excs, string[] idents);

    #endregion

    #region public

    /// <summary>
    /// Clones the command itself and all its subcommands.
    /// </summary>
    /// <param name="master">Master command to assign the copied command to.</param>
    /// <param name="excs">List of exceptions.</param>
    /// <param name="idents">Identifiers to use for the main copied command. Will use this command's identifiers if none is set.</param>
    /// <returns>The main copied command.</returns>
    public Command FullClone(Command? master, IList<CommandAlreadyExistsException> excs, params string[] idents)
    {
      Command comd = Clone(master, excs, idents.Length < 1 ? Identifiers : idents);
      Queue<Command> queue = new Queue<Command>();
      queue.Enqueue(comd);
      Command com;
      while (queue.Count > 0)
      {
        com = queue.Dequeue();
        foreach (Command c in com.Values) queue.Enqueue(c.Clone(com, excs));
      }
      return comd;
    }

    /// <summary>
    /// Is the array of identifiers for this command.
    /// </summary>
    public readonly string[] Identifiers;

    /// <summary>
    /// Returns the command's master. Can be null if the command is directly assigned to the static system.
    /// </summary>
    public readonly Command? Master;

    #endregion

    #region protected

    /// <summary>
    /// The execution of the command itself.
    /// </summary>
    /// <param name="args">Arguments that were supplied for the execution.</param>
    protected abstract Task DoExecute(IReadOnlyDictionary<string, IReadOnlyList<string>> args);

    #endregion

    #region private

    private bool modif;

    #endregion

    static Command() => Commands = new CommandCollection();

    #region static public

    /// <summary>
    /// Reads a string input and outputs a Dictionary with arguments and the command queue.
    /// </summary>
    /// <param name="input">Input string to be interpreted.</param>
    /// <param name="cmds">The generated command queue.</param>
    /// <returns>The command's arguments.</returns>
    public static IReadOnlyDictionary<string, IReadOnlyList<string>> Read(string input, out Queue<string> cmds)
    {
      using StringReader sr = new StringReader(input);
      return Read(sr, out cmds);
    }
    /// <summary>
    /// Reads the content of a text reader and outputs a Dictionary with arguments and the command queue.
    /// </summary>
    /// <param name="reader">Input reader to be interpreted.</param>
    /// <param name="cmds">The generated command queue.</param>
    /// <returns>The command's arguments.</returns>
    public static IReadOnlyDictionary<string, IReadOnlyList<string>> Read(TextReader reader, out Queue<string> cmds)
    {
      cmds = new Queue<string>();
      Dictionary<string, IReadOnlyList<string>> dict = new Dictionary<string, IReadOnlyList<string>>();
      StringBuilder sb = new StringBuilder();
      bool prot = false;
      bool arg = false;
      bool argval = false;
      List<string>? current = null;
      int read = reader.Read();
      while (read >= 0)
      {
        switch (read)
        {
          case 32:
          case 9:
            if (prot)
            {
              sb.Append((char)read);
              break;
            }
            CloseField(ref arg, ref argval, sb, dict, ref current, cmds);
            break;
          case 34:
            if (prot) CloseField(ref arg, ref argval, sb, dict, ref current, cmds);
            prot = !prot;
            break;
          case 45:
            if (prot)
            {
              sb.Append((char)read);
              break;
            }
            arg = true;
            argval = false;
            break;
          default: sb.TryAppend((char)read); break;
        }
        read = reader.Read();
      }
      CloseField(ref arg, ref argval, sb, dict, ref current, cmds);
      return dict;
    }

    /// <summary>
    /// Finds a command using a command queue.
    /// </summary>
    /// <param name="cmds">Queue to be used to locate the command.</param>
    /// <returns>The command defined by the command queue.</returns>
    /// <exception cref="CommandNotFoundException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static Command Find(Queue<string> cmds)
    {
      IReadOnlyDictionary<string, Command> dict = Commands;
      Command? master = null;
      Command? com = null;
      string ident;
      int c = cmds.Count;
      for (int i = 0; i < c; i++) 
      {
        ident = cmds.Dequeue().ToUpperInvariant();
        if (!dict.TryGetValue(ident, out com))
          throw new CommandNotFoundException(ident, cmds, master);
        master = com;
        dict = master;
      }
      return com ?? throw new ArgumentNullException();
    }

    /// <summary>
    /// Finds a command and executes using arguments defined by an input string. The input is trimmed and, if it is empty, then nothing will be done.
    /// </summary>
    /// <param name="input">String used to locate and parametrize the command.</param>
    /// <exception cref="CommandNotFoundException"></exception>
    /// <exception cref="CommandExecutionException"></exception>
    public static async Task Run(string input)
    {
      input = input.Trim();
      if (input.Length < 1) return;
      IReadOnlyDictionary<string, IReadOnlyList<string>> args = Read(input, out Queue<string> cmds);
      await Run(cmds, args);
    }
    /// <summary>
    /// Finds a command via a command queue and executes it using input arguments.
    /// </summary>
    /// <param name="cmds">Command queue to locate the command.</param>
    /// <param name="args">Arguments to parametrize the command's execution.</param>
    /// <exception cref="CommandNotFoundException"></exception>
    /// <exception cref="CommandExecutionException"></exception>
    public static async Task Run(Queue<string> cmds, IReadOnlyDictionary<string, IReadOnlyList<string>> args)
      => await Find(cmds).ExecuteAsync(args);

    /// <summary>
    /// Is the static command dictionary, referring to the commands that are not assigned under any other commands.
    /// </summary>
    public static readonly CommandCollection Commands;

    #endregion

    #region private static

    private static void CloseField(ref bool arg, ref bool argval, StringBuilder sb, Dictionary<string, IReadOnlyList<string>> dict, ref List<string>? current, Queue<string> queue)
    {
      if (sb.Length < 1) return;
      if (arg)
      {
        if (argval) current?.Add(sb.ToString());
        else
        {
          argval = true;
          current = new List<string>();
          dict.Add(sb.ToString(), current);
        }
      }
      else queue.Enqueue(sb.ToString());
      sb.Clear();
    }

    #endregion
  }
}
