using System;

namespace Cephei.Objects
{
  /// <summary>
  /// The PersistentAbstractObject offers an abstract implementation for objects that are meant to be identifiable, modifiable and deletable.
  /// </summary>
  /// <typeparam name="T">IIdentifiable type.</typeparam>
  public abstract class PersistentAbstractObject<T> : ReadOnlyIdentifiableAbstract<T>, IReadOnlyModifiable, IReadOnlyDeletable
  {
    #region overrides

    /// <summary>
    /// Returns a string identifying the object by its type, ID and hash code.
    /// </summary>
    /// <returns>The string identifying the object.</returns>
    public override string ToString()
      => "Type='" + GetType().ToString() + "' ID='" + ID?.ToString() + "' Hash='" + GetHashCode().ToString() + "'";

    /// <summary>
    /// Is this object modifiable?
    /// </summary>
    public abstract bool Modifiable { get; }

    /// <summary>
    /// Has this object been deleted?
    /// </summary>
    public abstract bool Deletable { get; }

    /// <summary>
    /// Has this object already been deleted?
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
      if (!Deletable && !BypassDeletable()) throw new ObjectNotDeletableException(this);
      DoDelete();
    }

    #endregion

    #region protected

    // EVENTS

    /// <summary>
    /// OnDeleted is called when the object is deleted.
    /// </summary>
    protected event Action? OnDeleted;

    // METHODS

    /// <summary>
    /// BypassDeletable is to return true if the Deletable flag is to be ignored.
    /// </summary>
    /// <returns>True if the Deletable flag is to be ignored.</returns>
    protected abstract bool BypassDeletable();

    /// <summary>
    /// Calls the object's Delete() method while bypassing its Deletable flag.
    /// </summary>
    /// <exception cref="ObjectDeletedException"></exception>
    /// <exception cref="ObjectDeletionException"></exception>
    protected void DoDelete()
    {
      Deleted = true;
      try { OnDeleted?.Invoke(); }
      catch (Exception e)
      {
        Deleted = false;
        throw new ObjectDeletionException(this, e);
      }
    }

    #endregion
  }
}
