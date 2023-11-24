using System;

namespace Cephei.Objects
{
    /// <summary>
    /// The ObjectDeleted exception is to be thrown when the Delete() method is called and the object has already been deleted.
    /// Use ObjectDeletionException when an exception has occurred DURING deletion.
    /// </summary>
    public class ObjectDeletedException : ObjectException<IDeletable>
    {
        /// <summary>
        /// Creates the exception, adding a basic message referencing the object.
        /// </summary>
        /// <param name="deletable">The deletable object reference.</param>
        public ObjectDeletedException(IDeletable deletable) : base(deletable, "The object has already been deleted: '" + deletable.ToString() + "'") { }

        /// <summary>
        /// Creates the exception with a custom message.
        /// </summary>
        /// <param name="deletable">The deletable object reference.</param>
        /// <param name="message">The message to be added.</param>
        public ObjectDeletedException(IDeletable deletable, string message) : base(deletable, message) { }

        /// <summary>
        /// Creates the exception with an inner exception and a message.
        /// </summary>
        /// <param name="deletable">The deletable object.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ObjectDeletedException(IDeletable deletable, string message, Exception inner) : base(deletable, message, inner) { }
    }
}
