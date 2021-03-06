﻿namespace NServiceBus.Features
{
    using NServiceBus.Pipeline;

    class OutgoingPipelineFeature : Feature
    {
        public OutgoingPipelineFeature()
        {
            EnableByDefault();
        }

        protected internal override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(WellKnownStep.MutateOutgoingMessages, typeof(MutateOutgoingMessageBehavior), "Executes IMutateOutgoingMessages");
            context.Pipeline.Register(WellKnownStep.MutateOutgoingTransportMessage, typeof(MutateOutgoingTransportMessageBehavior), "Executes IMutateOutgoingTransportMessages");

            context.Pipeline.Register("ForceImmediateDispatchForOperationsInSuppressedScopeBehavior", new ForceImmediateDispatchForOperationsInSuppressedScopeBehavior(), "Detects operations performed in a suppressed scope and request them to be immediately dispatched to the transport.");

            context.Pipeline.Register("OutgoingPhysicalToRoutingConnector", new OutgoingPhysicalToRoutingConnector(), "Starts the message dispatch pipeline");
            context.Pipeline.Register("RoutingToDispatchConnector", new RoutingToDispatchConnector(), "Decides if the current message should be batched or immediately be dispatched to the transport");
            context.Pipeline.Register("BatchToDispatchConnector", new BatchToDispatchConnector(), "Passes batched messages over to the immediate dispatch part of the pipeline");
            context.Pipeline.Register("ImmediateDispatchTerminator", typeof(ImmediateDispatchTerminator), "Hands the outgoing messages over to the transport for immediate delivery");
        }
    }
}