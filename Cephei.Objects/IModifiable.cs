namespace Cephei.Objects
{
    /// <summary>
    /// The IModifiable interface implements Modifiable{get} which is useful for making objects unable to be modified by external sources.
    /// </summary>
    public interface IModifiable
    {
        /// <summary>
        /// Is the object modifiable?
        /// </summary>
        bool Modifiable { set; get; }
    }
}
