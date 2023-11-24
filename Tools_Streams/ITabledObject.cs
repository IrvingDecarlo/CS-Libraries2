namespace Cephei.Collections
{
  /// <summary>
  /// ITabledCollections are collections with a table reference.
  /// </summary>
  public interface ITabledObject : IReadOnlyTabledObject
  {
    /// <summary>
    /// Reference to the collection's table.
    /// </summary>
    new string Table { get; set; }
  }
}
