using AzureServiceBusProject.Application.Events;
using AzureServiceBusProject.Application.Interfaces;
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
        private readonly IServiceBus serviceBus;

        public CreateOrderHandler( IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }
        public async Task<CreateOrderResponse> Handle (CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var result= await Task.FromResult( new CreateOrderResponse
            {
                Id = Guid.NewGuid(),
                ProductName = $"{request.CreatedName}",
                Quantity = 250
            });

            var eventOrderCreatedModel = new OrderCreatedEvent
            {
                CreatedDate = DateTime.UtcNow,
                Id = result.Id,
                ProductName = result.ProductName
            };

            //await this.serviceBus.CreateQueueIfNotExits("OrderCreatedQueue");
            //await this.serviceBus.SendMessageToQueueAsync("OrderCreatedQueue", eventOrderCreatedModel);
            serviceBus.SendMessageToCreateQueueAsync(eventOrderCreatedModel);
            
            return result;
        }

 
    }
}
