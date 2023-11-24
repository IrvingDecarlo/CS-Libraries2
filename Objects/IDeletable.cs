namespace Cephei.Objects
{
    /// <summary>
    /// IDeletable implements the Delete() method, which deletes the object that implements it. It is NOT to be confused with IDisposable, since IDeletable is to be used
    /// for detaching the object from MANAGED resources.
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Is the object Deletable?
        /// </summary>
        bool Deletable { set; get; }

        /// <summary>
        /// The Deleted flag is to be set when the Delete() method is called. Deleted may be used to hasten the deletion proccess, since it also signals that the object
        /// has already been flagged as Deleted or if it is currently being deleted.
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// The Delete method is used for Deleting the object.
        /// </summary>
        void Delete();
    }
}
