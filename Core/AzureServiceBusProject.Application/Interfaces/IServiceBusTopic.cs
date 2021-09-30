using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Interfaces
{
    public interface IServiceBusTopic
    {
        public Task SendMessageToOrderTopicCreatedSubscription(object messageContent);
        public Task SendMessageToOrderTopicDeletedSubscription(object messageContent);
    }
}
