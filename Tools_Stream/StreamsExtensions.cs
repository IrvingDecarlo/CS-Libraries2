using System.Collections.Generic;
using System.IO;

namespace Cephei
{
    using Tools;

    namespace Streams
    {
        /// <summary>
        /// StreamsExtensions contains extension methods for streams.
        /// </summary>
        public static class StreamsExtensions
        {
            /// <summary>
            /// Writes a KeyValuePair enumerable containing another KeyValuePair enumerable in its value in a stream.
            /// </summary>
            /// <typeparam name="T">The main enumerable's key type.</typeparam>
            /// <typeparam name="U">The secondary enumerable's key type.</typeparam>
            /// <typeparam name="V">The secondary enumerable's value type.</typeparam>
            /// <typeparam name="W">The secondary enumerable's type.</typeparam>
            /// <param name="writer">TextWriter to use.</param>
            /// <param name="dict">The collection to print.</param>
            public static void WriteLine<T, U, V, W>(this TextWriter writer, IEnumerable<KeyValuePair<T, W>> dict) where W : IEnumerable<KeyValuePair<U, V>>
            {
                foreach (KeyValuePair<T, W> sect in dict)
                {
                    writer.WriteLine($"[{sect.Key}]");
                    foreach (KeyValuePair<U, V> kvp in sect.Value) writer.WriteLine(kvp.ToPairString());
                }
            }
        }
    }
}