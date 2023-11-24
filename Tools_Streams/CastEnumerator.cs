using System;
using System.Collections;
using System.Collections.Generic;

namespace Cephei.Collections
{
    /// <summary>
    /// CastEnumerators envelop a non-generic Enumerator under it, returning its Current objects as the desired type.
    /// Note that the enumerator's original object must be convertible to the target type, or InvalidCastExceptions will be thrown.
    /// </summary>
    /// <typeparam name="T">The enumerator's target type.</typeparam>
    public struct CastEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Creates a CastEnumerator, enveloping it over another enumerator.
        /// </summary>
        /// <param name="enum">The enumerator to be enveloped.</param>
        public CastEnumerator(IEnumerator @enum) => enumerator = @enum;

        //
        // OVERRIDES
        //

        /// <summary>
        /// Returns the enumerator's object cast as the target type. Note that InvalidCastExceptions will be thrown if the enumerator's original type is not
        /// convertible to the target type.
        /// </summary>
        /// <exception cref="InvalidCastException"></exception>
        public T Current => (T)enumerator.Current;
        object IEnumerator.Current => enumerator.Current;

        /// <summary>
        /// Moves the enumerator to the next object.
        /// </summary>
        /// <returns>True if the enumerator moved, false if it reached its end.</returns>
        public bool MoveNext() => enumerator.MoveNext();

        /// <summary>
        /// Resets the enumerator.
        /// </summary>
        public void Reset() => enumerator.Reset();

        /// <summary>
        /// Attempts to dispose the enumerator by converting it to the target type. If the conversion is not successful, nothing is done.
        /// </summary>
        public void Dispose()
        {
            if (enumerator is IEnumerator<T>) (enumerator as IEnumerator<T>)?.Dispose();
        }

        //
        // PRIVATE
        //

        // VARIABLES

        private readonly IEnumerator enumerator;
    }

    /// <summary>
    /// CastEnumerators with two type params are built specifically for dictionaries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public readonly struct CastEnumerator<T, U> : IEnumerator<KeyValuePair<T, U>>, IDictionaryEnumerator
    {
        /// <summary>
        /// Creates a new CastEnumerator around a DictionaryEnumerator.
        /// </summary>
        /// <param name="enum">The enumerator to wrap around.</param>
        public CastEnumerator(IDictionaryEnumerator @enum) => enumerator = @enum;

        //
        // OVERRIDES
        //

        /// <summary>
        /// Gets a new KeyValuePair representing the current entry, cast to their appropriate types. May throw InvalidCastException if either the Key or Value
        /// fail to cast.
        /// Note that a new struct is created every time this is called.
        /// </summary>
        public KeyValuePair<T, U> Current
        {
            get
            {
                DictionaryEntry entry = enumerator.Entry;
                return new KeyValuePair<T, U>((T)entry.Key, (U)entry.Value);
            }
        }
        object IEnumerator.Current => Current;

        /// <summary>
        /// Moves the enumerator to the next entry.
        /// </summary>
        /// <returns>True if the enumerator was moved, false if it has reached its end.</returns>
        public bool MoveNext() => enumerator.MoveNext();

        /// <summary>
        /// Resets the enumerator back to its original position.
        /// </summary>
        public void Reset() => enumerator.Reset();

        /// <summary>
        /// Attempts to dispose the object by converting the base enumerator to target type.
        /// </summary>
        public void Dispose()
        {
            if (enumerator is IEnumerator<KeyValuePair<T, U>>) (enumerator as IEnumerator<KeyValuePair<T, U>>)?.Dispose();
        }

        object IDictionaryEnumerator.Key => enumerator.Key;
        object IDictionaryEnumerator.Value => enumerator.Value;
        DictionaryEntry IDictionaryEnumerator.Entry => enumerator.Entry;

        //
        // PRIVATE
        //

        // VARIABLES

        private readonly IDictionaryEnumerator enumerator;
    }
}
