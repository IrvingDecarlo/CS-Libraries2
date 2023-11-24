using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// The IReadOnlyTabledCollection contains a read-only access to the object's table.
  /// </summary>
  public interface IReadOnlyTabledObject
  {
    /// <summary>
    /// Gets this object's tables.
    /// </summary>
    /// <returns>The object's tables.</returns>
    ICollection<string> GetTables();

    /// <summary>
    /// The object's current table.
    /// </summary>
    string Table { get; }
  }
}
