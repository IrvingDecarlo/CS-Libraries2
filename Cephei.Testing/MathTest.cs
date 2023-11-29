using Cephei.Benchmarking;
using Cephei.Commands;
using Cephei.Commands.Consoles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class MathTest
  {
    internal static Command MathCommand = CreateCommand(null, null, null, "math");

    internal static void AddCommands()
    {
      CreateCommand((x) =>
      {
        int value = int.Parse(x["value"][0]);
        int mult = int.Parse(x["percent"][0]);
        int loops = x.TryGetValue("loops", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 1000000000;
        int variant = x.TryGetValue("variant", out args) ? int.Parse(args[0]) : 0;
        Stopwatch sw = new Stopwatch();
        float fmult = mult / 100f;
        int m256 = (mult * 256) / 100;
        BenchmarkerDelegated bench = new BenchmarkerDelegated(null);
        switch (variant)
        {
          case 0:
            Benchmark(() => (int)(value * fmult), bench, "Multiply by Float", loops);
            Benchmark(() => (value * mult) / 100, bench, "Multiply by Int Percent", loops);
            Benchmark(() => (value * m256) >> 8, bench, "Multiply by Int th256 mult=" + m256, loops);
            break;
          case 1:
            Benchmark(() => (value * mult) / 100, bench, "Multiply by Int Percent", loops);
            Benchmark(() => (value * m256) >> 8, bench, "Multiply by Int th256 mult=" + m256, loops);
            Benchmark(() => (int)(value * fmult), bench, "Multiply by Float", loops);
            break;
          case 2:
            Benchmark(() => (value * m256) >> 8, bench, "Multiply by Int th256 mult=" + m256, loops);
            Benchmark(() => (int)(value * fmult), bench, "Multiply by Float", loops);
            Benchmark(() => (value * mult) / 100, bench, "Multiply by Int Percent", loops);
            break;
        }
      }, null, MathCommand, "multiply", "mult");
    }

    private static void Benchmark(Func<int> func, BenchmarkerDelegated bench, string txt, int loops)
    {
      Out.WriteLine();
      Out.WriteLine(txt + " Value: " + func() + " Looped " + loops);
      bench.MainAction = (x) => func();
      bench.Benchmark(Out, loops);
    }
  }
}
