namespace Cephei.Objects
{
  /// <summary>
  /// The IReadOnlyDeletable interface denotes an object where its deletability cannot be altered.
  /// </summary>
  public interface IReadOnlyDeletable
  {
    /// <summary>
    /// Is this object deletable?
    /// </summary>
    bool Deletable { get; }

    /// <summary>
    /// The Deleted flag is to be set when the Delete() method is called. Deleted may be used to hasten the deletion proccess, since it also signals that the object
    /// has already been flagged as Deleted or if it is currently being deleted.
    /// </summary>
    bool Deleted { get; }

    /// <summary>
    /// The Delete method is used for Deleting the object.
    /// </summary>
    void Delete();
  }
}
