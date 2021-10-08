using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.Events;
using System;
using AzureServiceBusProject.Application.Features.Commands.CreateOrder;
using AzureServiceBusProject.Application.Results;
using AzureServiceBusProject.Application.Interfaces.Results;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrderForTopic
{
     
    public class CreateOrderForTopicHandler : IRequestHandler<CreateOrderForTopicRequest, IDataResult<CreateOrderForTopicResponse>>
    {
        public readonly IServiceBusTopic serviceBusTopic;

        public CreateOrderForTopicHandler(IServiceBusTopic serviceBusTopic)
        {
            this.serviceBusTopic = serviceBusTopic;
        }

        public async Task<IDataResult<CreateOrderForTopicResponse>> Handle(CreateOrderForTopicRequest request, CancellationToken cancellationToken)
        {
           

                var eventOrderCreatedModel = new OrderCreatedEvent
                {
                    CreatedDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    ProductName = request.ProductName
                };

            await serviceBusTopic.SendMessageToOrderTopicCreatedSubscriptionAsync(eventOrderCreatedModel);

            return new SuccessDataResult<CreateOrderForTopicResponse>(new CreateOrderForTopicResponse { Id = eventOrderCreatedModel.Id }, "Ürün Ekleme Kuyruğuna Gönderme İşlemi Başarılı !");

        }
    }
}
