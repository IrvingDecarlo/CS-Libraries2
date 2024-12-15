/*using Cephei.Collections;
using Cephei.Commands;
using Cephei.Commands.Consoles;
using Cephei.Objects;
using Cephei.Objects.Effects;
using Cephei.Objects.Stats;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Cephei.Testing
{
  using static ConsoleSystem;

  internal static class StatTest
  {
    static StatTest()
    {
      stats = new Dictionary<string, Stat>();
      effects = new Dictionary<string, Effect<string>>();
      modifiers = new Dictionary<string, Modifier>();
      StatCommand = CreateCommand(null, null, null, "stat");
      ModifierCommand = CreateCommand(null, null, null, "modifier", "mod");
      EffectCommand = CreateCommand(null, null, null, "effect", "eff");
    }
    internal static void CreateCommands()
    {
      CreateCommand((x) => ListItems(stats.Values, "Listing stats:"), null, StatCommand, "list");
      CreateCommand((x) =>
      {
        Stat stat = new Stat(x["id"][0], 0);
        SetObjectValues(stat, x);
        stats.Add(stat);
        Out.WriteLine("Stat created: " + stat);
      }, null, StatCommand, "new");
      CreateCommand((x) =>
      {
        string id = x["id"][0];
        stats[id].Delete();
        stats.Remove(id);
        Out.WriteLine("Stat deleted: ID " + id);
      }, null, StatCommand, "delete", "del");
      CreateCommand((x) => SetObjectValuesCommand(stats[x["id"][0]], x), null, StatCommand, "set");
      CreateCommand((x) =>
      {
        if (x.TryGetValue("id", out IReadOnlyList<string> args))
        {
          Stat stat = stats[args[0]];
          stat.Update();
          Out.WriteLine($"Stat {stat} updated.");
          return;
        }
        IEnumerable<Stat> stts = stats.Values;
        stts.UpdateAll();
        ListItems(stts, "Stats updated:");
      }, null, StatCommand, "update", "upd");
      Command source = CreateCommand((x) => ListItems(stats[x["id"][0]].Sources.Values, "Listing sources for stat:"), null, StatCommand, "sources", "src");
      CreateCommand((x) =>
      {
        Stat stt = GetStatAndModifier(x, out Modifier mod);
        stt.AddSource(mod);
        Out.WriteLine($"Source added to {stt}:\n{mod}");
      }, null, source, "add");
      CreateCommand((x) =>
      {
        Stat stt = GetStatAndModifier(x, out Modifier mod);
        stt.RemoveSource(mod);
        Out.WriteLine($"Source removed from {stt}:\n{mod}");
      }, null, source, "remove", "rem");
      Command target = CreateCommand((x) => ListItems(stats[x["id"][0]].Targets.Values, "Listing targets for stat:"), null, StatCommand, "targets");
      CreateCommand((x) =>
      {
        Stat stt = GetStatAndModifier(x, out Modifier mod);
        stt.AddTarget(mod);
        Out.WriteLine($"Target added to {stt}:\n{mod}");
      }, null, target, "add");

      CreateCommand((x) => ListItems(modifiers.Values, "Listing modifiers:"), null, ModifierCommand, "list");
      CreateCommand((x) =>
      {
        Modifier mod = new Modifier(x["id"][0]);
        SetObjectValues(mod, x);
        modifiers.Add(mod);
        Out.WriteLine("Modifier created: " + mod);
      }, null, ModifierCommand, "new");
      CreateCommand((x) =>
      {
        string id = x["id"][0];
        modifiers[id].Delete();
        modifiers.Remove(id);
        Out.WriteLine("Modifier deleted: ID " + id);
      }, null, ModifierCommand, "delete", "del");
      CreateCommand((x) => SetObjectValuesCommand(modifiers[x["id"][0]], x), null, ModifierCommand, "set");

      CreateCommand((x) =>
      {
        if (x.TryGetValue("id", out IReadOnlyList<string> args))
        {
          Effect<string> eff = effects[args[0]];
          ListItems(eff.Values, "Listing objects under effect " + eff);
          return;
        }
        ListItems(effects.Values, "Listing effects:");
      }, null, EffectCommand, "list");
      CreateCommand((x) =>
      {
        Effect<string> eff = new Effect(x["id"][0]);
        SetObjectValues(eff, x);
        effects.Add(eff);
        Out.WriteLine("Effect created: " + eff);
      }, null, EffectCommand, "new");
      CreateCommand((x) =>
      {
        string id = x["id"][0];
        effects[id].Delete();
        effects.Remove(id);
        Out.WriteLine("Effect deleted: ID " + id);
      }, null, EffectCommand, "delete", "del");
      CreateCommand((x) => SetObjectValuesCommand(effects[x["id"][0]], x), null, EffectCommand, "set");
    }

    internal readonly static Command StatCommand;
    internal readonly static Command ModifierCommand;
    internal readonly static Command EffectCommand;

    private static Stat GetStatAndModifier(IReadOnlyDictionary<string, IReadOnlyList<string>> x, out Modifier mod)
    {
      mod = modifiers[x["mod"][0]];
      return stats[x["id"][0]];
    }

    private static void SetObjectValues(EffectObject<string> obj, IReadOnlyDictionary<string, IReadOnlyList<string>> x)
    {
      TextWriter writer = Out;
      if (x.TryGetValue("eff", out IReadOnlyList<string> args))
      {
        obj.Effect = effects[args[0]];
        writer.WriteLine("Object effect set to " + obj.Effect);
      }
      if (obj is Stat stat && x.TryGetValue("value", out args))
      {
        stat.BaseValue = float.Parse(args[0]);
        writer.WriteLine("Stat value set to " + stat.BaseValue);
      }
    }

    private static void SetObjectValuesCommand(EffectObject<string> obj, IReadOnlyDictionary<string, IReadOnlyList<string>> x)
    {
      Out.WriteLine("Setting data for object " + obj);
      SetObjectValues(obj, x);
    }

    private static void ListItems(IEnumerable en, string msg)
    {
      Out.WriteLine(msg);
      Out.WriteLine(en.Join("{", "}\n{", "}"));
    }

    private readonly static Dictionary<string, Stat> stats;
    private readonly static Dictionary<string, Effect<string>> effects;
    private readonly static Dictionary<string, Modifier> modifiers;

    private class Stat : Stat<string, float>
    {
      internal Stat(string id, float value = 0, Effect<string> eff = null)
        : base(id, value, eff)
      { }

      protected override bool BypassDeletable() => false;

      protected override float Calculate(float modvalue) => Value + modvalue;

      public override int CompareTo(Stat<string, float> stat) => ID.CompareTo(stat.ID);

      public override bool Modifiable => true;

      public override bool Deletable => true;
    }

    private class Modifier : Modifier<string, float>
    {
      internal Modifier(string id, Effect<string> eff = null)
        : base(id, 0, eff)
      { }

      protected override bool BypassDeletable() => false;

      protected override float Calculate() => Source is null ? 0 : Source.Value;

      public override bool Modifiable => true;

      public override bool Deletable => true;
    }

    private class Effect : Effect<string>
    {
      internal Effect(string id, Effect<string> eff = null) : base(id, eff)
      { }

      protected override bool BypassDeletable() => false;

      public override bool Modifiable => true;

      public override bool Deletable => true;
    }
  }
}
    */