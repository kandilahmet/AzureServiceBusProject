using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrder
{
    public class CreateOrderRequest:IRequest<CreateOrderResponse>
    {
        public string CreatedName { get; set; }
    }
}
