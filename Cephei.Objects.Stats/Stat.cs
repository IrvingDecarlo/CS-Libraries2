using Cephei.Valuables;
using System;

namespace Cephei.Objects.Stats
{
  /// <summary>
  /// Stats are objects that have their values manipulated by Modifiers.
  /// </summary>
  /// <typeparam name="T">The stat's Identifiable type.</typeparam>
  /// <typeparam name="U">The stat's IValuable type.</typeparam>
  public abstract class Stat<T, U> : PersistentReadonlyObject<T>, IReadOnlyValuable<U>, IUpdateable, IComparable<Stat<T, U>>
  {
    /// <summary>
    /// Creates a new stat.
    /// </summary>
    /// <param name="id">The stat's ID.</param>
    /// <param name="value">The stat's base value.</param>
    public Stat(T id, U value) : base(id)
    {
      this.value = value;
      ValueBase = value;
      Targets = new ModifierCollection<T, Modifier<T, U>, U>();
      Sources = new ModifierCollection<T, IModifier<T, U>, U>();
      OnDeleted += OnDelete;
    }

    #region overrides

    /// <summary>
    /// Returns a string defining this stat (its ID, value and number of sources and targets).
    /// </summary>
    /// <returns>A string defining this stat.</returns>
    public override string ToString()
      => $"Stat ID='{ID}' Value='{value}' ({ValueBase}) Sources: {Sources.Count} Targets: {Targets.Count} Updated: {Updated}";

    /// <summary>
    /// Gets the stat's value.
    /// </summary>
    public U Value => value;

    /// <summary>
    /// Is this stat up to date?
    /// </summary>
    public bool Updated { get; private set; }

    /// <summary>
    /// Updates itself using its sources' updated values and then updates its target modifiers.
    /// Will do nothing if the stat is already up to date.
    /// </summary>
    public void Update()
    {
      if (Updated) return;
      Updated = true;
      value = PreCalculate();
      foreach (IModifier<T, U> mod in Sources.Values)
      {
        mod.Update();
        value = Calculate(mod.Value);
      }
      value = PostCalculate();
      Targets.Values.UpdateAll();
    }

    /// <summary>
    /// Signals itself and its target modifiers to update. Does nothing if it is already pending an update.
    /// </summary>
    public void SignalUpdate()
    {
      if (!Updated) return;
      Updated = false;
      Targets.Values.SignalUpdateAll();
    }

    /// <summary>
    /// Compares this stat to another for sorting.
    /// </summary>
    /// <param name="stat">Other stat to compare to.</param>
    /// <returns>1 if it is bigger than, 0 equal to and -1 if smaller than the other one.</returns>
    public abstract int CompareTo(Stat<T, U> stat);

    #endregion

    #region public

    /// <summary>
    /// Removes a source modifier from the stat. Throws NotModifiable exeption if the stat isn't modifiable or NotDeletable if the source modifier object is not deletable.
    /// </summary>
    /// <param name="mod">Modifier to remove.</param>
    /// <returns>True if the modifier was located and removed.</returns>
    /// <exception cref="ObjectNotModifiableException"></exception>
    /// <exception cref="ObjectNotDeletableException"></exception>
    /// <remarks>Removing a target can only be done by deleting said target modifier.</remarks>
    public bool RemoveSource(IModifier<T, U> mod)
    {
      if (!Modifiable) throw new ObjectNotModifiableException(this);
      if (mod is IReferencedModifier<T, U> modif && !modif.Deletable) throw new ObjectNotDeletableException(modif);
      return DoRemoveSource(mod);
    }

    /// <summary>
    /// Adds a source modifier to this stat. May throw ObjectNotModifiable exceptions if the stat or if the Modifier aren't modifiable.
    /// </summary>
    /// <param name="mod">Modifier to add as a source.</param>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public void AddSource(IModifier<T, U> mod)
    {
      if (!Modifiable) throw new ObjectNotModifiableException(this);
      if (mod is IModifiable modif && !modif.Modifiable) throw new ObjectNotModifiableException(modif);
      DoAddSource(mod);
    }

