using System.Collections;

namespace Cephei.Collections
{
    /// <summary>
    /// EmptyDictionaryEnumerators are useful to behave as an IDictionaryEnumerator but without any content, returning null, empty and false in every method.
    /// </summary>
    public readonly struct EmptyDictionaryEnumerator : IDictionaryEnumerator
    {
        //
        // OVERRIDES
        //

        /// <summary>
        /// Returns an empty default DictionaryEntry.
        /// </summary>
        public DictionaryEntry Entry => default;

        /// <summary>
        /// Returns a null key.
        /// </summary>
        public object? Key => null;

        /// <summary>
        /// Returns a null value.
        /// </summary>
        public object? Value => null;

        /// <summary>
        /// This Enumerator cannot move forward, so it always returns false.
        /// </summary>
        /// <returns>False by default.</returns>
        public bool MoveNext() => false;

        /// <summary>
        /// Reset does nothing, since there is nothing to reset.
        /// </summary>
        public void Reset() { }

        /// <summary>
        /// Returns a null object by default.
        /// </summary>
        public object? Current => null;

        //
        // STATIC
        //

        static EmptyDictionaryEnumerator() => Default = new EmptyDictionaryEnumerator();

        /// <summary>
        /// Reference to an EnmptyDictionaryEnumerator to prevent the instantiation of additional objects.
        /// </summary>
        public static readonly EmptyDictionaryEnumerator Default;
    }
}
