using System;
using System.ComponentModel;

namespace Cephei.Objects
{
    /// <summary>
    /// ObjectWarningExceptions are to behave similarly to WarningExceptions, but referencing the target object.
    /// </summary>
    /// <typeparam name="T">Any object.</typeparam>
    public class ObjectWarningException<T> : WarningException, IObjectException<T>
    {
        /// <summary>
        /// Creates a new ObjectWarningException, referencing an object.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        public ObjectWarningException(T obj) : base() => Object = obj;

        /// <summary>
        /// Creates the exception, adding a message to it.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        /// <param name="message">The message to be added.</param>
        public ObjectWarningException(T obj, string message) : base(message) => Object = obj;

        /// <summary>
        /// Creates a new exception, adding a message and an inner exception to it.
        /// </summary>
        /// <param name="obj">The object to be referenced.</param>
        /// <param name="message">The message to be added.</param>
        /// <param name="inner">An inner exception.</param>
        public ObjectWarningException(T obj, string message, Exception inner) : base(message, inner) => Object = obj;

        /// <summary>
        /// The object referenced by the exception.
        /// </summary>
        public T Object { private set; get; }
    }
}
