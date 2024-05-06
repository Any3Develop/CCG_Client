﻿using Shared.Abstractions.Game.Runtime.Data;

namespace Shared.Game.Events.Output
{
    public class ChangedObjectStat : GameEvent
    {
        public IRuntimeStatData RuntimeData { get; set; }
    }
}