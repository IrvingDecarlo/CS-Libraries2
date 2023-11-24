using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
    /// <summary>
    /// AbstractMultiEnumerators offer a base for the MultiEnumerator classes.
    /// </summary>
    /// <typeparam name="T">The IEnumerable object.</typeparam>
    public abstract class AbstractMultiEnumerator<T> : IEnumerator where T : IEnumerator
    {
        /// <summary>
        /// Creates a new MultiEnumerator from a collection of enumerables.
        /// </summary>
        /// <param name="enums">Collection of enumerables to draw the enumerators from.</param>
        public AbstractMultiEnumerator(IEnumerator<T> enums) => Enumerators = enums;

        //
        // OVERRIDES
        //

        /// <summary>
        /// The MultiEnumerator's current object.
        /// </summary>
        public object Current => CurrentEnumerator.Current;

        /// <summary>
        /// Iterates to the next object.
        /// </summary>
        /// <returns>True if the iteration was successful, false if all enumerators have reached their end.</returns>
        public bool MoveNext()
        {
            while (!Enumerators.Current.MoveNext())
            {
                if (!Enumerators.MoveNext()) return false;
            }
            return true;
        }

        /// <summary>
        /// Resets itself and all enumerators under this MultiEnumerator.
        /// </summary>
        public void Reset()
        {
            Enumerators.Reset();
            while (Enumerators.MoveNext()) Enumerators.Current.Reset();
            Enumerators.Reset();
        }

        //
        // PUBLIC
        //

        // PROPERTIES

        /// <summary>
        /// Reference to the enumerator currently being iterated through.
        /// </summary>
        public T CurrentEnumerator => Enumerators.Current;

        //
        // PROTECTED
        //

        // VARIABLES

        /// <summary>
        /// The enumerator handling all enumerators under it.
        /// </summary>
        protected readonly IEnumerator<T> Enumerators;
    }
}
