namespace Cephei.Objects
{
    /// <summary>
    /// The IModifiable interface implements Modifiable{get} which is useful for making objects unable to be modified by external sources.
    /// </summary>
    public interface IModifiable : IReadOnlyModifiable
    {
        /// <summary>
        /// Is the object modifiable?
        /// </summary>
        new bool Modifiable { set; get; }
    }
}
