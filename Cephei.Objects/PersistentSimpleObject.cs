namespace Cephei.Objects
{
  /// <summary>
  /// The PersistentSimpleObject offers a more basic implementation for objects that are meant to be identifiable, modifiable and deletable.
  /// </summary>
  /// <typeparam name="T">IIdentifiable type.</typeparam>
  public class PersistentSimpleObject<T> : PersistentAbstractObject<T>
  {
    /// <summary>
    /// Instantiates a persistent object.
    /// </summary>
    /// <param name="ID">The ID to assign to the object.</param>
    /// <param name="delet">Is the object to be flagged as deletable?</param>
    /// <param name="modif">Is the object to be flagged as modifiable?</param>
    public PersistentSimpleObject(T ID, bool modif = true, bool delet = true)
    {
      Id = ID;
      Modif = modif;
      Delet = delet;
    }

    #region overrides

    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    public sealed override T ID => Id;

    /// <summary>
    /// Gets the object's deletable flag.
    /// </summary>
    public sealed override bool Deletable => Delet;

    /// <summary>
    /// Gets the object's modifiable flag.
    /// </summary>
    public sealed override bool Modifiable => Modif;

    /// <summary>
    /// Should the deletable flag be bypassed?
    /// </summary>
    /// <returns>False by default.</returns>
    protected override bool BypassDeletable() => false;

    #endregion

    #region protected

    // VARIABLES

    /// <summary>
    /// The raw access to the object's ID.
    /// </summary>
    protected T Id;

    /// <summary>
    /// The raw access to the object's Modifiable flag.
    /// </summary>
    protected bool Modif;

    /// <summary>
    /// The raw access to the object's Deletable flag.
    /// </summary>
    protected bool Delet;

    #endregion
  }
}
