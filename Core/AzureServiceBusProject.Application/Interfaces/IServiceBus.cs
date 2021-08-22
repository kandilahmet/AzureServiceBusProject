using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Interfaces
{
    public interface IServiceBus
    {
        public Task CreateQueueIfNotExits(string QueueName); 
        public Task SendMessageToQueueAsync(string queueName,object messageContent);
        public Task GetMessageFromDeleteQueue(string queueName);
    }
}
