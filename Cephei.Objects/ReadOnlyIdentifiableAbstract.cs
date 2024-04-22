using Cephei.Tools;

namespace Cephei.Objects
{
  /// <summary>
  /// The Abstract ReadOnlyIdentifiable object offers a base for IReadOnlyIdentifiable objects.
  /// </summary>
  /// <typeparam name="T">Identifiable value type.</typeparam>
  public abstract class ReadOnlyIdentifiableAbstract<T> : IReadOnlyIdentifiable<T>
  {
    #region overrides

    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    public abstract T ID { get; }

    /// <summary>
    /// Checks if the object is equal to another IReadOnlyIdentifier of the same type.
    /// </summary>
    /// <param name="other">Object to equate to.</param>
    /// <returns>True if both have the same ID.</returns>
    public virtual bool Equals(IReadOnlyIdentifiable<T> other) => other.ID.SafeEquals(ID);
    /// <summary>
    /// Checks if the object is equal to another object.
    /// </summary>
    /// <param name="obj">Object to equate to.</param>
    /// <returns>True if both are identifiable objects with the same ID.</returns>
    public override bool Equals(object obj) => obj is IReadOnlyIdentifiable<T> other && Equals(other);

    /// <summary>
    /// Gets the object's hash code.
    /// </summary>
    /// <returns>The ID's hash code.</returns>
    public override int GetHashCode() => ID is null ? 0 : ID.GetHashCode();

    #endregion
  }
}
