using System;

namespace Cephei.Objects
{
  /// <summary>
  /// ObjectDoesNotExistExceptions are thrown when an object is not present in the static system (if the related object is null) or when it is not attached to a related object.
  /// </summary>
  public class ObjectDoesNotExistException : ObjectException<object?>
  {
    /// <summary>
    /// Creates an ObjectDoesNotExist exception with a basic message denoting the object's type and its ID.
    /// </summary>
    /// <param name="type">The type of the object that was being searched for.</param>
    /// <param name="id">The object's supposed ID.</param>
    /// <param name="obj">Object involved.</param>
    public ObjectDoesNotExistException(Type type, object? id, object? obj = null) : this(type, " of ID='" + id?.ToString() + "'", obj)
    { }
    /// <summary>
    /// Creates an ObjectDoesNotExist exception with a basic message denoting the object's type and it to string.
    /// </summary>
    /// <param name="obj">Any object.</param>
    /// <param name="type">Type of the object that was being searched for.</param>
    /// <param name="addmsg">Additional message to further explain the issue.</param>
    public ObjectDoesNotExistException(Type type, string addmsg, object? obj = null)
        : this("A " + type.ToString() + addmsg + " does not exist" + (obj != null ? " in " + obj.ToString() : "") + ".", obj)
    { }
    /// <summary>
    /// Creates an ObjectDoesNotExist exception with a custom message.
    /// </summary>
    /// <param name="obj">Any object.</param>
    /// <param name="message">The message.</param>
    public ObjectDoesNotExistException(string message, object? obj = null) : base(obj, message)
    { }
  }
}
