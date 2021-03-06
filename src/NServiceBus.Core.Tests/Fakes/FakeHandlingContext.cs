﻿namespace NServiceBus.Core.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus.DelayedDelivery;
    using NServiceBus.DeliveryConstraints;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;

    public class FakeHandlingContext : IMessageHandlerContext
    {
        int deferWasCalled;

        public string MessageId { get; }
        public string ReplyToAddress { get; }
        public IReadOnlyDictionary<string, string> MessageHeaders { get; }
        public ContextBag Extensions { get; }

        public Task Send(object message, SendOptions options)
        {
            DelayDeliveryWith constraint;
            if (options.GetExtensions().TryGetDeliveryConstraint(out constraint))
            {
                Interlocked.Increment(ref deferWasCalled);
                DeferDelay = constraint.Delay;
                DeferedMessage = message;
            }

            return TaskEx.CompletedTask;
        }

        public Task Send<T>(Action<T> messageConstructor, SendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Publish(object message, PublishOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Publish<T>(Action<T> messageConstructor, PublishOptions publishOptions)
        {
            throw new NotImplementedException();
        }

        public Task Subscribe(Type eventType, SubscribeOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Unsubscribe(Type eventType, UnsubscribeOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Reply(object message, ReplyOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Reply<T>(Action<T> messageConstructor, ReplyOptions options)
        {
            throw new NotImplementedException();
        }

        public Task HandleCurrentMessageLater()
        {
            throw new NotImplementedException();
        }

        public Task ForwardCurrentMessageTo(string destination)
        {
            throw new NotImplementedException();
        }

        public void DoNotContinueDispatchingCurrentMessageToHandlers()
        {
            throw new NotImplementedException();
        }

        public SynchronizedStorageSession SynchronizedStorageSession => null;

        public int DeferWasCalled
        {
            get { return deferWasCalled; }
            set { deferWasCalled = value; }
        }

        public TimeSpan DeferDelay { get; private set; } = TimeSpan.MinValue;

        public object DeferedMessage { get; set; }

        
    }
}