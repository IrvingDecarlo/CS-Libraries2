namespace Cephei.Objects
{
    /// <summary>
    /// ObjectException interfaces offer a common base for all ObjectExceptions.
    /// </summary>
    /// <typeparam name="T">Any object type.</typeparam>
    public interface IObjectException<T>
    {
        /// <summary>
        /// The object that is referenced by this exception.
        /// </summary>
        T Object { get; }
    }
}
