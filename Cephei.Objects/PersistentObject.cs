namespace Cephei.Objects
{
  /// <summary>
  /// Persistent Objects combine both IIdentifiable and IModifiable interfaces, being a base for any object with an ID. The object's type must be comparable.
  /// </summary>
  /// <typeparam name="T">The type to use for the object's ID.</typeparam>
  public abstract class PersistentObject<T> : PersistentReadonlyObject<T>, IIdentifiable<T>, IModifiable, IDeletable
  {
    /// <summary>
    /// Instantiates a persistent object.
    /// </summary>
    /// <param name="ID">The ID to assign to the object.</param>
    /// <param name="delet">Is the object to be flagged as deletable?</param>
    /// <param name="modif">Is the object to be flagged as modifiable?</param>
    /// <param name="lockid">Lock the object's ID?</param>
    /// <param name="lockdel">Lock the object's deletable flag?</param>
    public PersistentObject(T ID, bool modif = true, bool delet = true, bool lockid = false, bool lockdel = false) : base(ID, modif, delet)
    {
      LockID = lockid;
      LockDeletable = lockdel;
    }

    #region overrides

    /// <summary>
    /// Gets or sets the object's ID.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public new T ID
    {
      set
      {
        if (!Modif) throw new ObjectNotModifiableException(this);
        else if (LockID) throw new ObjectNotModifiableException(this, "The object '" + ToString() + "''s ID is locked.");
        else Id = value;
      }
      get => Id;
    }

    /// <summary>
    /// This is the object's modifiable flag. The flag may be set to false, but never from false to true.
    /// It also blocks the modification of other data, such as ID and Deletable.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public new bool Modifiable
    {
      set
      {
        if (!Modif) throw new ObjectNotModifiableException(this);
        else Modif = value;
      }
      get => Modif;
    }

    /// <summary>
    /// Is the object Deletable? The value can only be changed if Modifiable is true.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public new bool Deletable
    {
      set
      {
        if (!Modif) throw new ObjectNotModifiableException(this);
        else if (LockDeletable) throw new ObjectNotModifiableException(this, "The object '" + ToString() + "''s Deletable flag is locked.");
        else Delet = value;
      }
      get => Delet;
    }

    #endregion

    #region protected

    /// <summary>
    /// Is this object's ID locked despite of the Modifiable flag?
    /// </summary>
    protected bool LockID = false;

    /// <summary>
    /// Is this object's Deletable flag locked despite of the Modifiable flag?
    /// </summary>
    protected bool LockDeletable = false;

    #endregion
  }
}
