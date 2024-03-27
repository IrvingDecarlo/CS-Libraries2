namespace Cephei.Objects
{
    /// <summary>
    /// The ITickable interface denotes that the object can be ticked and it also has a visible Tick value.
    /// </summary>
    /// <typeparam name="T">The tick type.</typeparam>
    public interface ITickable<T>
    {
        /// <summary>
        /// The value representation of the object's ticks.
        /// </summary>
        T Ticks { set; get; }

        /// <summary>
        /// Ticks the object by a certain amount.
        /// </summary>
        /// <param name="amount">The amount to tick it for.</param>
        void Tick(T amount);
    }
    /// <summary>
    /// ITickable denotes that an object has Ticks (meaning it is temporary by nature). The non-generic interface only implements Tick() in order to be a simplistic
    /// denotation that the object can be ticked. It is normally used in conjunction with its generic counterpart by an object that inherits the main object.
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// Ticks the object.
        /// </summary>
        void Tick();
    }
}
