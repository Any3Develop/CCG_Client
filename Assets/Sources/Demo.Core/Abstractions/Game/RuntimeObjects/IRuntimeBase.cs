﻿using System;
using Demo.Core.Abstractions.Game.Data;
using Demo.Core.Abstractions.Game.RuntimeData;

namespace Demo.Core.Abstractions.Game.RuntimeObjects
{
    public interface IRuntimeBase : IDisposable
    {
        IRuntimeData RuntimeData { get; }
        IData Data { get; }
    }
}