﻿using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.StorageDirectory(@"C:\.learningtransport");

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
