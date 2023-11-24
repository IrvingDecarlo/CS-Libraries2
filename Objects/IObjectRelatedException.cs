namespace Cephei.Objects
{
    /// <summary>
    /// The IObjectRelatedException Interface is used when an IObjectException is to refer to another object involved in the exception.
    /// </summary>
    /// <typeparam name="T">Type of the main object involved.</typeparam>
    /// <typeparam name="U">Type of the related object involved.</typeparam>
    public interface IObjectRelatedException<T, U> : IObjectException<T>
    {
        /// <summary>
        /// Reference to the related object.
        /// </summary>
        U RelatedObject { get; }
    }
}
