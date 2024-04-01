namespace Cephei.Objects
{
  /// <summary>
  /// The PersistentReadonlyObject offers a readonly implementation for objects that are meant to be identifiable, modifiable and deletable.
  /// </summary>
  /// <typeparam name="T">IIdentifiable type.</typeparam>
  /// <remarks>The Modifiable and Deletable components still are abstract.</remarks>
  public abstract class PersistentReadonlyObject<T> : PersistentAbstractObject<T>
  {
    /// <summary>
    /// Instantiates a persistent object.
    /// </summary>
    /// <param name="id">The object's ID.</param>
    public PersistentReadonlyObject(T id) => ID = id;

    #region overrides

    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    public sealed override T ID { get; }

    #endregion
  }
}
