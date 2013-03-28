namespace NServiceBus.Hosting.Roles.Handlers
{
    using System.Linq;
    using Hosting.Roles;
    using Transports;
    using Unicast.Config;


    /// <summary>
    /// Configuring the right transport based on  UsingTransport<T> role on the endpoint config
    /// </summary>
    public class TransportRoleHandler : IConfigureRole<UsingTransport<ITransportDefinition>>
    {
        public ConfigUnicastBus ConfigureRole(IConfigureThisEndpoint specifier)
        {
            var transportDefinitionType =
                specifier.GetType()
                         .GetInterfaces()
                         .SelectMany(i => i.GetGenericArguments())
                         .Single(t => typeof (ITransportDefinition).IsAssignableFrom(t));

            Configure.Instance.UseTransport(transportDefinitionType);

            return null;
        }
    }
}