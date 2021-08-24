using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;

namespace AzureServiceBusProject.ConsumerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServiceBus ServiceBus;
        public OrderController(IServiceBus serviceBus)
        {
            ServiceBus = serviceBus;
        }

        [HttpPost("GetOrderDeletedFromQueue")]
        public  void  GetOrderDeletedFromQueue()
        {
             ServiceBus.GetMessageFromDeleteQueue();
        }

        [HttpPost("GetOrderCreatedFromQueue")]
        public  void GetOrderCreatedFromQueue()
        {
              ServiceBus.GetMessageFromCreateQueue();            
        }

    }
}
