using Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales
{
    class BuyersRemorsePolicy : Saga<BuyersRemorseState>,
        IAmStartedByMessages<PlaceOrder>,
        IHandleTimeouts<BuyersRemorseIsOver>,
        IHandleMessages<CancelOrder>
    {
        static ILog log = LogManager.GetLogger<BuyersRemorsePolicy>();

        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received PlaceOrder, OrderId = {message.Id}");

            Data.OrderId = message.Id;

            log.Info($"Starting cool down period for order #{Data.OrderId}.");

            await RequestTimeout(context, TimeSpan.FromSeconds(20), new BuyersRemorseIsOver());

            //return Task.CompletedTask;
        }

        public Task Handle(CancelOrder message, IMessageHandlerContext context)
        {
            log.Info($"Order #{message.Id} was cancelled.");

            //TODO: Possibly publish an OrderCancelled event?

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Timeout(BuyersRemorseIsOver timeout, IMessageHandlerContext context)
        {
            log.Info($"Cooling down period for order #{Data.OrderId} has elapsed.");

            var orderPlaced = new OrderPlaced
            {
                Id = Data.OrderId
            };

            await context.Publish(orderPlaced);

            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<BuyersRemorseState> mapper)
        {
            mapper.ConfigureMapping<PlaceOrder>(message => message.Id).ToSaga(saga => saga.OrderId);
            mapper.ConfigureMapping<CancelOrder>(message => message.Id).ToSaga(saga => saga.OrderId);
        }
    }

    class BuyersRemorseIsOver
    {
    }

    public class BuyersRemorseState : ContainSagaData
    {
        public string OrderId { get; set; }
    }
}
