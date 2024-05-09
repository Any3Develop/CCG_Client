﻿using System;
using System.Collections.Generic;
using System.Threading;
using Client.Game.Abstractions.Collections.Queues;
using Client.Game.Abstractions.Context.EventSource;
using Client.Game.Abstractions.Context.Queue;
using Cysharp.Threading.Tasks;
using Shared.Abstractions.Game.Events;
using Shared.Common.Logger;

namespace Client.Game.Context.Queue
{
    public class GameEventQueueProcessor : IGameEventQueueProcessor, IDisposable
    {
        private readonly IGameEventLocalQueue localQueue;
        private readonly IGameEventPublisher gameEventPublisher;
        private CancellationTokenSource unQueueProcess;
        private UniTaskCompletionSource interrupting;
        private bool initialized = true;

        public GameEventQueueProcessor(
            IGameEventLocalQueue localQueue,
            IGameEventPublisher gameEventPublisher)
        {
            this.localQueue = localQueue;
            this.gameEventPublisher = gameEventPublisher;
        }

        public void Dispose()
        {
            if (!initialized)
                return;
            
            initialized = false;
            interrupting?.TrySetResult();
            unQueueProcess?.Cancel();
            unQueueProcess?.Dispose();
            unQueueProcess = null;
            interrupting = null;
            localQueue.Clear();
        }

        public void Register(IEnumerable<IGameEvent> queue)
        {
            if (!initialized)
                return;
            
            localQueue.Enqueue(queue);
            StartProcess();
        }

        public void StartProcess()
        {
            StartUnQueueLoopAsync().Forget(SharedLogger.Error);
        }

        public async UniTask ProcessAsync(IGameEvent gameEvent)
        {
            if (!initialized)
                return;
            
            await gameEventPublisher.PublishAsync(gameEvent);
        }

        public async UniTask InterruptAsync()
        {
            if (interrupting != null)
            {
                await interrupting.Task; // TODO: check if it possible to await twice
                return;
            }

            interrupting = new UniTaskCompletionSource();
            unQueueProcess?.Cancel();
            unQueueProcess?.Dispose();
            unQueueProcess = null;
            await interrupting.Task;
            interrupting = null;
        }

        private async UniTask StartUnQueueLoopAsync()
        {
            if (!initialized || localQueue.Count == 0 || unQueueProcess != null || interrupting != null)
                return;

            unQueueProcess = new CancellationTokenSource();
            var token = unQueueProcess.Token;
            while (initialized && !token.IsCancellationRequested && localQueue.Count > 0)
                await ProcessAsync(localQueue.Dequeue());

            unQueueProcess?.Dispose();
            unQueueProcess = null;
            if (interrupting != null)
            {
                interrupting.TrySetResult();
                interrupting = null;
                return;
            }

            StartProcess();
        }
    }
}