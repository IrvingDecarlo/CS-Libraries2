namespace Cephei.Objects.Effects
{
  /// <summary>
  /// TickableEffects are effects that can be ticked down until deletion.
  /// </summary>
  /// <typeparam name="T">The object's ID type.</typeparam>
  /// <typeparam name="U">Object type for the effect's tickability.</typeparam>
  public abstract class TickableEffect<T, U> : Effect<T>, ITickable<U>
  {
    /// <summary>
    /// Creates a new Effect.
    /// </summary>
    /// <param name="id">The object's ID. It cannot be altered afterwards.</param>
    /// <param name="ticks">Starting amount of ticks.</param>
    /// <param name="eff">Effect to assign the object under.</param>
    public TickableEffect(T id, U ticks, Effect<T>? eff = null) : base(id, eff)
      => this.ticks = ticks;

    #region overrides

    /// <summary>
    /// The effect's number of ticks.
    /// </summary>
    public U Ticks
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        SetTicks(value);
      }
      get => ticks;
    }

    /// <summary>
    /// Ticks the effect by a certain amount.
    /// </summary>
    /// <param name="amount">Amount to tick the effect by.</param>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public void Tick(U amount)
    {
      if (!Modifiable) throw new ObjectNotModifiableException(this);
      SetTicks(OnTickedBy(amount));
    }
    /// <summary>
    /// Ticks the effect.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public void Tick()
    {
      if (!Modifiable) throw new ObjectNotModifiableException(this);
      SetTicks(OnTickedBy());
    }

    #endregion

    #region protected

    /// <summary>
    /// Returns an amount determining the new number of ticks after being ticket by a certain amount.
    /// </summary>
    /// <param name="amount">Amount to tick by.</param>
    /// <returns>The new number of ticks.</returns>
    protected abstract U OnTickedBy(U amount);
    /// <summary>
    /// Returns the new amount of ticks after being ticked a single time.
    /// </summary>
    /// <returns>The new amount of ticks.</returns>
    protected abstract U OnTickedBy();

    /// <summary>
    /// Sets the effect's tick amount bypassing the modifiable flag.
    /// </summary>
    protected void SetTicks(U value)
    {
      OnTicked(value);
      ticks = value;
    }

    /// <summary>
    /// What to do when the effect is ticked.
    /// </summary>
    /// <param name="value">The new amount of ticks. The old one can be accessed via the Ticks property.</param>
    protected abstract void OnTicked(U value);

    #endregion

    #region private

    private U ticks;

    #endregion
  }
}
