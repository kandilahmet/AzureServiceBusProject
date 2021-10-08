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
        private readonly IServiceBusQueue serviceBusQueue;
        private readonly IServiceBusTopic  serviceBusTopic;
        public OrderController(IServiceBusQueue serviceBus, IServiceBusTopic serviceBusTopic)
        {
            this.serviceBusQueue = serviceBus;
            this.serviceBusTopic = serviceBusTopic;
        }

        [HttpGet("GetOrderDeletedFromQueue")]
        public GetMessageFromDeleteQueueViewModel GetOrderDeletedFromQueue()
        {
            var result = serviceBusQueue.GetMessageFromDeleteQueueAsync<GetMessageFromDeleteQueueViewModel>().Result;
            return result;
        }

        [HttpGet("GetOrderCreatedFromQueue")]
        public GetMessageFromCreateQueueViewModel GetOrderCreatedFromQueue()
        {
            var result = serviceBusQueue.GetMessageFromCreateQueueAsync<GetMessageFromCreateQueueViewModel>().Result;
            return result;
        }

        [HttpGet("GetStartMessageFromDeleteQueue")]
        public void GetStartMessageFromDeleteQueue()
        {
            serviceBusQueue.GetStartMessageFromDeleteQueue<GetMessageFromDeleteQueueViewModel>();
            //serviceBus.GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>(); 
        }
        [HttpGet("GetStartMessageFromCreateQueue")]
        public void GetStartMessageFromCreateQueue()
        {
            serviceBusQueue.GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>();
        }

        [HttpGet("GetMessageFromOrderTopicCreatedSubscriptionAsync")]
        public async Task<GetMessageFromOrderTopicCreatedSubscriptionViewModel>  GetMessageFromOrderTopicCreatedSubscriptionAsync()
        {
           return await serviceBusTopic.GetMessageFromOrderTopicCreatedSubscriptionAsync<GetMessageFromOrderTopicCreatedSubscriptionViewModel>();
        }

        [HttpGet("GetMessageFromOrderTopicDeletedSubscriptionAsync")]
        public async Task<GetMessageFromOrderTopicDeletedSubscriptionViewModel> GetMessageFromOrderTopicDeletedSubscriptionAsync()
        {
            return await serviceBusTopic.GetMessageFromOrderTopicDeletedSubscriptionAsync<GetMessageFromOrderTopicDeletedSubscriptionViewModel>();
        }

    }
}
