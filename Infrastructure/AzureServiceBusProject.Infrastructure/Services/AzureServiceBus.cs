using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBusProject.Application.Interfaces;
using Microsoft.Azure.ServiceBus;

namespace AzureServiceBusProject.Infrastructure.Services
{
    public class AzureServiceBus : IServiceBus

    {
        public async Task SendMessageToQueueAsync(string queueName, object messageContent)
        {
            IQueueClient queueClient = new QueueClient("","");
           // asa

        }
    }
}
