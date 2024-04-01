using Cephei.Valuables;

namespace Cephei.Objects.Stats
{
  /// <summary>
  /// Modifiers are objects that link to a target Stat, having its value manipulated by either any valuable object or by another stat.
  /// </summary>
  /// <typeparam name="T">The modifier's identifiable type.</typeparam>
  /// <typeparam name="U">The modifier's valuable type.</typeparam>
  public abstract class Modifier<T, U> : PersistentReadonlyObject<T>, IReferencedModifier<T, U>
  {
    /// <summary>
    /// Creates a new Modifier object.
    /// </summary>
    /// <param name="id">The modifier's ID.</param>
    /// <param name="value">The modifier's initial value.</param>
    public Modifier(T id, U value) : base(id)
    {
      Value = value;
      OnDeleted += OnDelete;
    }

    #region overrides

    /// <summary>
    /// Returns a string defining this modifier (its ID, value, source and target).
    /// </summary>
    /// <returns>A string defining this modifier.</returns>
    public override string ToString()
      => $"Modifier ID='{ID}' Value='{Value}' Source='{Source}' Target='{Target}' Updated: {Updated}";

    /// <summary>
    /// BypassDeletion returns true if either the source or the target are not null and have been deleted.
    /// </summary>
    /// <returns>True if either the source or target are not null and deleted.</returns>
    protected override bool BypassDeletable()
    {
      if (Target.IsDeletedAndNotNull()) return true;
      if (Source is Stat<T, U> stat && stat.IsDeletedAndNotNull()) return true;
      return false;
    }

    /// <summary>
    /// Is the modifier's actual value.
    /// </summary>
    public U Value { get; private set; }

    /// <summary>
    /// Is this modifier's target stat.
    /// </summary>
    public Stat<T, U>? Target { get; private set; } = null;
    Stat<T, U>? IReferencedModifier<T, U>.Target
    {
      set => Target = value;
      get => Target;
    }

    void IReferencedModifier<T, U>.CallDelete() => CallDelete();
    internal void CallDelete() => DoDelete();

    /// <summary>
    /// Is this modifier up to date?
    /// </summary>
    public bool Updated { get; private set; }

    /// <summary>
    /// Updates itself and its target stat. 
    /// </summary>
    public void Update()
    {
      if (Updated) return;
      Updated = true;
      Value = Calculate();
      Target?.Update();
    }

    /// <summary>
    /// Signals itself and its target to update.
    /// </summary>
    public void SignalUpdate()
    {
      if (!Updated) return;
      Updated = false;
      Target?.SignalUpdate();
    }

    #endregion

    #region public

    /// <summary>
    /// Is this modifier's source stat or IValuable object.
    /// </summary>
    public IReadOnlyValuable<U>? Source { get; internal set; } = null;

    #endregion

    #region protected

    /// <summary>
    /// Calculates the modifier's value.
    /// </summary>
    /// <returns>The modifier's new value.</returns>
    protected abstract U Calculate();

    #endregion

    #region private

    private void OnDelete()
    {
      if (!(Target is null) && !Target.Deleted)
      {
        Target.Sources.Remove(this);
        Target.SignalUpdate();
      }
      if (Source is Stat<T, U> stat && !stat.Deleted) stat.Targets.Remove(this);
      Target = null;
      Source = null;
    }

    #endregion
  }
}
