using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;
namespace AzureServiceBusProject.Application.Features.Commands.DeleteOrderForTopic
{
    public class DeleteOrderForTopicRequest:IRequest<IDataResult<DeleteOrderForTopicResponse>>
    {
    }
}
