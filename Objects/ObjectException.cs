using System;

namespace Cephei.Objects
{
    /// <summary>
    /// The non-generic ObjectException, unlike its generic counterpart, is limited to referencing an object and an inner exception that has occurred to it.
    /// It is useful when it is intended to throw an Exception that is not covered by the Cephei.Objects namespace while referencing the source object
    /// at the same time.
    /// </summary>
    public class ObjectException : ObjectException<object>
    {
        /// <summary>
        /// Creates an ObjectException, referencing the object and the exception.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        /// <param name="inner">The exception that has occurred.</param>
        public ObjectException(object obj, Exception inner) : base(obj, "The '" + obj.GetType().ToString() + "' had an exception.", inner)
        { }
    }
    /// <summary>
    /// ObjectExceptions offer a base to any exception that must reference an object.
    /// </summary>
    /// <typeparam name="T">The object type for the exception.</typeparam>
    public class ObjectException<T> : Exception, IObjectException<T>
    {
        /// <summary>
        /// Creates the exception, referencing an object.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        public ObjectException(T obj) : base() => Object = obj;

        /// <summary>
        /// Creates the exception, adding a message to it.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        /// <param name="message">The message to be added.</param>
        public ObjectException(T obj, string message) : base(message) => Object = obj;

        /// <summary>
        /// Creates a new exception, adding a message and an inner exception to it.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        /// <param name="message">The message to be added.</param>
        /// <param name="inner">An inner exception.</param>
        public ObjectException(T obj, string message, Exception inner) : base(message, inner) => Object = obj;

        /// <summary>
        /// The object referenced by the exception.
        /// </summary>
        public T Object { private set; get; }
    }
}
