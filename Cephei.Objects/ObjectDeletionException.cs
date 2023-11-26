using System;

namespace Cephei.Objects
{
    /// <summary>
    /// ObjectDeletionExceptions are thrown when an exception is thrown during the object's deletion. It always contains an inner exception,
    /// since it only references the exception that has occurred during deletion.
    /// </summary>
    public class ObjectDeletionException : ObjectException<IReadOnlyDeletable>
    {
        /// <summary>
        /// Creates a new Deletion exception with default message.
        /// </summary>
        /// <param name="deletable">The deletable object.</param>
        /// <param name="inner">The inner exception.</param>
        public ObjectDeletionException(IReadOnlyDeletable deletable, Exception inner)
            : base(deletable, "An exception has occurred during the object's deletion: '" + deletable.ToString() + "'", inner) { }

        /// <summary>
        /// Creates a new deletable exception with custom message.
        /// </summary>
        /// <param name="deletable">The deletable object.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ObjectDeletionException(IReadOnlyDeletable deletable, string message, Exception inner) : base(deletable, message, inner) { }
    }
}
