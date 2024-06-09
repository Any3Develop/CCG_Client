﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Shared.Game.Context.EventSource;

namespace Client.Common.Services.EventSourceService.Realizations
{
    public class UnitaskBasedEventSource : EventSource, IEventSource
    {
        public new IDisposable Subscribe<T>(Action<T> callback, CancellationToken? token = null, int? order = null) 
            => base.Subscribe(callback, token, order);

        public new IDisposable Subscribe<T>(Action callback, CancellationToken? token = null, int? order = null) 
            => base.Subscribe<T>(callback, token, order);

        public IDisposable Subscribe<T>(Func<UniTask> callback, CancellationToken? token = null, int? order = null) 
            => InternalSubscribe<T>(callback, false, token, order);

        public IDisposable Subscribe<T>(Func<T, UniTask> callback, CancellationToken? token = null, int? order = null) 
            => InternalSubscribe<T>(callback, true, token, order);

        public new async UniTask PublishAsync<T>(T value)
        {
            foreach (var subscriber in GetSubscribers<T>())
            {
                switch (DynamicInvoke(subscriber, value))
                {
                    case UniTask uniTask : await uniTask; return;
                    case Task task : await task.AsUniTask(); return;
                }
            }
        }
    }
}