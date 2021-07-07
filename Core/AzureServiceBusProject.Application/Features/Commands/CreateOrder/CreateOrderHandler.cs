using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrder
{
    public class CreateOrderHandler:IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    { 
        public  Task<CreateOrderResponse> Handle (CreateOrderRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult( new CreateOrderResponse
            {
                Id = new Guid(),
                ProductName = $"{request.CreatedName} Vantilatör siparişi verdir",
                Quantity = 250
            });
        }

 
    }
}
