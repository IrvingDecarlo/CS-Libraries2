using System;
using System.IO;
using System.Diagnostics;

namespace Cephei.Benchmarking
{
    /// <summary>
    /// The BenchmarkerDelegated class is similar to Benchmarker, but will call a delegate on MainMethod().
    /// </summary>
    public class BenchmarkerDelegated : Benchmarker
    {
        /// <summary>
        /// Creates a new benchmarker.
        /// </summary>
        /// <param name="action">Action for it to execute.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action) : this(action, new Stopwatch())
        { }
        /// <summary>
        /// Creates a new benchmarker.
        /// </summary>
        /// <param name="action">Action for it to execute.</param>
        /// <param name="loops">Amount of times to execute the action.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action, int loops) : this(action, new Stopwatch(), loops)
        { }
        /// <summary>
        /// Creates a new Benchmarker, outputting data to a writer.
        /// </summary>
        /// <param name="action">The action to execute in the main method.</param>
        /// <param name="writer">Writer to output data to.</param>
        /// <param name="loops">Number of times to loop the main method.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action, TextWriter writer, int loops) : this(action, writer, new Stopwatch(), loops)
        { }
        /// <summary>
        /// Creates a new benchmarker.
        /// </summary>
        /// <param name="action">Action for it to execute.</param>
        /// <param name="stopwatch">Stopwatch to assign to it.</param>
        /// <param name="loops">Amount of times to execute the action.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action, Stopwatch stopwatch, int loops) : this(action, stopwatch) => Benchmark(loops);
        /// <summary>
        /// Creates a new benchmarker.
        /// </summary>
        /// <param name="action">Action for it to execute.</param>
        /// <param name="writer">Writer to assign to it.</param>
        /// <param name="stopwatch">Stopwatch to assign to it.</param>
        /// <param name="loops">Amount of times to execute the action.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action, TextWriter writer, Stopwatch stopwatch, int loops) : this(action, stopwatch) => Benchmark(writer, loops);
        /// <summary>
        /// Creates a new benchmarker.
        /// </summary>
        /// <param name="action">Action for it to execute.</param>
        /// <param name="stopwatch">Stopwatch to assign to it.</param>
        public BenchmarkerDelegated(Action<Benchmarker> action, Stopwatch stopwatch) : base(stopwatch) => MainAction = action;

        //
        // OVERRIDES
        //

        /// <summary>
        /// Calls its MainAction delegate.
        /// </summary>
        public override void MainMethod() => MainAction(this);

        //
        // PUBLIC
        //

        // VARIABLES

        /// <summary>
        /// The MainAction that this Benchmarker is to call.
        /// </summary>
        public Action<Benchmarker> MainAction;
    }
}
