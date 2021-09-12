using AzureServiceBusProject.Application.Interfaces.Results;
using AzureServiceBusProject.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Interfaces
{
    public interface IServiceBus
    {
         
        //public Task SendMessageToQueueAsync(string queueName,object messageContent);
        public Task SendMessageToCreateQueueAsync(object messageContent);
        public Task SendMessageToDeleteQueueAsync(object messageContent);
        public Task<GetMessageFromDeleteQueueViewModel> GetMessageFromDeleteQueueAsync<GetMessageFromDeleteQueueViewModel>();
        public Task<GetMessageFromCreateQueueViewModel> GetMessageFromCreateQueueAsync<GetMessageFromCreateQueueViewModel>();
        public void GetStartMessageFromCreateQueue<GetMessageFromCreateQueueViewModel>();
        public void GetStartMessageFromDeleteQueue<GetMessageFromDeleteQueueViewModel>();
    }
}
