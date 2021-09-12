using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Application.ViewModels;

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

        [HttpGet("GetOrderDeletedFromQueue")]
        public GetMessageFromDeleteQueueViewModel GetOrderDeletedFromQueue()
        {
            var result = serviceBus.GetMessageFromDeleteQueueAsync<GetMessageFromDeleteQueueViewModel>().Result;
            return result;
        }

        [HttpGet("GetOrderCreatedFromQueue")]
        public GetMessageFromCreateQueueViewModel GetOrderCreatedFromQueue()
        {
            var result = serviceBus.GetMessageFromCreateQueueAsync<GetMessageFromCreateQueueViewModel>().Result;
            return result;
        }

        [HttpGet("GetStartMessageFromDeleteQueue")]
        public void GetStartMessageFromDeleteQueue()
        {
            serviceBus.GetStartMessageFromDeleteQueue<GetMessageFromDeleteQueueViewModel>();
            //serviceBus.GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>(); 
        }
        [HttpGet("GetStartMessageFromCreateQueue")]
        public void GetStartMessageFromCreateQueue()
        {
            serviceBus.GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>();
        }
    }
}
