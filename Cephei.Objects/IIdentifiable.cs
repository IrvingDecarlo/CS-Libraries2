namespace Cephei.Objects
{
    /// <summary>
    /// IIdentifiable provides an ID variable for the object.
    /// </summary>
    /// <typeparam name="T">Any struct/class to be used as base for comparisons.</typeparam>
    public interface IIdentifiable<T> : IReadOnlyIdentifiable<T>
    {
        /// <summary>
        /// The ID is the primary comparison variable for Identifiable objects.
        /// </summary>
        new T ID { set; get; }
    }
}
