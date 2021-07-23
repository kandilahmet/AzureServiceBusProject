using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Events
{
    public class OrderCreatedEvent: OrderEventBase
    { 
        public string ProductName { get; set; }
        
    }
}
