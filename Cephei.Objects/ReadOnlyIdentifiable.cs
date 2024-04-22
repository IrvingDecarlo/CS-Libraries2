namespace Cephei.Objects
{
  /// <summary>
  /// The ReadOnlyIdentifiable object implements the IReadOnlyIdentifiable interface.
  /// </summary>
  /// <typeparam name="T">Identifiable value type.</typeparam>
  public class ReadOnlyIdentifiable<T> : ReadOnlyIdentifiableAbstract<T>
  {
    /// <summary>
    /// Instantiates a ReadOnlyIdentifiable object.
    /// </summary>
    /// <param name="id">Id to assign to the object.</param>
    public ReadOnlyIdentifiable(T id) => ID = id;

    #region overrides

    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    public override T ID { get; }

    #endregion
  }
}