    /// <summary>
    /// Adds a target modifier to this stat. May throw ObjectNotModifiable exceptions if the stat or if the Modifier aren't modifiable.
    /// </summary>
    /// <param name="mod">Modifier to add.</param>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public void AddTarget(Modifier<T, U> mod)
    {
      if (!Modifiable) throw new ObjectNotModifiableException(this);
      if (!mod.Modifiable) throw new ObjectNotModifiableException(mod);
      DoAddTarget(mod);
    }

    /// <summary>
    /// Gets or sets the stat's base value.
    /// </summary>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public U BaseValue
    {
      set
      {
        if (!Modifiable) throw new ObjectNotModifiableException(this);
        ValueBase = value;
        SignalUpdate();
      }
      get => ValueBase;
    }

    /// <summary>
    /// Is the collection of target modifiers this stat has.
    /// </summary>
    public readonly ModifierCollection<T, Modifier<T, U>, U> Targets;

    /// <summary>
    /// Is the collection of source modifiers this stat has.
    /// </summary>
    public readonly ModifierCollection<T, IModifier<T, U>, U> Sources;

    #endregion

    #region protected

    // METHODS

    /// <summary>
    /// The Calculate method determines how the stats combines its value with its source modifiers'. It should be as simple as an addition.
    /// To get the value, use the Value property.
    /// </summary>
    /// <param name="modvalue">The modifier's value to add to the current value.</param>
    /// <returns>The calculated stat's value.</returns>
    protected abstract U Calculate(U modvalue);

    /// <summary>
    /// Returns the Pre-Calculate (base) value prior to all modifier calculations.
    /// </summary>
    /// <returns>Returns the stat's ValueBase by default.</returns>
    protected virtual U PreCalculate() => ValueBase;

    /// <summary>
    /// Returns the Post-Calculate (final) value after all modifier calculations.
    /// </summary>
    /// <returns>Returns the stat's final value without any modifications.</returns>
    protected virtual U PostCalculate() => Value;

    /// <summary>
    /// Removes a source modifier, deleting it if it is a Modifier object.
    /// </summary>
    /// <param name="mod">Modifier to remove.</param>
    /// <returns>True if the modifier was located and removed.</returns>
    protected bool DoRemoveSource(IModifier<T, U> mod)
    {
      if (!Sources.Contains(mod)) return false;
      if (mod is IReferencedModifier<T, U> modf) modf.CallDelete();
      else Sources.Remove(mod);
      return true;
    }

    /// <summary>
    /// Adds a source modifier to this stat, bypassing Modifiable flags. Throws exception if the modifier already has a target.
    /// </summary>
    /// <param name="mod">Modifier to add.</param>
    /// <exception cref="ObjectAlreadyExistsException"></exception>
    protected void DoAddSource(IModifier<T, U> mod)
    {
      if (mod is IReferencedModifier<T, U> modf)
      {
        if (!(modf.Target is null)) throw new ObjectAlreadyExistsException(modf.Target);
        modf.Target = this;
      }
      Sources.Add(mod);
      SignalUpdate();
    }

    /// <summary>
    /// Adds a target modifier, bypassing modifiable flags. Throws exception if the modifier already has a source.
    /// </summary>
    /// <param name="mod">Modifier to add.</param>
    /// <exception cref="ObjectAlreadyExistsException"></exception>
    protected void DoAddTarget(Modifier<T, U> mod)
    {
      if (!(mod.Source is null)) throw new ObjectAlreadyExistsException(mod.Source);
      mod.Source = this;
      Targets.Add(mod);
      mod.SignalUpdate();
    }

    /// <summary>
    /// Raw access to the stat's base value.
    /// </summary>
    protected U ValueBase;

    #endregion

    #region private

    // VARIABLES

    U value;

    // METHODS

    private void OnDelete()
    {
      foreach (IModifier<T, U> mod in Sources.Values)
      {
        if (mod is IReferencedModifier<T, U> modif) modif.CallDelete();
      }
      Sources.Clear();
      foreach (Modifier<T, U> mod in Targets.Values) mod.CallDelete();
      Targets.Clear();
    }

    #endregion
  }
}
