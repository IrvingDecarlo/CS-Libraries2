namespace Cephei.Objects
{
  /// <summary>
  /// IDeletable implements the Delete() method, which deletes the object that implements it. It is NOT to be confused with IDisposable, since IDeletable is to be used
  /// for detaching the object from MANAGED resources.
  /// </summary>
  public interface IDeletable : IReadOnlyDeletable
  {
    /// <summary>
    /// Is the object Deletable?
    /// </summary>
    new bool Deletable { set; get; }
  }
}
