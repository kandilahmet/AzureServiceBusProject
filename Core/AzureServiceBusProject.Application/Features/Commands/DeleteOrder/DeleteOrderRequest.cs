using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;

namespace AzureServiceBusProject.Application.Features.Commands.DeleteOrder
{
    public class DeleteOrderRequest:IRequest<IDataResult<DeleteOrderResponse>>
    {
    }
}
