using System.Collections.Generic;

namespace Cephei.Data.DBDictionaries
{
  /// <summary>
  /// The IDBDictionary is a dictionary that works with a database in disk.
  /// </summary>
  /// <typeparam name="T">Key object type.</typeparam>
  /// <typeparam name="U">Value object type.</typeparam>
  public interface IDBDictionary<T, U> : IDictionary<T, U>, IReadOnlyDBDictionary<T, U>
  {

  }
}
