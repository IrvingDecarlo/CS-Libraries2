using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
    /// <summary>
    /// MultiEnumerators are enumerators capable of enumerating over multiple other enumerators seamlessly.
    /// </summary>
    /// <typeparam name="T">The collection object type.</typeparam>
    public class MultiEnumerator<T> : AbstractMultiEnumerator<IEnumerator<T>>, IEnumerator<T>
    {
        /// <summary>
        /// Creates a new MultiEnumerator from a collection of enumerables.
        /// </summary>
        /// <param name="enums">Collection of enumerables to draw the enumerators from.</param>
        public MultiEnumerator(params IEnumerable<T>[] enums) : base(enums.GetEnumerators().GetEnumerator())
        { }

        //
        // OVERRIDES
        //

        /// <summary>
        /// Gets the enumerator's current object.
        /// </summary>
        public new T Current => Enumerators.Current.Current;

        /// <summary>
        /// Disposes this MultiEnumerator and the enumerators under it.
        /// </summary>
        public void Dispose()
        {
            Enumerators.Reset();
            while (Enumerators.MoveNext()) Enumerators.Current.Dispose();
            Enumerators.Dispose();
        }
    }
    /// <summary>
    /// A non-generic MultiEnumerator behaves similarly to its non-generic counterpart, albeit built for non-generic IEnumerators.
    /// </summary>
    public class MultiEnumerator : AbstractMultiEnumerator<IEnumerator>
    {
        /// <summary>
        /// Creates a MultiEnumerator based off of multible enumerable collections.
        /// </summary>
        /// <param name="enums">Collections to be enumerated by this enumerator.</param>
        public MultiEnumerator(params IEnumerable[] enums) : base(enums.GetEnumerators().GetEnumerator())
        { }
    }
}
