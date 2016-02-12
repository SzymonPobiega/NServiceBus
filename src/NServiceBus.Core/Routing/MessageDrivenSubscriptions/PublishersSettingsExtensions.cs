namespace NServiceBus
{
    using NServiceBus.Routing.MessageDrivenSubscriptions;

    /// <summary>
    /// Allows to configure publishers.
    /// </summary>
    public static class PublishersSettingsExtensions
    {
        /// <summary>
        /// Gets the publisher settings.
        /// </summary>
        public static Publishers Publishers(this UnicastRoutingSettings config)
        {
            Guard.AgainstNull(nameof(config), config);
            Publishers publishers;
            if (!config.Settings.TryGet(out publishers))
            {
                publishers = new Publishers();
                config.Settings.Set<Publishers>(publishers);
            }
            return publishers;
        }
    }
}