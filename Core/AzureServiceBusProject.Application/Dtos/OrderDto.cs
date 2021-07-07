using System;
using System.Collections.Generic;
using System.Text;

namespace AzureServiceBusProject.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
