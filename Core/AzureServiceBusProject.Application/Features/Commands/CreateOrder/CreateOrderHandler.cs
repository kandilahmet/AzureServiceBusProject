using AzureServiceBusProject.Application.Events;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.Interfaces.Results;
using AzureServiceBusProject.Application.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrder
{
    public class CreateOrderHandler:IRequestHandler<CreateOrderRequest, IDataResult<CreateOrderResponse>>
    {
        private readonly IServiceBus serviceBus;
         
        public CreateOrderHandler( IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }
        public async Task<IDataResult<CreateOrderResponse>> Handle (CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var eventOrderCreatedModel = new OrderCreatedEvent
            {
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ProductName = request.ProductName
            };

            await serviceBus.SendMessageToCreateQueueAsync(eventOrderCreatedModel);

            return new SuccessDataResult<CreateOrderResponse>(new CreateOrderResponse { Id = eventOrderCreatedModel.Id }, "Ürün Ekleme Kuyruğuna Gönderme İşlemi Başarılı !");
                
        }

 
    }
}
