namespace NServiceBus
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus.Transports;
    using Pipeline;

    class AttachCausationHeadersBehavior : Behavior<IOutgoingPhysicalMessageContext>
    {
        public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
        {
            ApplyHeaders(context);

            return next();
        }

        void ApplyHeaders(IOutgoingPhysicalMessageContext context)
        {
            var conversationId = CombGuid.Generate().ToString();

            IncomingMessage incomingMessage;

            if (context.TryGetIncomingPhysicalMessage(out incomingMessage))
            {
                context.Headers[Headers.RelatedTo] = incomingMessage.MessageId;

                string conversationIdFromCurrentMessageContext;
                if (incomingMessage.Headers.TryGetValue(Headers.ConversationId, out conversationIdFromCurrentMessageContext))
                {
                    conversationId = conversationIdFromCurrentMessageContext;
                }
            }

            context.Headers[Headers.ConversationId] = conversationId;
        }
    }
}