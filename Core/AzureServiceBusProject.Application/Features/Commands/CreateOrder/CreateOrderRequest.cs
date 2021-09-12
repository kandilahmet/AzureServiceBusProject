using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrder
{
    public class CreateOrderRequest:IRequest<IDataResult<CreateOrderResponse>>
    {
        public string ProductName { get; set; }
    }
}
