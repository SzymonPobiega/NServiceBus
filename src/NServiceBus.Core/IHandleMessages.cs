namespace NServiceBus
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a message handler.
    /// </summary>
    /// <typeparam name="T">The type of message to be handled.</typeparam>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IHandleMessages<T>
    {
        /// <summary>
        /// Handles a message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="context">The context of the currently handled message.</param>
        /// <remarks>
        /// This method will be called when a message arrives on at the endpoint and should contain
        /// the custom logic to execute when the message is received.</remarks>
        Task Handle(T message, IMessageHandlerContext context);
    }
}