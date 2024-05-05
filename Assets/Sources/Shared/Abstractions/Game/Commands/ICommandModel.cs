﻿namespace Shared.Abstractions.Game.Commands
{
    
    public interface ICommandModel
    {
        string CommandId { get; }
        string PredictionId { get; set; }
    }
}