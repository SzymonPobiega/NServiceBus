﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus.Unicast.Subscriptions.DB.Config;

namespace NServiceBus
{
    /// <summary>
    /// Contains extension methods to NServiceBus.Configure.
    /// </summary>
    public static class ConfigureDbSubscriptionStorage
    {
        public static ConfigDbSubscriptionStorage DbSubscriptionStorage(this Configure config)
        {
            ConfigDbSubscriptionStorage cfg = new ConfigDbSubscriptionStorage();
            cfg.Configure(config);

            return cfg;
        }
    }
}
