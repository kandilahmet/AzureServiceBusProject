using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusProject.Application.Events
{
    public class OrderEventBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
 
