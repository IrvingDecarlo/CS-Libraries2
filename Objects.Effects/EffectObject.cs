using System;

namespace Cephei.Objects.Effects
{
  /// <summary>
  /// Effect Objects are effects that can be placed under effects.
  /// </summary>
  /// <typeparam name="T">Object type to be used as the object's ID.</typeparam>
  public abstract class EffectObject<T> : IReadOnlyIdentifiable<T>, IModifiable, IDeletable
  {
    /// <summary>
    /// Creates a new EffectObject.
    /// </summary>
    /// <param name="id">The object's ID. It cannot be altered afterwards.</param>
    /// <param name="eff">Effect to assign the object under.</param>
    public EffectObject(T id, Effect<T>? eff = null)
    {
      Effect = eff;
      ID = id;
    }

    #region overrides

    /// <summary>
    /// Reference to the effect's ID.
    /// </summary>
    public T ID { get; }

    /// <summary>
    /// Checks if this EffectObject is equal to another identifiable object of the same hash code.
    /// </summary>
    /// <param name="ident">Identifiable object to equate to.</param>
    /// <returns>True if both are EffectObjects with the same hash code..</returns>
    public bool Equals(IReadOnlyIdentifiable<T> ident) => ident is EffectObject<T> && GetHashCode().Equals(ident?.GetHashCode());
    /// <summary>
    /// Checks if this EffectObject is equal to another object.
    /// </summary>
    /// <param name="obj">Object to equate to.</param>
    /// <returns>True if both are EffectObjects with the same HashCode.</returns>
    public sealed override bool Equals(object obj) => obj is EffectObject<T> efobj && GetHashCode().Equals(efobj?.GetHashCode());

    /// <summary>
    /// Gets the ID's hash code.
    /// </summary>
    /// <returns>The ID's hash code (0 if null).</returns>
    public override int GetHashCode() => ID is null ? 0 : ID.GetHashCode();

    /// <summary>
    /// Gets or sets this effect object's modifiability.
    /// </summary>
    public abstract bool Modifiable { get; set; }

    /// <summary>
    /// Gets or sets this effect object's deletability.
    /// </summary>
    public abstract bool Deletable { get; set; }

    /// <summary>
    /// Was this object deleted already?
    /// </summary>
    public bool Deleted { get; private set; }

    /// <summary>
    /// Deletes the Effect Object, detaching it from its parent effect.
    /// </summary>
    /// <exception cref="ObjectDeletedException"></exception>
    /// <exception cref="ObjectNotDeletableException"></exception>
    public void Delete()
    {
      if (Deleted) throw new ObjectDeletedException(this);
      if (!Deletable && !(effect is null) && !effect.Deleted && !BypassDeletable()) throw new ObjectNotDeletableException(this);
      DoDelete();
    }

    #endregion

    #region public

    /// <summary>
    /// Reference to this object's effect. Throws ObjectNotModifiableException if one of the effects or if this object aren't modifiable.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    /// <exception cref="ObjectDeletedException"></exception>
    public Effect<T>? Effect
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        ValidateEffect(value);
        ValidateEffect(effect);
        SetEffect(value);
      }
      get => effect;
    }

    #endregion

    #region protected

    // EVENTS

    /// <summary>
    /// Additional actions to take when the effect is deleted.
    /// </summary>
    protected event Action? OnDeleted;

    // METHODS

    /// <summary>
    /// If BypassDeletable returns true then the object's Deletable flag will be ignored when it is deleted.
    /// </summary>
    /// <returns>True if the object's deletable flag is to be bypassed.</returns>
    protected abstract bool BypassDeletable();

    /// <summary>
    /// Deletes the object, bypassing its deletable flag.
    /// </summary>
    /// <exception cref="ObjectDeletionException"></exception>
    protected void DoDelete()
    {
      Deleted = true;
      try
      {
        OnDeleted?.Invoke();
        if (!(effect is null))
        { 
          if (!effect.Deleted) effect?.Remove(this);
          effect = null;
        }
      }
      catch (Exception e)
      {
        Deleted = false;
        throw new ObjectDeletionException(this, e);
      }
    }

    /// <summary>
    /// Sets the object's effect, bypassing modifiable flags.
    /// </summary>
    /// <param name="eff">Effect to set it to.</param>
    /// <exception cref="ObjectDeletedException"></exception>
    protected void SetEffect(Effect<T>? eff)
    {
      if (!(eff is null) && eff.Deleted) throw new ObjectDeletedException(eff);
      effect?.Remove(this);
      eff?.Add(this);
      effect = eff;
    }

    #endregion

    #region private

    private void ValidateEffect(Effect<T>? effect)
    {
      if (!(effect is null) && !effect.Modifiable) throw new ObjectNotModifiableException(effect);
    }

    Effect<T>? effect;

    #endregion
  }
}
