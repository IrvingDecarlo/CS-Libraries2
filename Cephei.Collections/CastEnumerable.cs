using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
    /// <summary>
    /// CastEnumerables are enumerables that cast an underlying enumerable to the target type, using a CastEnumerator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct CastEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Creates a CastEnumerable wrapped around an existing Enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable to be wrapped around.</param>
        public CastEnumerable(IEnumerable enumerable) => enumerator = new CastEnumerator<T>(enumerable.GetEnumerator());

        //
        // OVERRIDES
        //

        /// <summary>
        /// Gets the CastEnumerable's enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator() => enumerator;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //
        // PRIVATE
        //

        // VARIABLES

        private readonly CastEnumerator<T> enumerator;
    }
}
