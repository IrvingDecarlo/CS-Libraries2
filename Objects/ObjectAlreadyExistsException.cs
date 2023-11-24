namespace Cephei.Objects
{
    /// <summary>
    /// ObjectAlreadyExists exceptions can be thrown when an object is created with conflicting ID.
    /// </summary>
    public class ObjectAlreadyExistsException : ObjectException<object>, IObjectRelatedException<object, object?>
    {
        /// <summary>
        /// Creates an ObjectAlreadyExists exception with a basic message denoting the object's type and it to string.
        /// </summary>
        /// <param name="obj">Any object.</param>
        /// <param name="rel">Reference to any object related in the exception.</param>
        public ObjectAlreadyExistsException(object obj, object? rel = null) 
            : this(obj, "A " + obj.GetType().ToString() + " already exists" + (rel != null ? " in " + rel.ToString() : "" ) + ". Object: " + obj.ToString(), rel)
        { }
        /// <summary>
        /// Creates an ObjectAlreadyExists exception with a custom message.
        /// </summary>
        /// <param name="obj">The IIdentifiable object.</param>
        /// <param name="message">The message.</param>
        /// <param name="rel">Reference to a related object. Can be null for no reference.</param>
        public ObjectAlreadyExistsException(object obj, string message, object? rel = null) : base(obj, message) => RelatedObject = rel;

        //
        // OVERRIDES
        //

        /// <summary>
        /// Reference to another object involved in the exception.
        /// </summary>
        public object? RelatedObject { private set; get; }
    }
}
