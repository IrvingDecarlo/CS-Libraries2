namespace Cephei.Objects.Effects.Stats
{
  internal interface IReferencedModifier<T, U> : IModifier<T, U>, IDeletable
  {
    Stat<T, U>? Target { get; set; }

    void CallDelete();
  }
}
