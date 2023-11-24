using System;

namespace Cephei.Files
{
    /// <summary>
    /// The generic version of FileReadException contains a direct reference to the exception's line.
    /// </summary>
    /// <typeparam name="T">The type to use to build the reference to the line.</typeparam>
    public class FileReadException<T> : FileReadException where T : IConvertible
    {
        /// <summary>
        /// Creates a new FileReadException with a message and reference to the file's path.
        /// </summary>
        /// <param name="path">Path to the file related to this exception.</param>
        /// <param name="line">Line in which the exception has occurred.</param>
        /// <param name="message">The custom message. Can be an empty string "" to not have a message, only the refecerence to the file and the line.</param>
        public FileReadException(string path, T line, string message = "") : base(path, line.ToString(), message)
            => Line = line;

        //
        // OVERRIDES
        //

        /// <summary>
        /// Gets the line in which the exception took place.
        /// </summary>
        /// <returns>The line, as a string.</returns>
        public sealed override uint GetLine() => Convert.ToUInt32(Line);

        //
        // PUBLIC
        //

        // VARIABLES

        /// <summary>
        /// The line in which the exception took place.
        /// </summary>
        public readonly T Line;
    }
    /// <summary>
    /// FileReadExceptions are thrown when an exception occurs while reading a file. Note that this must directly relate to the file's content, not to
    /// an exception encountered by the reader regarding another issue.
    /// </summary>
    public abstract class FileReadException : Exception
    {
        /// <summary>
        /// Creates a new FileReadException with a message and reference to the file's path.
        /// </summary>
        /// <param name="path">Path to the file related to this exception.</param>
        /// <param name="line">Line in which the exception has occurred.</param>
        /// <param name="message">The custom message. Can be an empty string "" to not have a message, only the refecerence to the file and the line.</param>
        public FileReadException(string path, string line, string message = "")
            : base($"An exception has occurred while reading the file '{path}' at line {line}" + (message == "" ? "." : $":\n{message}"))
            => Path = path;

        //
        // PUBLIC
        //

        // VARIABLES

        /// <summary>
        /// Path to the file related to this exception.
        /// </summary>
        public readonly string Path;

        // METHODS

        /// <summary>
        /// Gets the file's line in which the exception took place as an uint.
        /// </summary>
        /// <returns>The exception's line.</returns>
        public abstract uint GetLine();
    }
}
