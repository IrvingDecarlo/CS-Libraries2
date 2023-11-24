using System.Collections.Generic;

namespace Cephei.Collections
{
  /// <summary>
  /// A ISimpleDictionary is a dictionary where the item changes are dictated by the value object type only.
  /// </summary>
  /// <typeparam name="T">Key object type.</typeparam>
  /// <typeparam name="U">Value object type.</typeparam>
  public interface ISimpleDictionary<T, U> : ISimpleReadonlyDictionary<T, U>, ICollection<U>
  {
  }
}
