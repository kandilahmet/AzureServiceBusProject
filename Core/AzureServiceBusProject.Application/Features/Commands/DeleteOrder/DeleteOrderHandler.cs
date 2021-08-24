using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Events;
using AzureServiceBusProject.Application.Interfaces;
using MediatR;
namespace AzureServiceBusProject.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, DeleteOrderResponse>
    {
        private readonly IServiceBus serviceBus;

        public DeleteOrderHandler(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }
        public async Task<DeleteOrderResponse> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            var result = await Task.FromResult(new DeleteOrderResponse
            {
                Id = Guid.NewGuid()
            }); 

            var eventOrderDeletedModel =   new OrderDeletedEvent
            {
                CreatedDate=DateTime.UtcNow,
                Id=result.Id
            };

            //await this.serviceBus.CreateQueueIfNotExits("OrderDeletedQueue");
            //await this.serviceBus.SendMessageToQueueAsync("OrderDeletedQueue", eventOrderDeletedModel);
            this.serviceBus.SendMessageToDeleteQueueAsync(eventOrderDeletedModel);

            return result;
        }
    }
}
