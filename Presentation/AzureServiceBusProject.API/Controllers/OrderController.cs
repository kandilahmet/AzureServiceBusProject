using AzureServiceBusProject.Application.Features.Commands.CreateOrder;
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
        readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }
 
        [HttpPost]
        public async Task<CreateOrderResponse>  CreateOrder ([FromBody]CreateOrderRequest createOrderRequest)
        {
          var response=  await this.mediator.Send(createOrderRequest);
            return   response;
        }
    }
}
