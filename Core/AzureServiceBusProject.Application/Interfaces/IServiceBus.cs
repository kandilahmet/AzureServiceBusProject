using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Interfaces
{
    public interface IServiceBus
    {
         
        //public Task SendMessageToQueueAsync(string queueName,object messageContent);
        public void SendMessageToCreateQueueAsync(object messageContent);
        public void SendMessageToDeleteQueueAsync(object messageContent);
        public void GetMessageFromDeleteQueue();
        public void GetMessageFromCreateQueue();
    }
}
