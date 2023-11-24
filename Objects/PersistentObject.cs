using Cephei.Tools;
using System;

namespace Cephei.Objects
{
  /// <summary>
  /// Persistent Objects combine both IIdentifiable and IModifiable interfaces, being a base for any object with an ID. The object's type must be comparable.
  /// </summary>
  /// <typeparam name="T">The type to use for the object's ID.</typeparam>
  public abstract class PersistentObject<T> : IIdentifiable<T>, IModifiable, IDeletable
  {
    /// <summary>
    /// Instantiates a persistent object.
    /// </summary>
    /// <param name="ID">The ID to assign to the object.</param>
    /// <param name="delet">Is the object to be flagged as deletable?</param>
    /// <param name="modif">Is the object to be flagged as modifiable?</param>
    /// <param name="lockid">Lock the object's ID?</param>
    /// <param name="lockdel">Lock the object's deletable flag?</param>
    public PersistentObject(T ID, bool modif = true, bool delet = true, bool lockid = false, bool lockdel = false)
    {
      Id = ID;
      Modif = modif;
      Delet = delet;
      LockID = lockid;
      LockDeletable = lockdel;
    }

    #region overrides

    /// <summary>
    /// The object's ID.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public T ID
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
    public bool Modifiable
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
    public bool Deletable
    {
      set
      {
        if (!Modif) throw new ObjectNotModifiableException(this);
        else if (LockDeletable) throw new ObjectNotModifiableException(this, "The object '" + ToString() + "''s Deletable flag is locked.");
        else Delet = value;
      }
      get => Delet;
    }

    /// <summary>
    /// Has this object already been Deleted?
    /// </summary>
    public bool Deleted { private set; get; } = false;

    /// <summary>
    /// Deletes the object. If it is not Deletable and DeletableBypass failed then throws a ObjectNotDeletable exception.
    /// If has already been deleted, then throws the ObjectDeleted exception.
    /// Else, calls its OnDeleted() method.
    /// If an exception is thrown during the execution of OnDeleted() then the Deleted flag will be reset. The exception is NOT handled and rethrown
    /// as ObjectDeletionException.
    /// Sets the Deleted flag to true prior to calling OnDeleted.
    /// </summary>
    /// <exception cref="ObjectNotDeletableException"></exception>
    /// <exception cref="ObjectDeletedException"></exception>
    /// <exception cref="ObjectDeletionException"></exception>
    public void Delete()
    {
      if (!Delet && !BypassDeletable()) throw new ObjectNotDeletableException(this);
      DoDelete();
    }

    /// <summary>
    /// Returns a string identifying the object by its type, ID and hash code.
    /// </summary>
    /// <returns>The string identifying the object.</returns>
    public override string ToString() 
      => "Type='" + GetType().ToString() + "' ID='" + ID?.ToString() + "' Hash='" + GetHashCode().ToString() + "'";

    /// <summary>
    /// Returns the object ID's HashCode.
    /// </summary>
    /// <returns>The object's ID. 0 if the ID is null.</returns>
    public override int GetHashCode() => ID is null ? 0 : ID.GetHashCode();

    /// <summary>
    /// Equates this object to any other. If the target is an IIdentifiable then equates both as IIdentifiables, else, returns false by default.
    /// </summary>
    /// <param name="obj">The target object to be equated with.</param>
    /// <returns>True if equals, false otherwise.</returns>
    public override bool Equals(object obj)
    {
      if (obj is IReadOnlyIdentifiable<T> identifiable) return Equals(identifiable);
      else return false;
    }
    /// <summary>
    /// Equates the object to another identifiable. Returns TRUE if both have the same ID.
    /// </summary>
    /// <param name="ident">The IIdentifiable to be equated with.</param>
    /// <returns>True if both are equal, false otherwise.</returns>
    public virtual bool Equals(IReadOnlyIdentifiable<T> ident) => ident.ID.SafeEquals(ID);

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

    /// <summary>
    /// Is this object's ID locked despite of the Modifiable flag?
    /// </summary>
    protected bool LockID = false;

    /// <summary>
    /// Is this object's Deletable flag locked despite of the Modifiable flag?
    /// </summary>
    protected bool LockDeletable = false;

    // METHODS

    /// <summary>
    /// BypassDeletable is to return true if the Deletable flag is to be ignored.
    /// </summary>
    /// <returns>True if the Deletable flag is to be ignored.</returns>
    protected abstract bool BypassDeletable();

    /// <summary>
    /// OnDeleted is called when the Delete() method is executed. It is highly recommended NOT to call this method independently, since it does not set the
    /// Deleted flag. Use DoDelete instead.
    /// </summary>
    protected abstract void OnDeleted();

    /// <summary>
    /// Calls the object's Delete() method while bypassing its Deletable flag.
    /// </summary>
    /// <exception cref="ObjectDeletedException"></exception>
    /// <exception cref="ObjectDeletionException"></exception>
    protected void DoDelete()
    {
      Deleted = true;
      try { OnDeleted(); }
      catch (Exception e)
      {
        Deleted = false;
        throw new ObjectDeletionException(this, e);
      }
    }

    #endregion
  }
}
