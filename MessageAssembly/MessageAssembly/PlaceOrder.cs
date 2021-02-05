using NServiceBus;
using System;

namespace Messages
{
    public class PlaceOrder:ICommand
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public string Requester { get; set; }
    }

    public class ShipOrder : ICommand
    {
        public string Id { get; set; }
    }

    public class OrderPlaced : IEvent
    {
        public string Id { get; set; }
    }

    public class OrderBilled : IEvent
    {
        public string Id { get; set; }
    }

    public class CancelOrder
    : ICommand
    {
        public string Id { get; set; }
    }
}
