using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Events;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.Interfaces.Results;
using AzureServiceBusProject.Application.Results;
using MediatR;
namespace AzureServiceBusProject.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, IDataResult<DeleteOrderResponse>>
    {
        private readonly IServiceBusQueue serviceBus;

        public DeleteOrderHandler(IServiceBusQueue serviceBus)
        {
            this.serviceBus = serviceBus;
        }
        public async Task<IDataResult<DeleteOrderResponse>> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            var eventOrderDeletedModel = new OrderDeletedEvent
            {
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            };

            await serviceBus.SendMessageToDeleteQueueAsync(eventOrderDeletedModel);

            return new SuccessDataResult<DeleteOrderResponse>(new DeleteOrderResponse { Id = eventOrderDeletedModel.Id }, "Ürün Silme Kuyruğuna Gönderme İşlemi Başarılı !");

        }
   
    
    }
}
