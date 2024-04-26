﻿namespace Demo.Core.Abstractions.Game.Runtime.Data
{
    public interface IRuntimeEffectData : IRuntimeDataBase
    {
        int EffectOwnerId { get; set; }
        int Value { get; set; }
        int Lifetime { get; set; }
    }
}