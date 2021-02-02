using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received Place Order Command");
            log.Info($"Received PlaceOrder, OrderId = {message.Id}");
            return Task.CompletedTask;
        }
    }

}
