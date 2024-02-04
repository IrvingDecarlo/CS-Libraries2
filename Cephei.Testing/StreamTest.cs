using Cephei.Benchmarking;
using Cephei.Commands;
using Cephei.Commands.Consoles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class StreamTest
  {
    internal static Command Main = CreateCommand(null, null, null, "streams", "stream");

    internal static void AddCommands()
    {
      Command cmd = CreateCommand(null, null, Main, "writer");
      CreateCommand((x) =>
      {
        BenchmarkArguments bargs = new BenchmarkArguments(x);
        int buffersize = x.TryGetValue("buffersize", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 16;
        int totalsize = bargs.GetTotalSize();
        Out.WriteLine($"Benchmarking stream writer: StringSize={bargs.stringsize} Number={bargs.number} TotalSize={totalsize} BufferSize={buffersize}");
        Out.WriteLine("Generating string array...");
        Random random = new Random();
        string[] strings = new string[bargs.number];
        char[] chars = new char[bargs.stringsize];
        int i;
        for (i = 0; i < bargs.number; i++) strings[i] = GenerateString(chars, random, bargs.stringsize);
        Out.WriteLine("String array generated.");
        BenchmarkerDelegated bench = new BenchmarkerDelegated(null);
        string nl = Environment.NewLine;
        string filepath = Path.Combine(bargs.path, "streamtest-stringbuilder.txt");
        Benchmark((x) =>
        {
          StringBuilder sb = new StringBuilder();
          for (i = 0; i < bargs.number; i++)
          {
            sb.Append(i);
            sb.Append(": ");
            sb.Append(strings[i]);
            sb.Append(nl);
          }
          string txt = sb.ToString();
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          using StreamWriter writer = new StreamWriter(filepath);
          writer.WriteLine(txt);
          Out.WriteLine("Content outputted to test file.");
        }, bench, filepath, "String Builder");
        filepath = Path.Combine(bargs.path, "streamtest-memorystreamwriter.txt");
        Benchmark((x) =>
        {
          int tsize = totalsize * 2;
          byte[] buffer = new byte[tsize];
          Out.WriteLine($"Underlying memory stream buffer size set to {tsize}.");
          using MemoryStream memory = new MemoryStream(buffer);
          using StreamWriter writer = new StreamWriter(memory);
          for (i = 0; i < bargs.number; i++)
          {
            writer.Write(i);
            writer.Write(": ");
            writer.Write(strings[i]);
            writer.Write(nl);
          }
          writer.Flush();
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          using Stream filestream = File.OpenWrite(filepath);
          filestream.Write(buffer, 0, (int)memory.Position);
          Out.WriteLine("Content outputted to test file.");
        }, bench, filepath, "Memory StreamWriter");
        filepath = Path.Combine(bargs.path, "streamtest-memorystreamwriterasync.txt");
        Benchmark(async (x) =>
        {
          int tsize = totalsize * 2;
          byte[] buffer = new byte[tsize];
          Out.WriteLine($"Underlying memory stream buffer size set to {tsize}.");
          using MemoryStream memory = new MemoryStream(buffer);
          using StreamWriter writer = new StreamWriter(memory);
          for (i = 0; i < bargs.number; i++)
          {
            await writer.WriteAsync(i.ToString());
            await writer.WriteAsync(": ");
            await writer.WriteAsync(strings[i]);
            await writer.WriteAsync(nl);
          }
          await writer.FlushAsync();
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          using Stream filestream = File.OpenWrite(filepath);
          filestream.Write(buffer, 0, (int)memory.Position);
          Out.WriteLine("Content outputted to test file.");
        }, bench, filepath, "Memory StreamWriter ASYNC");
        filepath = Path.Combine(bargs.path, "streamtest-directmemorystream.txt");
        Benchmark((x) =>
        {
          int tsize = totalsize * 2;
          byte[] membuffer = new byte[tsize];
          byte[] buffer = new byte[buffersize];
          Out.WriteLine($"Underlying memory stream buffer size set to {tsize}.");
          using MemoryStream memory = new MemoryStream(membuffer, 0, tsize, true);
          for (i = 0; i < bargs.number; i++)
          {
            WriteToMemory(memory, buffer, i.ToString());
            WriteToMemory(memory, buffer, ": ");
            WriteToMemory(memory, buffer, strings[i]);
            WriteToMemory(memory, buffer, nl);
          }
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          using Stream filestream = File.OpenWrite(filepath);
          filestream.Write(membuffer, 0, (int)memory.Position);
          Out.WriteLine("Content outputted to test file.");
        }, bench, filepath, "Direct MemoryStream");
      }, null, cmd, "benchmark", "bm");

      cmd = CreateCommand(null, null, Main, "reader");
      CreateCommand((x) =>
      {
        BenchmarkArguments bargs = new BenchmarkArguments(x);
        int totalsize = bargs.GetTotalSize();
        Out.WriteLine($"Benchmarking stream reader: StringSize={bargs.stringsize} Number={bargs.number} TotalSize={totalsize}");
        Out.WriteLine("Generating string on memory...");
        MemoryStream memory = new MemoryStream(totalsize + 3);
        using StreamWriter writer = new StreamWriter(memory, Encoding.UTF8, -1, true);
        char[] chars = new char[bargs.stringsize];
        Random random = new Random();
        for (int i = 0; i < bargs.number; i++) writer.Write(GenerateString(chars, random, bargs.stringsize));
        writer.Flush();
        Out.WriteLine("MemoryStream filled.");
        BenchmarkerDelegated bench = new BenchmarkerDelegated(null);
        string filepath = Path.Combine(bargs.path, "streamreader-substring.txt");
        Benchmark((x) =>
        {
          string[] strings = new string[bargs.number];
          using StreamReader reader = OpenReader(memory);
          string str = reader.ReadToEnd();
          for (int i = 0; i < bargs.number; i++) strings[i] = str.Substring(i * bargs.stringsize, bargs.stringsize);
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          WriteReaderContent(filepath, strings);
        }, bench, filepath, "Substring Reader");
        filepath = Path.Combine(bargs.path, "streamreader-reader.txt");
        Benchmark((x) =>
        {
          string[] strings = new string[bargs.number];
          char[] buffer = new char[bargs.stringsize];
          using StreamReader reader = OpenReader(memory);
          int size;
          for (int i = 0; i < bargs.number; i++)
          {
            size = reader.Read(buffer, 0, bargs.stringsize);
            strings[i] = new string(buffer, 0, size);
          }
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          WriteReaderContent(filepath, strings);
        }, bench, filepath, "StreamReader");
        filepath = Path.Combine(bargs.path, "streamreader-readerspan.txt");
        Benchmark((x) =>
        {
          string[] strings = new string[bargs.number];
          Span<char> buffer = new Span<char>(new char[bargs.stringsize]);
          using StreamReader reader = OpenReader(memory);
          int size;
          for (int i = 0; i < bargs.number; i++)
          {
            size = reader.Read(buffer);
            strings[i] = new string(buffer);
          }
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          WriteReaderContent(filepath, strings);
        }, bench, filepath, "StreamReader Span");
        filepath = Path.Combine(bargs.path, "streamreader-readerasync.txt");
        Benchmark(async (x) =>
        {
          string[] strings = new string[bargs.number];
          char[] buffer = new char[bargs.stringsize];
          using StreamReader reader = OpenReader(memory);
          int size;
          for (int i = 0; i < bargs.number; i++)
          {
            size = await reader.ReadAsync(buffer, 0, bargs.stringsize);
            strings[i] = new string(buffer, 0, size);
          }
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          WriteReaderContent(filepath, strings);
        }, bench, filepath, "StreamReader ASYNC");
        filepath = Path.Combine(bargs.path, "streamreader-readermemory.txt");
        Benchmark(async (x) =>
        {
          string[] strings = new string[bargs.number];
          Memory<char> buffer = new Memory<char>(new char[bargs.stringsize]);
          using StreamReader reader = OpenReader(memory);
          int size;
          for (int i = 0; i < bargs.number; i++)
          {
            size = await reader.ReadAsync(buffer);
            strings[i] = new string(buffer.Span);
          }
          Out.WriteLine("String generated:\n" + new Benchmarker.Report(bench.Stopwatch, bench.Reports, bargs.number));
          WriteReaderContent(filepath, strings);
        }, bench, filepath, "StreamReader ASYNC Memory");
      }, null, cmd, "benchmark", "bm");
    }

    #region private

    private readonly struct BenchmarkArguments
    {
      internal BenchmarkArguments(IReadOnlyDictionary<string, IReadOnlyList<string>> x)
      {
        path = x["path"][0];
        stringsize = x.TryGetValue("stringsize", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 10;
        number = x.TryGetValue("number", out args) ? int.Parse(args[0]) : 1000000;
      }

      internal readonly string path;
      internal readonly int stringsize, number;

      internal int GetTotalSize() => stringsize * number;
    }

    private const string RANDOM_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int RANDOM_CHARS_LENGTH = 62;

    private static StreamReader OpenReader(MemoryStream memory)
    {
      memory.Position = 0;
      return new StreamReader(memory, Encoding.UTF8, false, -1, true);
    }

    private static string GenerateString(char[] chars, Random random, int size)
    {
      for (int i = 0; i < size; i++) chars[i] = RANDOM_CHARS[random.Next(RANDOM_CHARS_LENGTH)];
      return new string(chars);
    }

    private static void Benchmark(Action<Benchmarker> action, BenchmarkerDelegated bench, string path, string filename)
    {
      Out.WriteLine();
      Out.WriteLine($"Benchmarking for {filename}...");
      Out.WriteLine($"File path: {path}");
      bench.MainAction = action;
      bench.Benchmark();
    }

    private static void WriteToMemory(MemoryStream memory, byte[] buffer, string str) 
    { 
      int c = Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);
      memory.Write(buffer, 0, c);
    }

    private static void WriteReaderContent(string path, string[] strings)
    {
      using StreamWriter writer = new StreamWriter(path);
      for (int i = 0; i < strings.Length; i++) writer.WriteLine($"{i}: {strings[i]}");
      Out.WriteLine("Content outputted to test file.");
    }

    #endregion
  }
}
