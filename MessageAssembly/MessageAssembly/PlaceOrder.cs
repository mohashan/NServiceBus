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
}
