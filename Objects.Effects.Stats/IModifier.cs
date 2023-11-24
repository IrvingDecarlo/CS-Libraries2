using Cephei.Valuables;

namespace Cephei.Objects.Effects.Stats
{
  /// <summary>
  /// The IModifier interface can be used as both a source and a target for stats.
  /// </summary>
  /// <typeparam name="T">The IIdentifiable type.</typeparam>
  /// <typeparam name="U">The IValuable type.</typeparam>
  public interface IModifier<T, U> : IReadOnlyIdentifiable<T>, IReadOnlyValuable<U>, IUpdateable
  {

  }
}
