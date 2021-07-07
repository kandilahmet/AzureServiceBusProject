using AzureServiceBusProject.Application.Interfaces;
using AzureServiceBusProject.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text; 

namespace AzureServiceBusProject.Infrastructure
{
    public static class ServiceRegistration
    {
       public static void AddInfrastructureServices(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
            serviceCollection.AddSingleton<IServiceBus, AzureServiceBus>();
           // serviceCollection.Configure<ServicesModel>(configuration.GetSection("Services")); 
            //configuration.GetSection("Services")
        }
    }
}
