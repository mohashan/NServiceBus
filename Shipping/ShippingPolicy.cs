using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping
{
    public class ShippingPolicy : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderPlaced>,
    IAmStartedByMessages<OrderBilled>
    {
        static ILog log = LogManager.GetLogger<ShippingPolicy>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.Id}");
            Data.IsOrderPlaced = true;
            return ProcessOrder(context);
        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderBilled, OrderId = {message.Id}");
            Data.IsOrderBilled = true;
            return ProcessOrder(context);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(message => message.Id).ToSaga(message => message.OrderId);
            mapper.ConfigureMapping<OrderBilled>(message => message.Id).ToSaga(message => message.OrderId);
        }

        private async Task ProcessOrder(IMessageHandlerContext context)
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await context.SendLocal(new ShipOrder() { Id = Data.OrderId });
                MarkAsComplete();
            }
        }
    }
    public class ShippingPolicyData : ContainSagaData
    {
        public string OrderId { get; set; }
        public bool IsOrderPlaced { get; set; }
        public bool IsOrderBilled { get; set; }
    }

}
