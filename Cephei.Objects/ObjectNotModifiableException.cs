using System;

namespace Cephei.Objects
{
    /// <summary>
    /// ObjectNotModifiableExceptions are to be thrown when an attempt to change one of the object's attributes happens while the object is not modifiable.
    /// </summary>
    public class ObjectNotModifiableException : ObjectException<IModifiable>
    {
        /// <summary>
        /// Creates a new ObjectNotModifiableException with a basic message attached to it.
        /// </summary>
        /// <param name="modif">The modifiable object.</param>
        public ObjectNotModifiableException(IModifiable modif) : base(modif, "The object is not modifiable: '" + modif.ToString() + "'") { }

        /// <summary>
        /// Creates a new ObjectNotModifiableException with a custom message.
        /// </summary>
        /// <param name="modif">The modifiable object.</param>
        /// <param name="message">The message.</param>
        public ObjectNotModifiableException(IModifiable modif, string message) : base(modif, message) { }

        /// <summary>
        /// Creates a new ObjectNotModifiableException with a custom message and an inner exception.
        /// </summary>
        /// <param name="modif">The modifiable object.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public ObjectNotModifiableException(IModifiable modif, string message, Exception inner) : base(modif, message, inner) { }
    }
}
