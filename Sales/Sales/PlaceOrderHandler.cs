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
            log.Info($"Received PlaceOrder, OrderId = {message.Id}");

            var rand = (new Random()).Next(0, 10);

            log.Info($"Produced Rand = {rand}");


            if (rand > 2)
                throw new Exception("BOOM");


            var orderPlaced = new OrderPlaced
            {
                Id = message.Id
            };
            return context.Publish(orderPlaced);
        }
    }

}
