using Cephei.Commands.Consoles;
using Cephei.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cephei.Benchmarking;
using Cephei.Streams;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class StreamTest
  {
    public static void AddCommands()
    {
      Main = CreateCommand(null, null, null, "stream");
      CreateCommand((x) =>
      {
        int size = x.TryGetValue("size", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 512;
        Out.WriteLine("Byte array created with size " + size);
        byte[] bytes = new byte[size];
        int i, cint, cfloat, cstring;
        MemoryStream stream;
        using (stream = new MemoryStream(bytes, true))
        {
          using BinaryWriter writer = new BinaryWriter(stream);
          if (x.TryGetValue("int", out args))
          {
            cint = args.Count;
            for (i = 0; i < cint; i++) writer.Write(int.Parse(args[i]));
          }
          else cint = 0;
          if (x.TryGetValue("float", out args))
          {
            cfloat = args.Count;
            for (i = 0; i < cfloat; i++) writer.Write(float.Parse(args[i]));
          }
          else cfloat = 0;
          if (x.TryGetValue("string", out args))
          {
            cstring = args.Count;
            for (i = 0; i < cstring; i++) writer.Write(args[i]);
          }
          else cstring = 0;
          Out.WriteLine($"Bytes written: {stream.Position}/{size}");
        }
        Out.WriteLine("Content written to byte array. Creating new stream to read...");
        using (stream = new MemoryStream(bytes, false))
        {
          using BinaryReader reader = new BinaryReader(stream);
          for (i = 0; i < cint; i++) Out.WriteLine($"Int {i}: " + reader.ReadInt32());
          for (i = 0; i < cfloat; i++) Out.WriteLine($"Float {i}: " + reader.ReadSingle());
          for (i = 0; i < cstring; i++) Out.WriteLine($"String {i}: " + reader.ReadString());
        }
        Out.WriteLine("All content was read.");
      }, null, Main, "binary");

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

      CreateCommand((x) =>
      {
        string path = x["path"][0];
        string pathdef = Path.Combine(path, "7bit_base.txt");
        string path32 = Path.Combine(path, "7bit_32.txt");
        string path7 = Path.Combine(path, "7bit_7.txt");
        Random random = new Random();
        int n = x.TryGetValue("n", out IReadOnlyList<string> args) ? int.Parse(args[0]) : 1000000;
        Out.WriteLine($"Testing 7Bit encoder performance with {n} numbers...");
        int i;
        int[] values;
        using (StreamWriter swriter = new StreamWriter(pathdef))
        {
          i = 0;
          values = new int[n];
          for (i = 0; i < n; i++)
          {
            values[i] = random.Next(int.MinValue, int.MaxValue);
            swriter.WriteLine(values[i]);
          }
        }
        Out.WriteLine("Base successfully written to " + pathdef);
        Out.WriteLine();
        BenchmarkerDelegated bench = new BenchmarkerDelegated(null);
        Benchmark7Bit((x, y) => x.Write(values[y]), bench, n, path32, "Default 32Bit Writer");
        Benchmark7Bit((x, y) => x.Write7Bit(values[y]), bench, n, path7, "7Bit Writer");
        Out.WriteLine("Reading and writing auxiliary files...");
        Benchmark7BitReader((x) => x.ReadInt32(), bench, n, path32, "Default 32Bit Reader", Path.Combine(path, "7bit_32_out.txt"));
        Benchmark7BitReader((x) => (int)x.Read7BitInt64(), bench, n, path32, "7Bit Reader", Path.Combine(path, "7bit_7_out.txt"));
      }, null, Main, "7bit");
    }

    public static Command Main;

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

    private static void Benchmark7Bit(Action<BinaryWriter, int> action, BenchmarkerDelegated bench, int n, string path, string name)
    {
      Out.WriteLine();
      using BinaryWriter bwriter = new BinaryWriter(File.OpenWrite(path));
      Benchmark((x) =>
      {
        for (int i = 0; i < n; i++) action(bwriter, i);
      }, bench, path, name);
      Out.WriteLine(new Benchmarker.Report(bench.Stopwatch, bench.Reports, n));
    }

    private static void Benchmark7BitReader(Func<BinaryReader, int> action, BenchmarkerDelegated bench, int n, string path, string name, string outpath)
    {
      Out.WriteLine();
      using BinaryReader reader = new BinaryReader(File.OpenRead(path));
      int[] values = new int[n];
      int i;
      Benchmark((x) =>
      {
        for (i = 0; i < n; i++) values[i] = action(reader);
      }, bench, path, name);
      Out.WriteLine(new Benchmarker.Report(bench.Stopwatch, bench.Reports, n));
      using StreamWriter writer = new StreamWriter(outpath);
      for (i = 0; i < n; i++) writer.WriteLine(values[i]);
      Out.WriteLine("Control data outputted to " + outpath);
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

