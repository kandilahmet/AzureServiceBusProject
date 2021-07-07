using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrder
{
    public class CreateOrderResponse
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
