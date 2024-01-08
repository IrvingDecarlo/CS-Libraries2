using Cephei.Commands.Consoles;
using Cephei.Commands;
using System.Collections.Generic;
using System.IO;

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
      }, null, Main, "test");
    }

    public static Command Main;

    #region private

    #endregion
  }
}

