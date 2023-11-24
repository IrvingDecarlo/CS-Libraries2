using System;

namespace Cephei.Objects
{
  /// <summary>
  /// The IReadOnlyIdentifiable interface refers to an object where its ID can only be read.
  /// </summary>
  /// <typeparam name="T">Value type used for the object's identification.</typeparam>
  public interface IReadOnlyIdentifiable<T> : IEquatable<IReadOnlyIdentifiable<T>>
  {
    /// <summary>
    /// Gets the object's ID.
    /// </summary>
    T ID { get; }
  }
}
