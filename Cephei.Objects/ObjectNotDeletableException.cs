using System;

namespace Cephei.Objects
{
    /// <summary>
    /// The ObjectNotDeletable exception is to be thrown when the Delete() method is called, but the object is not deletable.
    /// </summary>
    public class ObjectNotDeletableException : ObjectException<IReadOnlyDeletable>
    {
        /// <summary>
        /// Creates the exception, adding a basic message referencing the object.
        /// </summary>
        /// <param name="deletable">The deletable object reference.</param>
        public ObjectNotDeletableException(IReadOnlyDeletable deletable) : base(deletable, "The object is not deletable: '" + deletable.ToString() + "'") { }

        /// <summary>
        /// Creates the exception with a custom message.
        /// </summary>
        /// <param name="deletable">The deletable object reference.</param>
        /// <param name="message">The message to be added.</param>
        public ObjectNotDeletableException(IReadOnlyDeletable deletable, string message) : base(deletable, message) { }

        /// <summary>
        /// Creates the exception with an inner exception and a message.
        /// </summary>
        /// <param name="deletable">The deletable object.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ObjectNotDeletableException(IReadOnlyDeletable deletable, string message, Exception inner) : base(deletable, message, inner) { }
    }
}
