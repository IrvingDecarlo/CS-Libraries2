namespace Cephei.Objects
{
    /// <summary>
    /// The IUpdateable interface denotes an object that is to be manually updated.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// The flag determining if the object is updated.
        /// </summary>
        bool Updated { get; }

        /// <summary>
        /// The action to take when the object is to be updated.
        /// </summary>
        void Update();

        /// <summary>
        /// What to do when this object flagged to be updated. Can be as simple as to just set the Updated flag to false.
        /// </summary>
        void SignalUpdate();
    }
}
