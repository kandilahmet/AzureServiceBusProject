using AzureServiceBusProject.Application.Events;
using AzureServiceBusProject.Application.Features.Commands.CreateOrder;
using AzureServiceBusProject.Application.Features.Commands.DeleteOrder;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("CreateOrder")] 
        public async Task<IDataResult<CreateOrderResponse>> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            var response = await this.mediator.Send(createOrderRequest);

            return response;
        }

        [HttpPost("DeleteOrder")]
        public async Task<IDataResult<DeleteOrderResponse>> DeleteOrder([FromBody] DeleteOrderRequest deleteOrderRequeset)
        {

            var result = await this.mediator.Send(deleteOrderRequeset);
            return result;
        }
    }
}
