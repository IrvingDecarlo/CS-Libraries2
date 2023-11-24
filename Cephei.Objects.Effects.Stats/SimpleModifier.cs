namespace Cephei.Objects.Effects.Stats
{
  /// <summary>
  /// SimpleModifiers are modifiers that only target a Stat, they have no source valuable that manipulates its value, therefore it can only be altered manually.
  /// </summary>
  /// <typeparam name="T">The identifiable type.</typeparam>
  /// <typeparam name="U">The valuable type.</typeparam>
  public abstract class SimpleModifier<T, U> : EffectObject<T>, IReferencedModifier<T, U>
  {
    /// <summary>
    /// Creates a new simple modifier.
    /// </summary>
    /// <param name="id">The modifier's ID.</param>
    /// <param name="value">The modifier's value.</param>
    /// <param name="eff">Effect to assign it under.</param>
    public SimpleModifier(T id, U value, Effect<T>? eff = null) : base(id, eff)
    {
      this.value = value;
      OnDeleted += OnDelete;
    }

    #region overrides

    /// <summary>
    /// Returns a string defining this modifier (its ID, value and target).
    /// </summary>
    /// <returns>A string defining the modifier.</returns>
    public override string ToString()
      => $"Modifier ID='{ID}' Value='{value}' Target='{Target}'";

    /// <summary>
    /// Bypasses the deletable flag if the target stat has been deleted.
    /// </summary>
    /// <returns>True if the deletable flag is to be bypassed.</returns>
    protected override bool BypassDeletable() => Target.IsDeletedAndNotNull();

    /// <summary>
    /// Gets or sets the modifier's value. Signals the target stat to update.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public U Value
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        SetValue(value);
      }
      get => value;
    }

    /// <summary>
    /// Reference to the modifier's target.
    /// </summary>
    public Stat<T, U>? Target { get; private set; }
    Stat<T, U>? IReferencedModifier<T, U>.Target
    {
      set => Target = value;
      get => Target;
    }

    void IReferencedModifier<T, U>.CallDelete() => DoDelete();

    /// <summary>
    /// Is this modifier up to date? Simple modifiers are always up to date by default.
    /// </summary>
    public virtual bool Updated => true;

    /// <summary>
    /// Updates the modifier. Simple modifiers by default don't do anything when updated.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Signals its target stat to be updated.
    /// </summary>
    public void SignalUpdate() => Target?.SignalUpdate();

    #endregion

    #region protected

    /// <summary>
    /// Sets the modifier's value bypassing its modifiable flag.
    /// </summary>
    /// <param name="value">The value to set it to.</param>
    protected void SetValue(U value)
    {
      this.value = value;
      Target?.SignalUpdate();
    }

    #endregion

    #region private

    // VARIABLES

    private U value;

    // METHODS

    private void OnDelete()
    {
      if (!(Target is null) && !Target.Deleted)
      {
        Target.Sources.Remove(this);
        Target.SignalUpdate();
      }
    }

    #endregion
  }
}
