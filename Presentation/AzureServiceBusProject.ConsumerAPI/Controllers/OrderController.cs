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
        private readonly IServiceBus serviceBus;
        public OrderController(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        [HttpPost("GetOrderDeletedFromQueue")]
        public  void  GetOrderDeletedFromQueue()
        {
             serviceBus.GetMessageFromDeleteQueue();
        }

        [HttpPost("GetOrderCreatedFromQueue")]
        public  void GetOrderCreatedFromQueue()
        {
              serviceBus.GetMessageFromCreateQueue();            
        }

    }
}
