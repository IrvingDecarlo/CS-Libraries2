using Cephei.Collections;
using System.Collections.Generic;

namespace Cephei.Commands
{
  /// <summary>
  /// The CommandCollection class is used by the static command system to organize its underlying commands.
  /// </summary>
  public sealed class CommandCollection : ProtectedDictionary<string, Command>
  {
    internal CommandCollection() : base()
    { }

    internal IDictionary<string, Command> GetCollection() => Collection;
  }
}
