namespace NServiceBus
{
    /// <summary>
    /// Configuration extensions for routing.
    /// </summary>
    public static class RoutingSettingsExtensions
    {
        /// <summary>
        /// Gets the routing table for the direct routing.
        /// </summary>
        public static UnicastRoutingSettings UnicastRouting(this EndpointConfiguration config)
        {
            Guard.AgainstNull(nameof(config), config);
            return new UnicastRoutingSettings(config.Settings);
        }
    }
}