using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Interfaces
{
    public interface IServiceBusTopic
    {
        public Task SendMessageToOrderTopicCreatedSubscriptionAsync(object messageContent);
        public Task SendMessageToOrderTopicDeletedSubscriptionAsync(object messageContent);
        public Task<GetMessageFromCreatedSubscriptionViewModel> GetMessageFromOrderTopicCreatedSubscriptionAsync<GetMessageFromCreatedSubscriptionViewModel>();
        public Task<GetMessageFromDeleteSubscriptionViewModel> GetMessageFromOrderTopicDeletedSubscriptionAsync<GetMessageFromDeleteSubscriptionViewModel>();
    }
}
