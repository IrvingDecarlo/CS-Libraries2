using System;

namespace Cephei.Objects.Managed
{
  /// <summary>
  /// Managed objects are objects that are directly managed by the static system. They have unique HashCodes and are never collected by the GC unless they're deleted.
  /// </summary>
  /// <typeparam name="T">Object type to use as the object's ID.</typeparam>
  public abstract class ManagedObject<T> : PersistentObject<T>, IEquatable<ManagedObject<T>>
  {
    /// <summary>
    /// Creates a new ManagedObject.
    /// </summary>
    /// <param name="id">The ID to assign to the object.</param>
    /// <param name="delet">Is the object to be flagged as deletable?</param>
    /// <param name="modif">Is the object to be flagged as modifiable?</param>
    /// <param name="lockid">Lock the object's ID?</param>
    /// <param name="lockdel">Lock the object's deletable flag?</param>
    public ManagedObject(T id, bool modif = true, bool delet = true, bool lockid = false, bool lockdel = false) : base(id, modif, delet, lockid, lockdel)
    {
      ManagedObjectCollection col = GetManagedCollection();
      hash_code = col.Position;
      col.Add(this);
    }

    #region overrides

    /// <summary>
    /// Checks if this ManagedObject is equal to another object.
    /// </summary>
    /// <param name="obj">Object to equate to.</param>
    /// <returns>True if both are Managed Objects and with the same hash codes.</returns>
    public sealed override bool Equals(object obj)
    {
      if (obj is ManagedObject<T> mobj) return Equals(mobj);
      return false;
    }
    /// <summary>
    /// Checks if this Managed Object is equal to another IIdentifiable object.
    /// </summary>
    /// <param name="ident">Identifiable to equate to.</param>
    /// <returns>True if both are Managed Objects and with same hash codes.</returns>
    public sealed override bool Equals(IReadOnlyIdentifiable<T> ident)
    {
      if (ident is ManagedObject<T> mobj) return Equals(mobj);
      return false;
    }
    /// <summary>
    /// Checks if both ManagedObjects have the same hash code.
    /// </summary>
    /// <param name="mobj">Managed Object to equate to.</param>
    /// <returns>True if both have the same hash code.</returns>
    public bool Equals(ManagedObject<T> mobj) => GetHashCode().Equals(mobj.GetHashCode());

    /// <summary>
    /// Returns the Managed Object's unique hash code.
    /// </summary>
    /// <returns>The object's hash code.</returns>
    public sealed override int GetHashCode() => hash_code;

    /// <summary>
    /// BypassDeletable returns true if the managed collection does not contain this object or if BypassDeletion returns true.
    /// </summary>
    /// <returns>True if the managed collection does not contain this object or if BypassDeletion returns true.</returns>
    protected sealed override bool BypassDeletable()
      => !GetManagedCollection().Contains(this) || BypassDeletion();

    /// <summary>
    /// OnDeleted removes the object from the managed collection and calls OnDeletion.
    /// </summary>
    protected sealed override void OnDeleted()
    {
      OnDeletion();
      GetManagedCollection().Remove(this);
    }

    #endregion

    #region protected

    /// <summary>
    /// BypassDeletion is used to bypass the deletable flag if it returns true.
    /// </summary>
    /// <returns>True if the deletable flag is to be bypassed.</returns>
    protected abstract bool BypassDeletion();

    /// <summary>
    /// OnDeletion is called when the managed object is deleted.
    /// </summary>
    protected abstract void OnDeletion();

    #endregion

    #region private

    private readonly int hash_code;

    #endregion

    #region public static

    /// <summary>
    /// Gets the collection of basic objects managed by the system. It will instantiate the collection if none exists.
    /// </summary>
    /// <returns>The collection of basic objects managed by the system.</returns>
    public static ManagedCollection GetCollection()
    {
      Collection ??= new ManagedCollection();
      return Collection;
    }

    /// <summary>
    /// Gets the collection of ManagedObjects managed by the system. It will instantiate the collection if none exists.
    /// </summary>
    /// <returns>The collection of ManagedObjects managed by the system.</returns>
    public static ManagedObjectCollection GetManagedCollection()
    {
      ManagedCollection ??= new ManagedObjectCollection();
      return ManagedCollection;
    }

    /// <summary>
    /// The collection of basic objects managed by the system.
    /// </summary>
    public static ManagedCollection? Collection { get; private set; } = null;

    /// <summary>
    /// The collection of ManagedObjects managed by the system.
    /// </summary>
    public static ManagedObjectCollection? ManagedCollection { get; private set; } = null;

    #endregion
  }
}
