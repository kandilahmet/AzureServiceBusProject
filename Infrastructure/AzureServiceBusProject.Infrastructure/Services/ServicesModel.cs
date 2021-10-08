using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServiceBusProject.Infrastructure.Services
{
    public class ServicesModel
    {
        public AzureServices AzureServices { get; set; }
          
    }
     public class AzureServices
    {
        public string AzureConnectionString { get; set; }
        public string OrderDeletedQueue { get; set; } 
        public string OrderCreatedQueue { get; set; }
        public string OrderTopic { get; set; }
        public string OrderCreatedSubscription { get; set; } 
        public string OrderCreatedFilter { get; set; }
        public string OrderDeletedSubscription { get; set; }
        public string OrderDeletedFilter { get; set; }
    }
    
}
