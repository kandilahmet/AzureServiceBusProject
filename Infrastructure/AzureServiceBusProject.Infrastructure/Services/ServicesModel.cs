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
    }
    
}
