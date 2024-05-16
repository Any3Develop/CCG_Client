﻿using Client.Game.Abstractions.Collections.Queues;
using Client.Game.Abstractions.Context.EventProcessors;
using Client.Lobby.Runtime;
using Cysharp.Threading.Tasks;
using Shared.Abstractions.Game.Commands;
using Shared.Abstractions.Game.Context;
using Shared.Common.Logger;
using Shared.Game.Events.Context.Queue;

namespace Client.Game.Context.EventProcessors
{
    public class GameEventQueuePredictionProcessor : IGameEventQueuePredictionProcessor
    {
        private readonly IContext context;
        private readonly IGameEventPredictedQueue predictedQueue;
        private readonly IGameEventQueueLocalProcessor queueLocalProcessor;
        private readonly ICommandProcessor commandProcessor;

        public GameEventQueuePredictionProcessor(
            IContext context,
            IGameEventPredictedQueue predictedQueue,
            IGameEventQueueLocalProcessor queueLocalProcessor,
            ICommandProcessor commandProcessor)
        {
            this.context = context;
            this.predictedQueue = predictedQueue;
            this.queueLocalProcessor = queueLocalProcessor;
            this.commandProcessor = commandProcessor;
        }

        public void Execute<TCommand>(ICommandModel model) where TCommand : ICommand
        {
            using var _ = context.EventSource.Subscribe<AfterGameQueueReleasedEvent>(data => predictedQueue.Enqueue(data.Queue));
            commandProcessor.Execute<TCommand>(User.Id, model);
            queueLocalProcessor.ProcessAsync(predictedQueue).Forget(SharedLogger.Error);
        }
    }
}