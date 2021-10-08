
using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.Events;
using System;
using AzureServiceBusProject.Application.Results;
namespace AzureServiceBusProject.Application.Features.Commands.DeleteOrderForTopic
{
    public class DeleteOrderForTopicHandler : IRequestHandler<DeleteOrderForTopicRequest, IDataResult<DeleteOrderForTopicResponse>>
    {
        private readonly IServiceBusTopic serviceBusTopic;

        public DeleteOrderForTopicHandler(IServiceBusTopic serviceBusTopic)
        {
            this.serviceBusTopic = serviceBusTopic;
        }
        public async Task<IDataResult<DeleteOrderForTopicResponse>> Handle(DeleteOrderForTopicRequest request, CancellationToken cancellationToken)
        {
            var eventOrderDeletedModel = new OrderDeletedEvent
            {
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
            };

           await  this.serviceBusTopic.SendMessageToOrderTopicDeletedSubscriptionAsync(eventOrderDeletedModel);

            return new SuccessDataResult<DeleteOrderForTopicResponse>(new DeleteOrderForTopicResponse { Id = eventOrderDeletedModel.Id }, "İşlemi Başarılı !");
        }
    }
}
