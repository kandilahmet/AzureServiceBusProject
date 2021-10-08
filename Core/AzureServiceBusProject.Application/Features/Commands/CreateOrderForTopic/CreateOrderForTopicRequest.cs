using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Features.Commands.CreateOrderForTopic
{
    public class CreateOrderForTopicRequest:IRequest<IDataResult<CreateOrderForTopicResponse>>
    {
        public string ProductName { get; set; }
    }
}
