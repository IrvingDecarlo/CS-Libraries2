﻿namespace Cephei.Objects.Stats
{
  internal interface IReferencedModifier<T, U> : IModifier<T, U>, IReadOnlyDeletable
  {
    Stat<T, U>? Target { get; set; }

    void CallDelete();
  }
}
